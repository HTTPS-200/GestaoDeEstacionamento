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

public class EditarVeiculoCommandHandler : IRequestHandler<EditarVeiculoCommand, Result<EditarVeiculoResult>>
{
    private readonly IRepositorioVeiculo _repositorio;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly IValidator<EditarVeiculoCommand> _validator;
    private readonly ILogger<EditarVeiculoCommandHandler> _logger;

    public EditarVeiculoCommandHandler(
        IRepositorioVeiculo repositorio,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IDistributedCache cache,
        IValidator<EditarVeiculoCommand> validator,
        ILogger<EditarVeiculoCommandHandler> logger)
    {
        _repositorio = repositorio;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<EditarVeiculoResult>> Handle(EditarVeiculoCommand command, CancellationToken cancellationToken)
    {
        var resultadoValidacao = await _validator.ValidateAsync(command, cancellationToken);
        if (!resultadoValidacao.IsValid)
            return Result.Fail(string.Join(", ", resultadoValidacao.Errors.Select(e => e.ErrorMessage)));

        try
        {
            var veiculoEditado = _mapper.Map<Veiculo>(command);

            await _repositorio.EditarAsync(command.Ticket, veiculoEditado);
            await _unitOfWork.CommitAsync();

            await _cache.RemoveAsync("veiculos:all", cancellationToken);

            var result = _mapper.Map<EditarVeiculoResult>(veiculoEditado);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Erro ao editar veículo {@Veiculo}", command);
            return Result.Fail("Erro interno ao editar veículo.");
        }
    }
}
