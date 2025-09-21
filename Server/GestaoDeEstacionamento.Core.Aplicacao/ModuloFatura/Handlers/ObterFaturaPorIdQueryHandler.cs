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
    public class ObterFaturaPorIdQueryHandler(
     IRepositorioFatura repositorioFatura,
     IMapper mapper
 ) : IRequestHandler<ObterFaturaPorIdQuery, Result<ObterFaturaPorIdResult>>
    {
        public async Task<Result<ObterFaturaPorIdResult>> Handle(
            ObterFaturaPorIdQuery query,
            CancellationToken cancellationToken)
        {
            try
            {
                var fatura = await repositorioFatura.SelecionarRegistroPorIdAsync(query.Id);

                if (fatura == null)
                    return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(
                        $"Fatura com ID {query.Id} não encontrada"));

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
