using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;

public class ObterVagasLivresQueryHandler(
    IRepositorioVaga repositorioVaga,
    IMapper mapper,
    ILogger<ObterVagasLivresQueryHandler> logger
) : IRequestHandler<ObterVagasLivresQuery, Result<ObterTodasVagasResult>>
{
    public async Task<Result<ObterTodasVagasResult>> Handle(
        ObterVagasLivresQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var vagas = await repositorioVaga.ObterVagasLivres();
            var result = mapper.Map<ObterTodasVagasResult>(vagas);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter vagas livres");
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}