using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloFatura;

public record ObterFaturasResponse(ImmutableList<FaturaItemResponse> Faturas);

public record FaturaItemResponse(
    Guid Id,
    string NumeroTicket,
    string PlacaVeiculo,
    DateTime DataHoraSaida,
    int Diarias,
    decimal ValorTotal,
    bool Pago,
    DateTime? DataPagamento
);