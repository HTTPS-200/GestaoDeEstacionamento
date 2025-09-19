using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;

public class LiberarVagaCommandHandler(
    IRepositorioVaga repositorioVaga,
    ILogger<LiberarVagaCommandHandler> logger
) : IRequestHandler<LiberarVagaCommand, Result<LiberarVagaResult>>
{
    public async Task<Result<LiberarVagaResult>> Handle(
        LiberarVagaCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var vaga = await repositorioVaga.SelecionarRegistroPorIdAsync(command.VagaId);
            if (vaga == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.VagaId));

            if (!vaga.Ocupada)
                return Result.Fail("Vaga já está liberada");

            vaga.Liberar();
            await repositorioVaga.EditarAsync(vaga.Id, vaga);

            var result = new LiberarVagaResult(
                vaga.Id,
                vaga.Identificador,
                vaga.Ocupada
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao liberar vaga");
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}