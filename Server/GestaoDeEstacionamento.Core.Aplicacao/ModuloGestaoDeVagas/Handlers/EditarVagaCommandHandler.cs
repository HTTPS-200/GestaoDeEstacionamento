using AutoMapper;
using FluentResults;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Command;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Handlers;
public class EditarVagaCommandHandler(
    IRepositorioVaga repositorioVaga,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<EditarVagaCommand> validator,
    ILogger<EditarVagaCommandHandler> logger
) : IRequestHandler<EditarVagaCommand, Result<EditarVagaResult>>
{
    public async Task<Result<EditarVagaResult>> Handle(EditarVagaCommand command, CancellationToken cancellationToken)
    {

        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);
        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
            return Result.Fail(new Error("Erro de validação").WithMetadata("Erros", erros));
        }

        var vagaExistente = await repositorioVaga.SelecionarRegistroPorIdAsync(command.Id);
        if (vagaExistente is null)
            return Result.Fail("Vaga não encontrada.");

        vagaExistente.Zona = command.Zona;
        vagaExistente.Ocupada = command.Ocupada;
        vagaExistente.VeiculoEstacionado = command.Veiculo;

        try
        {
            await repositorioVaga.EditarAsync(command.Id, vagaExistente);
            await unitOfWork.CommitAsync();

            var result = mapper.Map<EditarVagaResult>(vagaExistente);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(ex, "Erro ao editar vaga {@Vaga}", command);
            return Result.Fail("Erro interno ao editar vaga.");
        }
    }
}