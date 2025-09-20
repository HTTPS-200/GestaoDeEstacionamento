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

public class ObterTicketsPorVeiculoQueryHandler(
    IRepositorioTicket repositorioTicket,
    IRepositorioVeiculo repositorioVeiculo,
    IMapper mapper,
    ILogger<ObterTicketsPorVeiculoQueryHandler> logger
) : IRequestHandler<ObterTicketsPorVeiculoQuery, Result<SelecionarTicketsResult>>
{
    public async Task<Result<SelecionarTicketsResult>> Handle(
        ObterTicketsPorVeiculoQuery query, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Buscando tickets para o veículo com placa: {PlacaVeiculo}", query.PlacaVeiculo);

            var veiculos = await repositorioVeiculo.ObterPorPlaca(query.PlacaVeiculo);

            if (veiculos == null || !veiculos.Any())
            {
                logger.LogWarning("Nenhum veículo encontrado com a placa: {PlacaVeiculo}", query.PlacaVeiculo);
                return Result.Ok(new SelecionarTicketsResult([]));
            }

            var todosTickets = new List<SelecionarTicketsItemDto>();

            foreach (var veiculo in veiculos)
            {
                var tickets = await repositorioTicket.ObterPorVeiculoId(veiculo.Id);

                foreach (var ticket in tickets)
                {
                    var ticketDto = new SelecionarTicketsItemDto(
                        ticket.Id,
                        veiculo.Placa, 
                        ticket.NumeroTicket,
                        ticket.DataCriacao,
                        ticket.Ativo
                    );

                    todosTickets.Add(ticketDto);
                }
            }

            var result = new SelecionarTicketsResult(todosTickets.ToImmutableList());

            logger.LogInformation("Encontrados {Quantidade} tickets para a placa {PlacaVeiculo}",
                todosTickets.Count, query.PlacaVeiculo);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante a busca dos tickets para o veículo {PlacaVeiculo}",
                query.PlacaVeiculo);
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}