using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using MediatR;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Handlers;

public class ObterRelatorioFaturamentoQueryHandler(
    IRepositorioFatura repositorioFatura,
    ITenantProvider tenantProvider 
) : IRequestHandler<ObterRelatorioFaturamentoQuery, Result<ObterRelatorioFaturamentoResult>>
{
    public async Task<Result<ObterRelatorioFaturamentoResult>> Handle(
        ObterRelatorioFaturamentoQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            // Obtenha o ID do usuário logado
            var usuarioId = tenantProvider.UsuarioId.GetValueOrDefault();

            // Passe o usuarioId para o método do repositório
            var faturas = await repositorioFatura.ObterPorPeriodo(
                query.Inicio,
                query.Fim,
                usuarioId
            );

            var faturasDetalhe = faturas.Select(f => new FaturaDetalheResult(
                f.DataHoraSaida.Date,
                f.NumeroTicket,
                f.PlacaVeiculo,
                f.Diarias,
                f.ValorTotal
            )).ToImmutableList();

            var valorTotal = faturas.Sum(f => f.ValorTotal);
            var totalDiarias = faturas.Sum(f => f.Diarias);
            var totalVeiculos = faturas.Select(f => f.VeiculoId).Distinct().Count();

            var result = new ObterRelatorioFaturamentoResult(
                query.Inicio,
                query.Fim,
                faturasDetalhe,
                valorTotal,
                totalDiarias,
                totalVeiculos
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}