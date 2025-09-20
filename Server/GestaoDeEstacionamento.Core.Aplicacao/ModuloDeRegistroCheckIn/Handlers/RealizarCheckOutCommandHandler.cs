using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Comands;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using MediatR;
using Microsoft.AspNetCore.RateLimiting;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Handlers;
public class RealizarCheckOutCommandHandler : IRequestHandler<RealizarCheckOutCommand, Result<RealizarCheckOutResult>>
{
    private readonly IRepositorioRegistroCheckIn _repositorioRegistroCheckIn;
    private readonly IRepositorioVeiculo _repositorioVeiculo;
    private readonly IRepositorioVaga _repositorioVaga;
    private readonly IRepositorioTicket _repositorioTicket;
    private readonly IMediator _mediator;

    public RealizarCheckOutCommandHandler(
        IRepositorioRegistroCheckIn repositorioRegistroCheckIn,
        IRepositorioVeiculo repositorioVeiculo,
        IRepositorioVaga repositorioVaga,
        IRepositorioTicket repositorioTicket,
        IMediator mediator
        )
    {    
        _repositorioRegistroCheckIn = repositorioRegistroCheckIn;
        _repositorioVaga = repositorioVaga;
        _repositorioVeiculo = repositorioVeiculo;
        _repositorioTicket = repositorioTicket;
        _mediator = mediator;
    }

    public async Task<Result<RealizarCheckOutResult>> Handle(RealizarCheckOutCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var veiculo = await ObterVeiculoPorPlacaECpf(request.PlacaVeiculo, request.CPFHospede);

            if (veiculo == null)
                return Result.Fail("Veículo não encontrado.");

            var checkin = await _repositorioRegistroCheckIn.ObterCheckInsPorVeiculoId(veiculo.Id)
                .ContinueWith(task => task.Result?.FirstOrDefault(c => c.Ativo));

            if (checkin == null)
                return Result.Fail("CheckIn Ativo Inválido");

            var ticket = await _repositorioTicket.SelecionarRegistroPorIdAsync(checkin.TicketId);

            if (ticket == null)
                return Result.Fail("Ticket não encontrado.");

            VagaInfoResult? vagaInfo = null;

            var vaga = await _repositorioVaga.ObterPorVeiculoId(veiculo.Id);

            if (vaga != null)
            {
                vagaInfo = new VagaInfoResult(vaga.Identificador, vaga.Zona);
            }

            var (diarias, valorTotal) = CalcularDiariasECusto(checkin.DataHoraCheckIn, DateTime.UtcNow);

            checkin.EncerrarCheckIn();
            veiculo.RegistrarSaida();

            if (vaga != null)
                vaga.Liberar();

            var checkInEditado = await _repositorioRegistroCheckIn.EditarAsync(checkin.Id, checkin);
            var veiculoEditado = await _repositorioVeiculo.EditarAsync(veiculo.Id, veiculo);
            var ticketEditado = await _repositorioTicket.EditarAsync(ticket.Id, ticket);

            bool vagaEditada = true;

            if (vaga != null)
                vagaEditada = await _repositorioVaga.EditarAsync(vaga.Id, vaga);

            if (!checkInEditado || !vagaEditada || !veiculoEditado)
                return Result.Fail("Falha ao realizar o checkOut.");

            return Result.Ok(new RealizarCheckOutResult(
                checkin.Id,
                veiculo.Id,
                ticket.Id,
                veiculo.Placa,
                ticket.NumeroTicket,
                checkin.DataHoraCheckIn,
                DateTime.UtcNow,
                diarias,
                valorTotal,
                checkin.Ativo,
                vagaInfo
                ));
        }
        catch(Exception ex)
        {
            return Result.Fail($"Erro ao realizar Check-Out {ex.Message}");       
        }
    }

    private async Task<Veiculo?> ObterVeiculoPorPlacaECpf(string placa, string cpfHospede)
    {
        var veiculo = await _repositorioVeiculo.ObterPorPlaca(placa);

        return veiculo?.FirstOrDefault(v => v.CPFHospede == cpfHospede);
    }

    private(int Diarias, decimal ValorTotal)CalcularDiariasECusto(DateTime entrada, DateTime saida)
    {
        var diasCompletos = (saida.Date - entrada.Date).Days;

        if (saida.TimeOfDay > TimeSpan.Zero || diasCompletos == 0)
            diasCompletos++;

        const decimal valorDiaria = 50.00m;

        var valorTotal = diasCompletos * valorDiaria;

        return (diasCompletos, valorTotal);
    }
}
