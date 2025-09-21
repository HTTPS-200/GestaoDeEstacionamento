namespace GestaoDeEstacionamento.WebApi.Models.ModuloFatura;

public record CriarFaturaResponse(
    Guid Id,
    string NumeroTicket,
    string PlacaVeiculo,
    DateTime DataHoraSaida,
    int Diarias,
    decimal ValorTotal,
    bool Pago
);
public record CriarFaturaRequest(
    Guid CheckInId,
    Guid VeiculoId,
    Guid TicketId,
    string NumeroTicket,
    string PlacaVeiculo,
    string ModeloVeiculo,
    string CorVeiculo,
    string CPFHospede,
    string? IdentificadorVaga,
    string? ZonaVaga,
    DateTime DataHoraEntrada,
    DateTime DataHoraSaida,
    int Diarias,
    decimal ValorDiaria,
    decimal ValorTotal
);

public record MarcarFaturaComoPagaRequest(
    DateTime DataPagamento
);