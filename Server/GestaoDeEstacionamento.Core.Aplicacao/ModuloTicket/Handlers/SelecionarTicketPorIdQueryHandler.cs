using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Handlers;

public class SelecionarTicketPorIdQueryHandler(
    IRepositorioTicket repositorioTicket,
    IRepositorioVeiculo repositorioVeiculo,
    IMapper mapper,
    ILogger<SelecionarTicketPorIdQueryHandler> logger
) : IRequestHandler<SelecionarTicketPorIdQuery, Result<SelecionarTicketPorIdResult>>
{
    public async Task<Result<SelecionarTicketPorIdResult>> Handle(
     SelecionarTicketPorIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var registro = await repositorioTicket.SelecionarRegistroPorIdAsync(query.Id);

            if (registro is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

            // Busca a placa do veículo
            var veiculo = await repositorioVeiculo.SelecionarRegistroPorIdAsync(registro.VeiculoId);
            var placa = veiculo?.Placa ?? "Placa não encontrada";

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
            logger.LogError(ex, "Ocorreu um erro durante a seleção do ticket {@Registro}.", query);
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}