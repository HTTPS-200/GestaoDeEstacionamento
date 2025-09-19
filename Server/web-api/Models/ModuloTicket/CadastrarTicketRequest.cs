using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloTicket;

public record CadastrarTicketRequest(
    string PlacaVeiculo 
);

public record CadastrarTicketResponse(
    Guid Id,
    string NumeroTicket, 
    string PlacaVeiculo 
);

public record EditarTicketRequest(
    string PlacaVeiculo,
    bool Ativo
);

public record EditarTicketResponse(
    Guid Id,
    string PlacaVeiculo,
    string NumeroTicket,
    bool Ativo
);

public record ExcluirTicketRequest(Guid Id);

public record ExcluirTicketResponse();

public record SelecionarTicketPorIdRequest(Guid Id);

public record SelecionarTicketPorIdResponse(
    Guid Id,
    string PlacaVeiculo, 
    string NumeroTicket,
    DateTime DataCriacao,
    bool Ativo
);

public record SelecionarTicketsRequest(int? Quantidade);

public record SelecionarTicketsResponse(
    int Quantidade,
    ImmutableList<SelecionarTicketsDto> Tickets
);

public record SelecionarTicketsDto(
    Guid Id,
    string PlacaVeiculo,  
    string NumeroTicket,
    DateTime DataCriacao,
    bool Ativo
);

public record ObterTicketPorNumeroRequest(string NumeroTicket);

public record ObterTicketsAtivosRequest();

public record ObterTicketsPorVeiculoRequest(string PlacaVeiculo); 