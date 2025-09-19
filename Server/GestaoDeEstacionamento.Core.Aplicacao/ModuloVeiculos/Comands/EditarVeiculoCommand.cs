using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;

public record EditarVeiculoCommand(
    Guid Id,
    string Placa,
    string Modelo,
    string Cor,
    string CPFHospede,
    string? Observacoes = null
) : IRequest<Result<EditarVeiculoResult>>;

public record EditarVeiculoResult(
    string Placa,
    string Modelo,
    string Cor,
    string CPFHospede,
    string? Observacoes
);