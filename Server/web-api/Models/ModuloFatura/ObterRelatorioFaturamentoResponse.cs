using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloFatura;

public record ObterRelatorioFaturamentoResponse(
    DateTime PeriodoInicio,
    DateTime PeriodoFim,
    ImmutableList<FaturaDetalheResponse> Faturas,
    decimal ValorTotalPeriodo,
    int TotalDiarias,
    int TotalVeiculos
);

public record FaturaDetalheResponse(
    DateTime Data,
    string NumeroTicket,
    string Placa,
    int Diarias,
    decimal Valor
);