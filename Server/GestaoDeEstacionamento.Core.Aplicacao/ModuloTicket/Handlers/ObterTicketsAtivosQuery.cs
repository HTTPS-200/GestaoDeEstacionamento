using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Handlers;

public class ObterTicketsAtivosQueryHandler(
    IRepositorioTicket repositorioTicket,
    IRepositorioVeiculo repositorioVeiculo,
    IMapper mapper,
    ILogger<ObterTicketsAtivosQueryHandler> logger
) : IRequestHandler<ObterTicketsAtivosQuery, Result<SelecionarTicketsResult>>
{
    public async Task<Result<SelecionarTicketsResult>> Handle(
        ObterTicketsAtivosQuery query, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Buscando tickets ativos");

            var tickets = await repositorioTicket.ObterTicketsAtivos();

            if (tickets == null || !tickets.Any())
            {
                logger.LogWarning("Nenhum ticket ativo encontrado");
                return Result.Ok(new SelecionarTicketsResult([]));
            }

            var ticketsResult = new List<SelecionarTicketsItemDto>();

            foreach (var ticket in tickets)
            {

                var veiculo = await repositorioVeiculo.ObterPorId(ticket.VeiculoId);
                var placa = veiculo?.Placa ?? "Placa não encontrada";

                var ticketDto = new SelecionarTicketsItemDto(
                    ticket.Id,
                    placa,
                    ticket.NumeroTicket,
                    ticket.DataCriacao,
                    ticket.Ativo
                );

                ticketsResult.Add(ticketDto);
            }

            var result = new SelecionarTicketsResult(ticketsResult.ToImmutableList());

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante a busca dos tickets ativos");
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}