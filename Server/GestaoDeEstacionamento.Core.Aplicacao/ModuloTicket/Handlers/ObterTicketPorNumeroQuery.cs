using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Handlers;

public class ObterTicketPorNumeroQueryHandler(
    IRepositorioTicket repositorioTicket,
    IRepositorioVeiculo repositorioVeiculo,
    IMapper mapper,
    ILogger<ObterTicketPorNumeroQueryHandler> logger
) : IRequestHandler<ObterTicketPorNumeroQuery, Result<SelecionarTicketPorIdResult>>
{
    public async Task<Result<SelecionarTicketPorIdResult>> Handle(
        ObterTicketPorNumeroQuery query, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Buscando ticket por número: {NumeroTicket}", query.NumeroTicket);

            var registro = await repositorioTicket.ObterPorNumero(query.NumeroTicket);

            if (registro is null)
            {
                logger.LogWarning("Ticket não encontrado: {NumeroTicket}", query.NumeroTicket);
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.NumeroTicket));
            }

            logger.LogInformation("Ticket encontrado. Buscando veículo com ID: {VeiculoId}", registro.VeiculoId);

            // Use o método específico do repositório de veículo
            var veiculo = await repositorioVeiculo.ObterPorId(registro.VeiculoId);
            var placa = veiculo?.Placa ?? "Placa não encontrada";

            logger.LogInformation("Placa do veículo: {Placa}", placa);

            var result = new SelecionarTicketPorIdResult(
                registro.Id,
                placa,
                registro.NumeroTicket,
                registro.DataCriacao,
                registro.Ativo
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante a busca do ticket por número {@Query}.", query);
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}