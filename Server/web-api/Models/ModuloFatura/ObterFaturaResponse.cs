namespace GestaoDeEstacionamento.WebApi.Models.ModuloFatura;

public record ObterFaturaResponse(
    Guid Id,
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
    decimal ValorTotal,
    bool Pago,
    DateTime? DataPagamento
);