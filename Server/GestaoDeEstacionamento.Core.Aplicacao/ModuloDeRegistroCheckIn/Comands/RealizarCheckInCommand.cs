using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;

public record RealizarCheckInCommand(
    string Placa,
    string CPFHospede
) : IRequest<Result<RealizarCheckInResult>>;

public record RealizarCheckInResult(
    Guid CheckInId,
    Guid VeiculoId,
    Guid TicketId,
    string NumeroTicket,
    DateTime DataHoraCheckIn
);