using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;

public record SelecionarVeiculoPorTicketQuery(Guid Ticket) : IRequest<Result<SelecionarVeiculoPorTicketResult>>;

public record SelecionarVeiculoPorTicketResult(
    Guid Ticket,
    string Placa,
    string Modelo,
    string Cor,
    string CpfHospede,
    string? Observacoes,
    DateTime DataEntrada
);
