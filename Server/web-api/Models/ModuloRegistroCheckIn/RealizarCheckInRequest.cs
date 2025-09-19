namespace GestaoDeEstacionamento.WebApi.Models.ModuloCheckIn;

public record RealizarCheckInRequest(
    string PlacaVeiculo,
    string CPFHospede
);

public record RealizarCheckInResponse(
    Guid CheckInId,
    Guid VeiculoId,
    Guid TicketId,
    string NumeroTicket,
    DateTime DataHoraCheckIn
);