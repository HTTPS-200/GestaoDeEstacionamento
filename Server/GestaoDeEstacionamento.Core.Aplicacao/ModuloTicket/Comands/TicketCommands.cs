using FluentResults;
using MediatR;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;

public record CadastrarTicketCommand(
    string PlacaVeiculo 
) : IRequest<Result<CadastrarTicketResult>>;

public record CadastrarTicketResult(
    Guid Id,
    string NumeroTicket,
    string PlacaVeiculo
);

public record EditarTicketCommand(
    Guid Id,
    string PlacaVeiculo,  
    bool Ativo
) : IRequest<Result<EditarTicketResult>>;

public record EditarTicketResult(
    Guid Id,
    string PlacaVeiculo, 
    string NumeroTicket,
    bool Ativo
);

public record ExcluirTicketCommand(Guid Id) : IRequest<Result<ExcluirTicketResult>>;

public record ExcluirTicketResult();

public record SelecionarTicketPorIdQuery(Guid Id) : IRequest<Result<SelecionarTicketPorIdResult>>;

public record SelecionarTicketPorIdResult(
    Guid Id,
    string PlacaVeiculo, 
    string NumeroTicket,
    DateTime DataCriacao,
    bool Ativo
);

public record SelecionarTicketsQuery(int? Quantidade)
    : IRequest<Result<SelecionarTicketsResult>>;

public record SelecionarTicketsResult(ImmutableList<SelecionarTicketsItemDto> Tickets);

public record SelecionarTicketsItemDto(
    Guid Id,
    string PlacaVeiculo,  
    string NumeroTicket,
    DateTime DataCriacao,
    bool Ativo
);

public record ObterTicketPorNumeroQuery(string NumeroTicket)
    : IRequest<Result<SelecionarTicketPorIdResult>>;

public record ObterTicketsAtivosQuery()
    : IRequest<Result<SelecionarTicketsResult>>;

public record ObterTicketsPorVeiculoQuery(string PlacaVeiculo)  // ← Mudado para placa
    : IRequest<Result<SelecionarTicketsResult>>;