using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;

public record CadastrarVeiculoCommand(
    string Placa,
    string Modelo,
    string Cor,
    string CpfHospede,
    string? Observacoes = null
) : IRequest<Result<CadastrarVeiculoResult>>;

public record CadastrarVeiculoResult(
    Guid Ticket,
    string Placa,
    string Modelo,
    string Cor,
    string CpfHospede,
    string? Observacoes,
    DateTime DataEntrada
);