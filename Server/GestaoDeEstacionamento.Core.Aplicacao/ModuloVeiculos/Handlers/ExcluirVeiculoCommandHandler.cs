using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Handlers;

public class ExcluirVeiculoCommandHandler : IRequestHandler<ExcluirVeiculoCommand, Result<ExcluirVeiculoResult>>
{
    private readonly IRepositorioVeiculo _repositorio;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDistributedCache _cache;
    private readonly ILogger<ExcluirVeiculoCommandHandler> _logger;

    public ExcluirVeiculoCommandHandler(
        IRepositorioVeiculo repositorio,
        IUnitOfWork unitOfWork,
        IDistributedCache cache,
        ILogger<ExcluirVeiculoCommandHandler> logger)
    {
        _repositorio = repositorio;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<ExcluirVeiculoResult>> Handle(ExcluirVeiculoCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _repositorio.ExcluirAsync(command.Ticket);
            await _unitOfWork.CommitAsync();

            await _cache.RemoveAsync("veiculos:all", cancellationToken);

            return Result.Ok(new ExcluirVeiculoResult());
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Erro ao excluir veículo {@Veiculo}", command);
            return Result.Fail("Erro interno ao excluir veículo.");
        }
    }
}
