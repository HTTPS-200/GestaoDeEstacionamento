using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Handlers;

public class SelecionarVeiculosQueryHandler(
    IRepositorioVeiculo repositorioVeiculo,
    ITenantProvider tenantProvider,
    IMapper mapper,
    IDistributedCache cache,
    ILogger<SelecionarVeiculosQueryHandler> logger
) : IRequestHandler<SelecionarVeiculosQuery, Result<SelecionarVeiculosResult>>
{
    public async Task<Result<SelecionarVeiculosResult>> Handle(
        SelecionarVeiculosQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var cacheQuery = query.Quantidade.HasValue ? $"q={query.Quantidade.Value}" : "q=all";
            var cacheKey = $"veiculos:u={tenantProvider.UsuarioId.GetValueOrDefault()}:{cacheQuery}";

            var jsonString = await cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                var resultadoEmCache = JsonSerializer.Deserialize<SelecionarVeiculosResult>(jsonString);
                if (resultadoEmCache is not null)
                    return Result.Ok(resultadoEmCache);
            }

            var registros = query.Quantidade.HasValue ?
                await repositorioVeiculo.SelecionarRegistrosAsync(query.Quantidade.Value) :
                await repositorioVeiculo.SelecionarRegistrosAsync();

            var result = mapper.Map<SelecionarVeiculosResult>(registros);

            var jsonPayload = JsonSerializer.Serialize(result);
            var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60) };
            await cache.SetStringAsync(cacheKey, jsonPayload, cacheOptions, cancellationToken);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante a seleção dos veículos {@Registros}.", query);
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}