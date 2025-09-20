using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;

public class CriarVagaCommandHandler(
    IRepositorioVaga repositorioVaga,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IDistributedCache cache,
    IValidator<Vaga> validator, // <-- agora valida Vaga diretamente
    ILogger<CriarVagaCommandHandler> logger
) : IRequestHandler<CriarVagaCommand, Result<CriarVagaResult>>
{
    public async Task<Result<CriarVagaResult>> Handle(
        CriarVagaCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            // 1. Verifica duplicidade de identificador
            var vagaExistente = await repositorioVaga.ObterPorIdentificador(command.Identificador);
            if (vagaExistente != null)
                return Result.Fail(ResultadosErro.RegistroDuplicadoErro(
                    $"Já existe uma vaga com identificador {command.Identificador}"));

            // 2. Cria a entidade
            var vaga = new Vaga(command.Identificador, command.Zona, tenantProvider.UsuarioId.GetValueOrDefault());

            // 3. Validação da entidade
            var resultadoValidacao = await validator.ValidateAsync(vaga, cancellationToken);
            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            // 4. Persiste a vaga
            await repositorioVaga.CadastrarAsync(vaga);
            await unitOfWork.CommitAsync();

            // 5. Invalida caches
            await InvalidarCaches(vaga.UsuarioId);

            // 6. Retorna resultado
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
            await unitOfWork.RollbackAsync();
            logger.LogError(ex, "Erro ao criar vaga {@Registro}", command);
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    private async Task InvalidarCaches(Guid usuarioId)
    {
        var cacheKeys = new[]
        {
            $"vagas:u={usuarioId}:q=all"
        };

        foreach (var cacheKey in cacheKeys)
            await cache.RemoveAsync(cacheKey);
    }
}
