using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;

public class ExcluirVagaCommandHandler(
    IRepositorioVaga repositorioVaga,
    ILogger<ExcluirVagaCommandHandler> logger
) : IRequestHandler<ExcluirVagaCommand, Result<ExcluirVagaResult>>
{
    public async Task<Result<ExcluirVagaResult>> Handle(
        ExcluirVagaCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var vaga = await repositorioVaga.SelecionarRegistroPorIdAsync(command.Id);
            if (vaga == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

            if (vaga.Ocupada)
                return Result.Fail("Não é possível excluir uma vaga ocupada");

            await repositorioVaga.ExcluirAsync(command.Id);

            return Result.Ok(new ExcluirVagaResult());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao excluir vaga");
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}