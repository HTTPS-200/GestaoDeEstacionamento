using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Handlers
{
    public class MarcarFaturaComoPagaCommandHandler(
     IRepositorioFatura repositorioFatura,
     IUnitOfWork unitOfWork,
     ILogger<MarcarFaturaComoPagaCommandHandler> logger
 ) : IRequestHandler<MarcarFaturaComoPagaCommand, Result<MarcarFaturaComoPagaResult>>
    {
        public async Task<Result<MarcarFaturaComoPagaResult>> Handle(
            MarcarFaturaComoPagaCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var fatura = await repositorioFatura.SelecionarRegistroPorIdAsync(command.FaturaId);

                if (fatura == null)
                    return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(
                        $"Fatura com ID {command.FaturaId} não encontrada"));

                fatura.MarcarComoPago();

                var editado = await repositorioFatura.EditarAsync(fatura.Id, fatura);

                if (!editado)
                    return Result.Fail("Falha ao atualizar fatura");

                await unitOfWork.CommitAsync();

                var result = new MarcarFaturaComoPagaResult(
                    fatura.Id,
                    fatura.NumeroTicket,
                    fatura.DataPagamento.Value,
                    fatura.Pago
                );

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                logger.LogError(ex, "Erro ao marcar fatura como paga {@Registro}", command);
                return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
            }
        }
    }
}
