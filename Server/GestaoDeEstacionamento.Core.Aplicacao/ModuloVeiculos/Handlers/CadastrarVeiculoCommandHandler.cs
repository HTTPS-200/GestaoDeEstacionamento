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

public class CadastrarVeiculoCommandHandler(
    IRepositorioVeiculo repositorioVeiculo,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IDistributedCache cache,
    IValidator<CadastrarVeiculoCommand> validator,
    ILogger<CadastrarVeiculoCommandHandler> logger
) : IRequestHandler<CadastrarVeiculoCommand, Result<CadastrarVeiculoResult>>
{
    public async Task<Result<CadastrarVeiculoResult>> Handle(
    CadastrarVeiculoCommand command, CancellationToken cancellationToken)
    {
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
            var erroFormatado = ResultadosErro.RequisicaoInvalidaErro(erros);
            return Result.Fail(erroFormatado);
        }

        try
        {
            // Log para debug
            logger.LogInformation("Iniciando cadastro de veículo: {@Command}", command);

            var veiculo = mapper.Map<Veiculo>(command);
            veiculo.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();

            logger.LogInformation("Veículo mapeado: {@Veiculo}", veiculo);

            await repositorioVeiculo.CadastrarAsync(veiculo);
            logger.LogInformation("Veículo cadastrado no repositório");

            await unitOfWork.CommitAsync();
            logger.LogInformation("Commit realizado com sucesso");

            // Invalida o cache
            var cacheKey = $"veiculos:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";
            await cache.RemoveAsync(cacheKey, cancellationToken);

            var result = mapper.Map<CadastrarVeiculoResult>(veiculo);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(ex, "Ocorreu um erro durante o registro do veículo {@Registro}.", command);

            // Log mais detalhado do erro interno
            if (ex.InnerException != null)
            {
                logger.LogError("Inner exception: {InnerException}", ex.InnerException.Message);
            }

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}