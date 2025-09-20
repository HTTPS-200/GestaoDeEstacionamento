namespace GestaoDeEstacionamento.WebApi.Models.ModuloCheckOut;

public record RealizarCheckOutRequest(
    string CPFHospede,
    string PlacaVeiculo
);

public record RealizarCheckOutResponse(
    Guid CheckInId,
    Guid VeiculoId,
    Guid TicketId,
    string Placa,
    string NumeroTicket,
    VagaInfoResponse? Vaga,
    DateTime DataHoraCheckIn,
    DateTime DataHoraCheckOut,
    int Diarias,
    decimal ValorTotal,
    bool Ativo
);

public record VagaInfoResponse(
    string Identificador,
    string Zona
);