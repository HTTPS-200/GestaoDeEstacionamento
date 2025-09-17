// Command
using FluentResults;
using MediatR;

public record EditarVeiculoCommand(
    Guid Ticket,  // Guid
    string Placa,
    string Modelo,
    string Cor,
    string CpfHospede,
    DateTime DataEntrada,
    string? Observacoes = null
) : IRequest<Result<EditarVeiculoResult>>;

// Result
public record EditarVeiculoResult(
    Guid TicketId,   // Guid (coerente com API)
    string Placa,
    string Modelo,
    string Cor,
    string CpfHospede,
    string? Observacoes,
    DateTime DataEntrada
);
