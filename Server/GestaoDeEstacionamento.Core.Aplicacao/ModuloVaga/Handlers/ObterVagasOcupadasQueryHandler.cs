using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;

public class ObterVagasOcupadasQueryHandler(
    IRepositorioVaga repositorioVaga,
    IMapper mapper,
    ILogger<ObterVagasOcupadasQueryHandler> logger
) : IRequestHandler<ObterVagasOcupadasQuery, Result<ObterTodasVagasResult>>
{
    public async Task<Result<ObterTodasVagasResult>> Handle(
        ObterVagasOcupadasQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var vagas = await repositorioVaga.ObterVagasOcupadas();
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