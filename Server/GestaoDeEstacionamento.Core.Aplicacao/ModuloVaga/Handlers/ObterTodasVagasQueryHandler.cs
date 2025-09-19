using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;

public class ObterTodasVagasQueryHandler(
    IRepositorioVaga repositorioVaga,
    IMapper mapper,
    ILogger<ObterTodasVagasQueryHandler> logger
) : IRequestHandler<ObterTodasVagasQuery, Result<ObterTodasVagasResult>>
{
    public async Task<Result<ObterTodasVagasResult>> Handle(
        ObterTodasVagasQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var vagas = await repositorioVaga.SelecionarRegistrosAsync();
            var result = mapper.Map<ObterTodasVagasResult>(vagas);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter todas as vagas");
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}