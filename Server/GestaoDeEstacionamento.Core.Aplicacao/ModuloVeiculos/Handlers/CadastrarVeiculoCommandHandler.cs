using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;
using FluentValidation;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Handlers;

public class CadastrarVeiculoCommandHandler : IRequestHandler<CadastrarVeiculoCommand, Result<CadastrarVeiculoResult>>
{
    private readonly IRepositorioVeiculo _repositorio;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly IValidator<CadastrarVeiculoCommand> _validator;
    private readonly ILogger<CadastrarVeiculoCommandHandler> _logger;

    public CadastrarVeiculoCommandHandler(
        IRepositorioVeiculo repositorio,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IDistributedCache cache,
        IValidator<CadastrarVeiculoCommand> validator,
        ILogger<CadastrarVeiculoCommandHandler> logger)
    {
        _repositorio = repositorio;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<CadastrarVeiculoResult>> Handle(CadastrarVeiculoCommand command, CancellationToken cancellationToken)
    {
        var resultadoValidacao = await _validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
            return Result.Fail(string.Join(", ", resultadoValidacao.Errors.Select(e => e.ErrorMessage)));

        try
        {
            var veiculo = _mapper.Map<Veiculo>(command);
            veiculo.DataEntrada = DateTime.UtcNow;

            await _repositorio.CadastrarAsync(veiculo);
            await _unitOfWork.CommitAsync();

            await _cache.RemoveAsync("veiculos:all", cancellationToken);

            var result = _mapper.Map<CadastrarVeiculoResult>(veiculo);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Erro ao cadastrar veículo {@Veiculo}", command);
            return Result.Fail("Erro interno ao cadastrar veículo.");
        }
    }
}
