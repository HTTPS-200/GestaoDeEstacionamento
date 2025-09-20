using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloRegistroCheckIn;

public record RealizarCheckOutRequest(
    string CPFHospede,
    string PlacaVeiculo
    );

public record RealizarCheckOutResponse(
    Guid CheckInId,
    Guid VeiculoId,
    Guid TickeId,
    string Placa,
    string NumeroTicket,
    DateTime DataHoraCheckIn,
    DateTime DataHoraCheckOut,
    int Diarias,
    decimal ValorTotal,
    bool Ativo,
    VagaInfoResponse? Vaga
    );

public record VagaInfoResponse(
    string Identificador,
    string Zona
    );
