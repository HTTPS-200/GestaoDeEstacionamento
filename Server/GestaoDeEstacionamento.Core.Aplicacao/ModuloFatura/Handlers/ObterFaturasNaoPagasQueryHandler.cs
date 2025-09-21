using AutoMapper;
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
    public class ObterFaturasNaoPagasQueryHandler(
     IRepositorioFatura repositorioFatura,
     IMapper mapper
 ) : IRequestHandler<ObterFaturasNaoPagasQuery, Result<ObterFaturasResult>>
    {
        public async Task<Result<ObterFaturasResult>> Handle(
            ObterFaturasNaoPagasQuery query,
            CancellationToken cancellationToken)
        {
            try
            {
                var faturas = await repositorioFatura.ObterNaoPagas();

                var faturasResult = faturas.Select(f => new FaturaItemResult(
                    f.Id,
                    f.NumeroTicket,
                    f.PlacaVeiculo,
                    f.DataHoraSaida,
                    f.Diarias,
                    f.ValorTotal,
                    f.Pago,
                    f.DataPagamento
                )).ToImmutableList();

                var result = new ObterFaturasResult(faturasResult);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
            }
        }
    }
}
