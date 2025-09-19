using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Handlers;

public class EditarTicketCommandHandler(
    IRepositorioTicket repositorioTicket,
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
            var erroFormatado = ResultadosErro.RequisicaoInvalidaErro(erros);
            return Result.Fail(erroFormatado);
        }

        try
        {
            // Verifica se outro ticket já usa este número
            var ticketComMesmoNumero = await repositorioTicket.ObterPorNumero(command.PlacaVeiculo);
            if (ticketComMesmoNumero != null && ticketComMesmoNumero.Id != command.Id)
            {
                return Result.Fail(ResultadosErro.RegistroDuplicadoErro(
                    $"Já existe um ticket com o número {command.PlacaVeiculo}"));
            }

            var ticketEditado = mapper.Map<Ticket>(command);
            await repositorioTicket.EditarAsync(command.Id, ticketEditado);
            await unitOfWork.CommitAsync();

            // Invalida cache
            var cacheKey = $"tickets:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";
            await cache.RemoveAsync(cacheKey, cancellationToken);

            var result = mapper.Map<EditarTicketResult>(ticketEditado);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(ex, "Ocorreu um erro durante a edição do ticket {@Registro}.", command);
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}