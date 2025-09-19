using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;

public class OcuparVagaCommandHandler(
    IRepositorioVaga repositorioVaga,
    ILogger<OcuparVagaCommandHandler> logger
) : IRequestHandler<OcuparVagaCommand, Result<OcuparVagaResult>>
{
    public async Task<Result<OcuparVagaResult>> Handle(
        OcuparVagaCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var vaga = await repositorioVaga.SelecionarRegistroPorIdAsync(command.VagaId);
            if (vaga == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.VagaId));

            if (vaga.Ocupada)
                return Result.Fail("Vaga já está ocupada");

            vaga.Ocupar(command.VeiculoId);
            await repositorioVaga.EditarAsync(vaga.Id, vaga);

            var result = new OcuparVagaResult(
                vaga.Id,
                vaga.Identificador,
                vaga.Ocupada,
                command.VeiculoId
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao ocupar vaga");
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}