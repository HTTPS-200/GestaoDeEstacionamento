using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloCheckIn;

public record SelecionarCheckInPorIdRequest(Guid Id);

public record SelecionarCheckInPorIdResponse(
    Guid Id,
    Guid VeiculoId,
    Guid TicketId,
    string NumeroTicket,
    DateTime DataHoraCheckIn,
    bool Ativo
);

public record SelecionarCheckInsRequest(int? Quantidade);

public record SelecionarCheckInsResponse(
    int Quantidade,
    ImmutableList<SelecionarCheckInsDto> CheckIns
);

public record SelecionarCheckInsDto(
    Guid Id,
    Guid VeiculoId,
    Guid TicketId,
    string NumeroTicket,
    DateTime DataHoraCheckIn,
    bool Ativo
);