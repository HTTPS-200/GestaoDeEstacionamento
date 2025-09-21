using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using MediatR;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Handlers
{
    public class ObterRelatorioFaturamentoQueryHandler(
     IRepositorioFatura repositorioFatura
 ) : IRequestHandler<ObterRelatorioFaturamentoQuery, Result<ObterRelatorioFaturamentoResult>>
    {
        public async Task<Result<ObterRelatorioFaturamentoResult>> Handle(
            ObterRelatorioFaturamentoQuery query,
            CancellationToken cancellationToken)
        {
            try
            {
                var faturas = await repositorioFatura.ObterPorPeriodo(query.Inicio, query.Fim);

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
}
