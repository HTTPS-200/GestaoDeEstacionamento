namespace GestaoDeEstacionamento.WebApi.Models.ModuloFatura;

public record MarcarFaturaComoPagaResponse(
    Guid Id,
    string NumeroTicket,
    DateTime DataPagamento,
    bool Pago
);