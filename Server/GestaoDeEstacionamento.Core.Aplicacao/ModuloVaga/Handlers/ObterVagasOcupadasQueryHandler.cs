using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;

public class ObterVagasOcupadasQueryHandler(
    IRepositorioVaga repositorioVaga,
    IMapper mapper,
    IDistributedCache cache,
    ITenantProvider tenantProvider,
    ILogger<ObterVagasOcupadasQueryHandler> logger
) : IRequestHandler<ObterVagasOcupadasQuery, Result<ObterTodasVagasResult>>
{
    public async Task<Result<ObterTodasVagasResult>> Handle(
        ObterVagasOcupadasQuery query, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Buscando vagas ocupadas para usuário: {UsuarioId}",
                tenantProvider.UsuarioId);

            var vagas = await repositorioVaga.ObterVagasOcupadas();

            logger.LogInformation("Encontradas {Count} vagas ocupadas", vagas.Count);

            var result = mapper.Map<ObterTodasVagasResult>(vagas);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter vagas ocupadas");
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
