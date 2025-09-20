using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;

public class EditarTicketCommandHandler(
    IRepositorioTicket repositorioTicket,
    IRepositorioVeiculo repositorioVeiculo,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IDistributedCache cache,
    IValidator<EditarTicketCommand> validator,
    ILogger<EditarTicketCommandHandler> logger
) : IRequestHandler<EditarTicketCommand, Result<EditarTicketResult>>
{
    public async Task<Result<EditarTicketResult>> Handle(
        EditarTicketCommand command, CancellationToken cancellationToken)
    {
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
            return Result.Fail(string.Join("; ", erros));
        }

        try
        {
            var ticketExistente = await repositorioTicket.SelecionarRegistroPorIdAsync(command.Id);
            if (ticketExistente == null)
                return Result.Fail("Ticket não encontrado.");

            if (!string.IsNullOrWhiteSpace(command.PlacaVeiculo))
            {
                var veiculos = await repositorioVeiculo.ObterPorPlaca(command.PlacaVeiculo);

                if (veiculos == null || veiculos.Count == 0)
                    return Result.Fail("Veículo não encontrado.");

                if (veiculos.Count > 1)
                    return Result.Fail("Mais de um veículo encontrado com a mesma placa.");

                ticketExistente.VeiculoId = veiculos.First().Id;
            }

            ticketExistente.Ativo = command.Ativo;

            await repositorioTicket.EditarAsync(command.Id, ticketExistente);
            await unitOfWork.CommitAsync();

            var cacheKey = $"tickets:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";
            await cache.RemoveAsync(cacheKey, cancellationToken);

            var result = new EditarTicketResult(
                ticketExistente.Id,
                command.PlacaVeiculo ?? string.Empty,
                ticketExistente.NumeroTicket,
                ticketExistente.Ativo
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(ex, "Erro durante a edição do ticket {@Registro}", command);
            return Result.Fail($"Erro interno: {ex.Message}");
        }
    }
}
