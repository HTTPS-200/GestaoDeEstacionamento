using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Handlers;

public class RealizarCheckInCommandHandler(
    IRepositorioVeiculo repositorioVeiculo,
    IRepositorioTicket repositorioTicket,
    IRepositorioRegistroCheckIn repositorioCheckIn,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IDistributedCache cache,
    IValidator<RealizarCheckInCommand> validator,
    ILogger<RealizarCheckInCommandHandler> logger
) : IRequestHandler<RealizarCheckInCommand, Result<RealizarCheckInResult>>
{
    public async Task<Result<RealizarCheckInResult>> Handle(
        RealizarCheckInCommand command, CancellationToken cancellationToken)
    {
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
            var erroFormatado = ResultadosErro.RequisicaoInvalidaErro(erros);
            return Result.Fail(erroFormatado);
        }

        try
        {
            // Verifica se já existe um check-in ativo para esta placa
            var checkInAtivo = await repositorioCheckIn.ObterPorPlacaVeiculo(command.Placa);
            if (checkInAtivo != null)
            {
                return Result.Fail(ResultadosErro.RegistroDuplicadoErro(
                    $"Já existe um check-in ativo para o veículo com placa {command.Placa}"));
            }

            // Verifica se o veículo já existe no sistema
            var veiculoExistente = await repositorioVeiculo.ObterPorPlaca(command.Placa);
            Veiculo veiculo;

            if (veiculoExistente != null && veiculoExistente.Any())
            {
                // Usa o veículo existente
                veiculo = veiculoExistente.First();

                // Atualiza o CPF do hóspede se necessário
                if (veiculo.CPFHospede != command.CPFHospede)
                {
                    veiculo.CPFHospede = command.CPFHospede;
                    await repositorioVeiculo.EditarAsync(veiculo.Id, veiculo);
                }
            }
            else
            {
                // Cria um novo veículo com dados básicos
                // Modelo e cor podem ser obtidos de uma API externa ou deixados em branco
                veiculo = new Veiculo(
                    command.Placa,
                    "Não informado", // Modelo padrão
                    "Não informada", // Cor padrão
                    command.CPFHospede
                );
                veiculo.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();
                await repositorioVeiculo.CadastrarAsync(veiculo);
            }

            // Gera o número do ticket
            var ultimoNumero = await repositorioTicket.ObterUltimoNumeroSequencial();
            var proximoNumero = ultimoNumero + 1;
            var numeroTicket = proximoNumero.ToString("D6");

            // Cria o ticket
            var ticket = new Ticket(numeroTicket, veiculo.Id, proximoNumero);
            ticket.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();
            await repositorioTicket.CadastrarAsync(ticket);

            // Cria o registro de check-in
            var registroCheckIn = new RegistroCheckIn(veiculo, ticket);
            registroCheckIn.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();
            await repositorioCheckIn.CadastrarAsync(registroCheckIn);

            await unitOfWork.CommitAsync();

            // Invalida caches
            await InvalidarCaches(tenantProvider.UsuarioId.GetValueOrDefault());

            var result = new RealizarCheckInResult(
                registroCheckIn.Id,
                veiculo.Id,
                ticket.Id,
                numeroTicket,
                registroCheckIn.DataHoraCheckIn
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(ex, "Ocorreu um erro durante o check-in do veículo {@Registro}.", command);
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
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
            await cache.RemoveAsync(cacheKey);
        }
    }
}