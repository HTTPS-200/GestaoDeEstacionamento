using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Command;
using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Handlers;
public class SelecionarVagaQueryHandler(
    IMapper mapper,
    IRepositorioVaga repositorioVaga,
    ILogger<SelecionarVagaQueryHandler> logger
) : IRequestHandler<SelecionarVagasQuery, Result<SelecionarVagasResult>>
{
    public async Task<Result<SelecionarVagasResult>> Handle(SelecionarVagasQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var registros = query.Quantidade.HasValue ?
                await repositorioVaga.SelecionarRegistrosAsync(query.Quantidade.Value) :
                await repositorioVaga.SelecionarRegistrosAsync();

            var result = mapper.Map<SelecionarVagasResult>(registros);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro na seleção de {@Registros}.", query);

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}

