using FluentResults;
using MediatR;
using System.Collections.Generic;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;

public record SelecionarVeiculosQuery(int? Quantidade) : IRequest<Result<SelecionarVeiculosResult>>;

public record SelecionarVeiculosResult(
    int Quantidade,
    IReadOnlyList<SelecionarVeiculosDto> Veiculos
);

public record SelecionarVeiculosDto(
    Guid TicketId,
    string Placa,
    string Modelo,
    string Cor,
    string CpfHospede,
    string? Observacoes,
    DateTime DataEntrada
);
