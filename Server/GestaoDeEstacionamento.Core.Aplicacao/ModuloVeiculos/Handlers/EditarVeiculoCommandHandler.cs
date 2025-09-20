using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Handlers;

public class EditarVeiculoCommandHandler(
    IRepositorioVeiculo repositorioVeiculo,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IDistributedCache cache,
    IValidator<EditarVeiculoCommand> validator,
    ILogger<EditarVeiculoCommandHandler> logger
) : IRequestHandler<EditarVeiculoCommand, Result<EditarVeiculoResult>>
{
    public async Task<Result<EditarVeiculoResult>> Handle(
        EditarVeiculoCommand command, CancellationToken cancellationToken)
    {
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
            var erroFormatado = ResultadosErro.RequisicaoInvalidaErro(erros);
            return Result.Fail(erroFormatado);
        }

        var registros = await repositorioVeiculo.SelecionarRegistrosAsync();

        if (registros.Any(v => !v.Id.Equals(command.Id) && v.Placa.Equals(command.Placa)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um veículo registrado com esta placa."));

        try
        {
            var veiculoEditado = mapper.Map<Veiculo>(command);
            await repositorioVeiculo.EditarAsync(command.Id, veiculoEditado);
            await unitOfWork.CommitAsync();

            var cacheKey = $"veiculos:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";
            await cache.RemoveAsync(cacheKey, cancellationToken);

            var result = mapper.Map<EditarVeiculoResult>(veiculoEditado);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(ex, "Ocorreu um erro durante a edição do veículo {@Registro}.", command);
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}