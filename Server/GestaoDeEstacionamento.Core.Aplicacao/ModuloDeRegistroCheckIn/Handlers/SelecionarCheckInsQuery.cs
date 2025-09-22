using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using System.Collections.Immutable;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Comands;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Handlers
{
    public class SelecionarCheckInsQueryHandler :
        IRequestHandler<SelecionarCheckInsQuery, Result<SelecionarCheckInsResult>>
    {
        private readonly IRepositorioRegistroCheckIn _repositorioCheckIn;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly ILogger<SelecionarCheckInsQueryHandler> _logger;
        private readonly ITenantProvider _tenantProvider;

        public SelecionarCheckInsQueryHandler(
            IRepositorioRegistroCheckIn repositorioCheckIn,
            IMapper mapper,
            IDistributedCache cache,
            ILogger<SelecionarCheckInsQueryHandler> logger,
            ITenantProvider tenantProvider)
        {
            _repositorioCheckIn = repositorioCheckIn;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
            _tenantProvider = tenantProvider;
        }

        public async Task<Result<SelecionarCheckInsResult>> Handle(
            SelecionarCheckInsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var usuarioId = _tenantProvider.UsuarioId.GetValueOrDefault();
                var cacheKey = $"checkins:u={usuarioId}:q=all";

                var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);
                if (cachedData != null)
                {
                    var cachedResult = System.Text.Json.JsonSerializer.Deserialize<SelecionarCheckInsResult>(cachedData);
                    if (cachedResult != null)
                    {
                        return ApplyQuantityFilter(cachedResult, query.Quantidade);
                    }
                }

                var checkIns = await _repositorioCheckIn.ObterTodosAsync();

                var result = new SelecionarCheckInsResult(
                    checkIns.Select(c => new SelecionarCheckInsDto(
                        c.Id,
                        c.VeiculoId,
                        c.TicketId,
                        c.NumeroTicket,
                        c.DataHoraCheckIn,
                        c.Ativo
                    )).ToImmutableList()
                );

                var serializedResult = System.Text.Json.JsonSerializer.Serialize(result);
                await _cache.SetStringAsync(cacheKey, serializedResult,
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) },
                    cancellationToken);

                return ApplyQuantityFilter(result, query.Quantidade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao selecionar check-ins");
                return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
            }
        }

        private Result<SelecionarCheckInsResult> ApplyQuantityFilter(
            SelecionarCheckInsResult result, int? quantidade)
        {
            if (quantidade.HasValue && quantidade > 0)
            {
                var filteredCheckIns = result.CheckIns
                    .Take(quantidade.Value)
                    .ToImmutableList();

                return Result.Ok(new SelecionarCheckInsResult(filteredCheckIns));
            }

            return Result.Ok(result);
        }
    }
}