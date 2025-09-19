using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;

public record ExcluirVeiculoCommand(Guid Id) : IRequest<Result<ExcluirVeiculoResult>>;

public record ExcluirVeiculoResult();