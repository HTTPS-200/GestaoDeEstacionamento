using FluentResults;
using MediatR;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using Microsoft.Extensions.Caching.Distributed;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckOut.Commands;

public class RealizarCheckOutCommandHandler : IRequestHandler<RealizarCheckOutCommand, Result<RealizarCheckOutResult>>
{
    private readonly IRepositorioRegistroCheckIn _repositorioCheckIn;
    private readonly IRepositorioVeiculo _repositorioVeiculo;
    private readonly IRepositorioTicket _repositorioTicket;
    private readonly IRepositorioVaga _repositorioVaga;
    private readonly IMediator _mediator;
    private readonly IDistributedCache _cache;
    private readonly IUnitOfWork _unitOfWork;

    public RealizarCheckOutCommandHandler(
        IRepositorioRegistroCheckIn repositorioCheckIn,
        IRepositorioVeiculo repositorioVeiculo,
        IRepositorioTicket repositorioTicket,
        IRepositorioVaga repositorioVaga,
        IMediator mediator,
        IDistributedCache cache,
        IUnitOfWork unitOfWork) 
    {
        _repositorioCheckIn = repositorioCheckIn;
        _repositorioVeiculo = repositorioVeiculo;
        _repositorioTicket = repositorioTicket;
        _repositorioVaga = repositorioVaga;
        _mediator = mediator;
        _cache = cache;
        _unitOfWork = unitOfWork; 
    }

    public async Task<Result<RealizarCheckOutResult>> Handle(RealizarCheckOutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar veículo por placa e CPF
            var veiculo = await ObterVeiculoPorPlacaECpf(request.PlacaVeiculo, request.CPFHospede);
            if (veiculo == null)
                return Result.Fail("Veículo não encontrado para o CPF informado");

            // Buscar check-in ativo para o veículo
            var checkIn = await _repositorioCheckIn.ObterCheckInsPorVeiculoId(veiculo.Id)
                .ContinueWith(task => task.Result?.FirstOrDefault(c => c.Ativo));

            if (checkIn == null)
                return Result.Fail("Check-in ativo não encontrado para o veículo");

            // Buscar ticket
            var ticket = await _repositorioTicket.SelecionarRegistroPorIdAsync(checkIn.TicketId);
            if (ticket == null)
                return Result.Fail("Ticket não encontrado");

            // Buscar vaga (se existir)
            VagaInfoResult? vagaInfo = null;
            var vaga = await _repositorioVaga.ObterPorVeiculoId(veiculo.Id);
            if (vaga != null)
            {
                vagaInfo = new VagaInfoResult(vaga.Identificador, vaga.Zona);
            }

            var (diarias, valorTotal) = CalcularDiariasECusto(checkIn.DataHoraCheckIn, DateTime.UtcNow);

            Console.WriteLine($"ANTES - Ticket Ativo: {ticket.Ativo}, VeiculoId: {ticket.VeiculoId}");

            checkIn.EncerrarCheckIn();
            veiculo.RegistrarSaida();
            ticket.Encerrar();

            Console.WriteLine($"DEPOIS - Ticket Ativo: {ticket.Ativo}, VeiculoId: {ticket.VeiculoId}");

            if (vaga != null)
                vaga.Liberar();

            var ticketEditado = await _repositorioTicket.EditarAsync(ticket.Id, ticket);
            var checkInEditado = await _repositorioCheckIn.EditarAsync(checkIn.Id, checkIn);
            var veiculoEditado = await _repositorioVeiculo.EditarAsync(veiculo.Id, veiculo);

            bool vagaEditada = true;
            if (vaga != null)
                vagaEditada = await _repositorioVaga.EditarAsync(vaga.Id, vaga);

            // Verificar se todas as edições foram bem-sucedidas
            if (!checkInEditado || !veiculoEditado || !vagaEditada || !ticketEditado)
                return Result.Fail("Falha ao atualizar registros no banco de dados");

            // CRIAR FATURA APÓS CHECKOUT BEM-SUCEDIDO
            var criarFaturaCommand = new CriarFaturaCommand(
                checkIn.Id,
                veiculo.Id,
                ticket.Id,
                ticket.NumeroTicket,
                veiculo.Placa,
                veiculo.Modelo,
                veiculo.Cor,
                veiculo.CPFHospede,
                vaga?.Identificador,
                vaga?.Zona,
                checkIn.DataHoraCheckIn,
                DateTime.UtcNow,
                diarias,
                50.00m,
                valorTotal
            );

            var faturaResult = await _mediator.Send(criarFaturaCommand, cancellationToken);

            if (faturaResult.IsFailed)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Fail("Falha ao criar fatura: " + string.Join(", ", faturaResult.Errors));
            }

            await _unitOfWork.CommitAsync();
            await InvalidarCaches(veiculo.UsuarioId);

            return Result.Ok(new RealizarCheckOutResult(
                checkIn.Id,
                veiculo.Id,
                ticket.Id,
                veiculo.Placa,
                ticket.NumeroTicket,
                vagaInfo,
                checkIn.DataHoraCheckIn,
                DateTime.UtcNow,
                diarias,
                valorTotal,
                checkIn.Ativo
            ));
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result.Fail($"Erro ao realizar checkout: {ex.Message}");
        }
    }

    private async Task<Veiculo?> ObterVeiculoPorPlacaECpf(string placa, string cpfHospede)
    {
        var veiculos = await _repositorioVeiculo.ObterPorPlaca(placa);
        return veiculos?.FirstOrDefault(v => v.CPFHospede == cpfHospede);
    }

    private (int Diarias, decimal ValorTotal) CalcularDiariasECusto(DateTime entrada, DateTime saida)
    {
        var diasCompletos = (saida.Date - entrada.Date).Days;

        if (saida.TimeOfDay > TimeSpan.Zero || diasCompletos == 0)
            diasCompletos++;

        const decimal valorDiaria = 50.00m;
        var valorTotal = diasCompletos * valorDiaria;

        return (diasCompletos, valorTotal);
    }

    private async Task InvalidarCaches(Guid usuarioId)
    {
        var cacheKeys = new[]
        {
        $"veiculos:u={usuarioId}:q=all",
        $"tickets:u={usuarioId}:q=all",
        $"checkins:u={usuarioId}:q=all"
    };

        foreach (var cacheKey in cacheKeys)
        {
            await _cache.RemoveAsync(cacheKey);
        }
    }

}