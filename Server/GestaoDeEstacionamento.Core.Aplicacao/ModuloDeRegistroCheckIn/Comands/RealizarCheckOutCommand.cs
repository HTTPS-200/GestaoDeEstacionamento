using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckOut.Commands;

public record RealizarCheckOutCommand(
    string CPFHospede,
    string PlacaVeiculo
) : IRequest<Result<RealizarCheckOutResult>>;

public record RealizarCheckOutResult(
    Guid CheckInId,
    Guid VeiculoId,
    Guid TicketId,
    string Placa,
    string NumeroTicket,
    VagaInfoResult? Vaga,
    DateTime DataHoraCheckIn,
    DateTime DataHoraCheckOut,
    int Diarias,
    decimal ValorTotal,
    bool Ativo
);

public record VagaInfoResult(
    string Identificador,
    string Zona
);