using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;

public class CriarVagaCommandHandler(
    IRepositorioVaga repositorioVaga,
    IMapper mapper,
    ILogger<CriarVagaCommandHandler> logger
) : IRequestHandler<CriarVagaCommand, Result<CriarVagaResult>>
{
    public async Task<Result<CriarVagaResult>> Handle(
        CriarVagaCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var vagaExistente = await repositorioVaga.ObterPorIdentificador(command.Identificador);
            if (vagaExistente != null)
                return Result.Fail("Já existe uma vaga com este identificador");

            var vaga = new Vaga(command.Identificador, command.Zona);
            await repositorioVaga.CadastrarAsync(vaga);

            var result = new CriarVagaResult(
                vaga.Id,
                vaga.Identificador,
                vaga.Zona,
                vaga.Ocupada
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao criar vaga");
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}