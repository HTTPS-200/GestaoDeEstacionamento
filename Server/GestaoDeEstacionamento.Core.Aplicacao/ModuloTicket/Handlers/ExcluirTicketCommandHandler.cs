using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Handlers;

public class ExcluirTicketCommandHandler(
    IRepositorioTicket repositorioTicket,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IDistributedCache cache,
    ILogger<ExcluirTicketCommandHandler> logger
) : IRequestHandler<ExcluirTicketCommand, Result<ExcluirTicketResult>>
{
    public async Task<Result<ExcluirTicketResult>> Handle(
        ExcluirTicketCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await repositorioTicket.ExcluirAsync(command.Id);
            await unitOfWork.CommitAsync();

            var cacheKey = $"tickets:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";
            await cache.RemoveAsync(cacheKey, cancellationToken);

            var result = new ExcluirTicketResult();
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(ex, "Ocorreu um erro durante a exclusão do ticket {@Registro}.", command);
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}