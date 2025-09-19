using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;

public class EditarVagaCommandHandler(
    IRepositorioVaga repositorioVaga,
    IMapper mapper,
    ILogger<EditarVagaCommandHandler> logger
) : IRequestHandler<EditarVagaCommand, Result<EditarVagaResult>>
{
    public async Task<Result<EditarVagaResult>> Handle(
        EditarVagaCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var vaga = await repositorioVaga.SelecionarRegistroPorIdAsync(command.Id);
            if (vaga == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

            vaga.Identificador = command.Identificador;
            vaga.Zona = command.Zona;
            vaga.Ocupada = command.Ocupada;
            vaga.VeiculoId = command.VeiculoId;

            await repositorioVaga.EditarAsync(vaga.Id, vaga);

            var result = new EditarVagaResult(
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
            logger.LogError(ex, "Erro ao editar vaga");
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}