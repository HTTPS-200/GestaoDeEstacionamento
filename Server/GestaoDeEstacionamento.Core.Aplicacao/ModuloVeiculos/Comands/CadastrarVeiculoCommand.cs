using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;

public record CadastrarVeiculoCommand(
    string Placa,
    string Modelo,
    string Cor,
    string CPFHospede,
    string? Observacoes = null
) : IRequest<Result<CadastrarVeiculoResult>>;

public record CadastrarVeiculoResult(Guid Id);