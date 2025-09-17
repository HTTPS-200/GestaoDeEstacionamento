using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;

public record ExcluirVeiculoCommand(Guid Ticket) : IRequest<Result<ExcluirVeiculoResult>>;

public record ExcluirVeiculoResult();
