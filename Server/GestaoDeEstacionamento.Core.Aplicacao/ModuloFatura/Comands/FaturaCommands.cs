using FluentResults;
using MediatR;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;

public record CriarFaturaCommand(
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
) : IRequest<Result<CriarFaturaResult>>;

public record CriarFaturaResult(
    Guid Id,
    string NumeroTicket,
    string PlacaVeiculo,
    DateTime DataHoraSaida,
    int Diarias,
    decimal ValorTotal,
    bool Pago
);

public record MarcarFaturaComoPagaCommand(Guid FaturaId) : IRequest<Result<MarcarFaturaComoPagaResult>>;

public record MarcarFaturaComoPagaResult(
    Guid Id,
    string NumeroTicket,
    DateTime DataPagamento,
    bool Pago
);

public record ObterFaturaPorIdQuery(Guid Id) : IRequest<Result<ObterFaturaPorIdResult>>;

public record ObterFaturaPorIdResult(
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

public record ObterFaturaPorTicketQuery(string NumeroTicket) : IRequest<Result<ObterFaturaPorIdResult>>;

public record ObterFaturaPorPlacaQuery(string Placa) : IRequest<Result<ObterFaturaPorIdResult>>;

public record ObterRelatorioFaturamentoQuery(DateTime Inicio, DateTime Fim)
    : IRequest<Result<ObterRelatorioFaturamentoResult>>;

public record ObterRelatorioFaturamentoResult(
    DateTime PeriodoInicio,
    DateTime PeriodoFim,
    ImmutableList<FaturaDetalheResult> Faturas,
    decimal ValorTotalPeriodo,
    int TotalDiarias,
    int TotalVeiculos
);

public record FaturaDetalheResult(
    DateTime Data,
    string NumeroTicket,
    string Placa,
    int Diarias,
    decimal Valor
);

public record ObterFaturasNaoPagasQuery() : IRequest<Result<ObterFaturasResult>>;

public record ObterFaturasResult(ImmutableList<FaturaItemResult> Faturas);

public record FaturaItemResult(
    Guid Id,
    string NumeroTicket,
    string PlacaVeiculo,
    DateTime DataHoraSaida,
    int Diarias,
    decimal ValorTotal,
    bool Pago,
    DateTime? DataPagamento
);