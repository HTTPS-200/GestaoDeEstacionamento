using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Handlers;

public class SelecionarVeiculoPorTicketQueryHandler : IRequestHandler<SelecionarVeiculoPorTicketQuery, Result<SelecionarVeiculoPorTicketResult>>
{
    private readonly IRepositorioVeiculo _repositorio;
    private readonly IMapper _mapper;
    private readonly ILogger<SelecionarVeiculoPorTicketQueryHandler> _logger;

    public SelecionarVeiculoPorTicketQueryHandler(
        IRepositorioVeiculo repositorio,
        IMapper mapper,
        ILogger<SelecionarVeiculoPorTicketQueryHandler> logger)
    {
        _repositorio = repositorio;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<SelecionarVeiculoPorTicketResult>> Handle(SelecionarVeiculoPorTicketQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var registro = await _repositorio.SelecionarRegistroPorIdAsync(query.Ticket);

            if (registro == null)
                return Result.Fail("Veículo não encontrado.");

            var result = _mapper.Map<SelecionarVeiculoPorTicketResult>(registro);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao selecionar veículo {@Veiculo}", query);
            return Result.Fail("Erro interno ao selecionar veículo.");
        }
    }
}
