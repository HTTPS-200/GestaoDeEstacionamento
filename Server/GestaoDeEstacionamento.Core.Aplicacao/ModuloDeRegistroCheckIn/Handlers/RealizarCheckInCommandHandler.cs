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
            // 1. Busca o veículo pela placa
            var veiculos = await repositorioVeiculo.ObterPorPlaca(command.PlacaVeiculo);
            var veiculo = veiculos.FirstOrDefault();

            if (veiculo == null)
            {
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(
                    $"Veículo com placa {command.PlacaVeiculo} não encontrado"));
            }

            // 2. Busca o ticket EXISTENTE para este veículo (não cria novo)
            var ticketsDoVeiculo = await repositorioTicket.ObterPorVeiculoId(veiculo.Id);
            var ticket = ticketsDoVeiculo.FirstOrDefault(t => t.Ativo); // ← Só tickets ATIVOS

            if (ticket == null)
            {
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(
                    $"Não existe ticket ATIVO para o veículo com placa {command.PlacaVeiculo}. Crie um ticket primeiro."));
            }

            // 3. Verifica se já existe check-in ativo para este ticket
            var checkInAtivo = await repositorioCheckIn.ObterPorNumeroTicket(ticket.NumeroTicket);
            if (checkInAtivo != null && checkInAtivo.Ativo)
            {
                return Result.Fail(ResultadosErro.RegistroDuplicadoErro(
                    $"Já existe um check-in ativo para o ticket {ticket.NumeroTicket}"));
            }

            // 4. Cria o registro de check-in usando o ticket EXISTENTE
            var registroCheckIn = new RegistroCheckIn(veiculo, ticket);
            registroCheckIn.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();

            await repositorioCheckIn.CadastrarAsync(registroCheckIn);
            await unitOfWork.CommitAsync();

            // 5. Invalida caches
            await InvalidarCaches(tenantProvider.UsuarioId.GetValueOrDefault());

            var result = new RealizarCheckInResult(
                registroCheckIn.Id,
                veiculo.Id,
                ticket.Id,
                ticket.NumeroTicket, 
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