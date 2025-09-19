using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;

public record SelecionarVeiculoPorIdQuery(Guid Id) : IRequest<Result<SelecionarVeiculoPorIdResult>>;

public record SelecionarVeiculoPorIdResult(
    Guid Id,
    string Placa,
    string Modelo,
    string Cor,
    string CPFHospede,
    string? Observacoes,
    DateTime DataEntrada,
    DateTime? DataSaida
);