using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Comands
{
    public record SelecionarCheckInPorIdQuery(Guid Id) : IRequest<Result<SelecionarCheckInPorIdResult>>;

    public record SelecionarCheckInPorIdResult(
        Guid Id,
        Guid VeiculoId,
        Guid TicketId,
        string NumeroTicket,
        DateTime DataHoraCheckIn,
        bool Ativo
    );

    public record SelecionarCheckInsQuery(int? Quantidade)
        : IRequest<Result<SelecionarCheckInsResult>>;

    public record SelecionarCheckInsResult(ImmutableList<SelecionarCheckInsDto> CheckIns);

    public record SelecionarCheckInsDto(
        Guid Id,
        Guid VeiculoId,
        Guid TicketId,
        string NumeroTicket,
        DateTime DataHoraCheckIn,
        bool Ativo
    );
}