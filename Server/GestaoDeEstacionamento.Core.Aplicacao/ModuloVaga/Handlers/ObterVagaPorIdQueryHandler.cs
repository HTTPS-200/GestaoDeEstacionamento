using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;

public class ObterVagaPorIdQueryHandler(
    IRepositorioVaga repositorioVaga,
    ILogger<ObterVagaPorIdQueryHandler> logger
) : IRequestHandler<ObterVagaPorIdQuery, Result<ObterVagaPorIdResult>>
{
    public async Task<Result<ObterVagaPorIdResult>> Handle(
        ObterVagaPorIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var vaga = await repositorioVaga.SelecionarRegistroPorIdAsync(query.Id);
            if (vaga == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

            var result = new ObterVagaPorIdResult(
                vaga.Id,
                vaga.Identificador,
                vaga.Zona,
                vaga.Ocupada,
                vaga.VeiculoId
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter vaga por ID");
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}