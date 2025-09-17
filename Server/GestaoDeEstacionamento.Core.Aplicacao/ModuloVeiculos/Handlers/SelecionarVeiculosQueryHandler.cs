using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;
using System.Text.Json;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Handlers;

public class SelecionarVeiculosQueryHandler : IRequestHandler<SelecionarVeiculosQuery, Result<SelecionarVeiculosResult>>
{
    private readonly IRepositorioVeiculo _repositorio;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly ILogger<SelecionarVeiculosQueryHandler> _logger;

    public SelecionarVeiculosQueryHandler(
        IRepositorioVeiculo repositorio,
        IMapper mapper,
        IDistributedCache cache,
        ILogger<SelecionarVeiculosQueryHandler> logger)
    {
        _repositorio = repositorio;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<SelecionarVeiculosResult>> Handle(SelecionarVeiculosQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var cacheKey = query.Quantidade.HasValue ? $"veiculos:q={query.Quantidade.Value}" : "veiculos:all";

            var jsonString = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                var resultadoEmCache = JsonSerializer.Deserialize<SelecionarVeiculosResult>(jsonString);
                if (resultadoEmCache != null) return Result.Ok(resultadoEmCache);
            }

            var registros = query.Quantidade.HasValue ?
                await _repositorio.SelecionarRegistrosAsync(query.Quantidade.Value) :
                await _repositorio.SelecionarRegistrosAsync();

            var result = _mapper.Map<SelecionarVeiculosResult>(registros);

            var jsonPayload = JsonSerializer.Serialize(result);
            await _cache.SetStringAsync(cacheKey, jsonPayload, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60) }, cancellationToken);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao selecionar veículos {@Query}", query);
            return Result.Fail("Erro interno ao selecionar veículos.");
        }
    }
}
