using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Handlers
{
    public class ObterFaturaPorPlacaQueryHandler(
     IRepositorioFatura repositorioFatura,
     IMapper mapper
 ) : IRequestHandler<ObterFaturaPorPlacaQuery, Result<ObterFaturaPorIdResult>>
    {
        public async Task<Result<ObterFaturaPorIdResult>> Handle(
            ObterFaturaPorPlacaQuery query,
            CancellationToken cancellationToken)
        {
            try
            {
                var fatura = await repositorioFatura.ObterPorPlaca(query.Placa);

                if (fatura == null)
                    return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(
                        $"Fatura para veículo com placa {query.Placa} não encontrada"));

                var result = mapper.Map<ObterFaturaPorIdResult>(fatura);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
            }
        }
    }
}
