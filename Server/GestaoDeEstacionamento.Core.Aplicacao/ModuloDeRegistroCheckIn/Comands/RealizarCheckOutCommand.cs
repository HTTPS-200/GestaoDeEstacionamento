using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Comands;
public record RealizarCheckOutCommand(
    string CPFHospede,
    string PlacaVeiculo
    ) : IRequest<Result<RealizarCheckOutResult>>;

public record RealizarCheckOutResult(
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
    VagaInfoResult? Vaga
    );

public record VagaInfoResult(
    string Identificador,
    string Zona
    );
