using AutoMapper;
using FluentResults;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Command;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Handlers;
public class CadastrarVagaCommandHandler(
    IValidator<CadastrarVagaCommand> validator,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IRepositorioVaga repositorioVaga,
    ILogger<CadastrarVagaCommandHandler> logger
    ) : IRequestHandler<CadastrarVagaCommand, Result<CadastrarVagaResult>>
{
    public async Task<Result<CadastrarVagaResult>> Handle(CadastrarVagaCommand command, CancellationToken cancellationToken)
    {
        var resultValidator = await validator.ValidateAsync(command);

        if (!resultValidator.IsValid)
        {
            var erros = resultValidator.Errors.Select(e => e.ErrorMessage);

            var errosFormatados = ResultadosErro.RequisicaoInvalidaErro(erros);

            return Result.Fail(errosFormatados);
        }

        var registros = await repositorioVaga.SelecionarRegistrosAsync();

        if (registros.Any(i => i.NumeroDaVaga.Equals(command.NumeroDaVaga, StringComparison.OrdinalIgnoreCase) && 
            i.Zona.Equals(command.Zona, StringComparison.OrdinalIgnoreCase)))
        {
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Essa vaga já foi registrada nesta zona."));
        }

        try
        {
            var vaga = mapper.Map<Vaga>(command);

            await repositorioVaga.CadastrarAsync(vaga);
            await unitOfWork.CommitAsync();

            var result = mapper.Map<CadastrarVagaResult>(vaga);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();

            logger.LogError(ex,"Ocorreu um erro durante o registro de: {@Registro}.",command);

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
