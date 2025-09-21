
using AutoMapper;
using FluentResults;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Handlers
{
    public class CriarFaturaCommandHandler(
    IRepositorioFatura repositorioFatura,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IDistributedCache cache,
    IValidator<CriarFaturaCommand> validator,
    ILogger<CriarFaturaCommandHandler> logger
) : IRequestHandler<CriarFaturaCommand, Result<CriarFaturaResult>>
    {
        public async Task<Result<CriarFaturaResult>> Handle(
            CriarFaturaCommand command,
            CancellationToken cancellationToken)
        {
            var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);
            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            try
            {
                var fatura = new Fatura(
                    command.CheckInId,
                    command.VeiculoId,
                    command.TicketId,
                    command.NumeroTicket,
                    command.PlacaVeiculo,
                    command.ModeloVeiculo,
                    command.CorVeiculo,
                    command.CPFHospede,
                    command.IdentificadorVaga,
                    command.ZonaVaga,
                    command.DataHoraEntrada,
                    command.DataHoraSaida,
                    command.Diarias,
                    command.ValorDiaria,
                    command.ValorTotal,
                    tenantProvider.UsuarioId.GetValueOrDefault()
                );

                await repositorioFatura.CadastrarAsync(fatura);
                await unitOfWork.CommitAsync();

                await InvalidarCaches(fatura.UsuarioId);

                var result = new CriarFaturaResult(
                    fatura.Id,
                    fatura.NumeroTicket,
                    fatura.PlacaVeiculo,
                    fatura.DataHoraSaida,
                    fatura.Diarias,
                    fatura.ValorTotal,
                    fatura.Pago
                );

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                logger.LogError(ex, "Erro ao criar fatura {@Registro}", command);
                return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
            }
        }

        private async Task InvalidarCaches(Guid usuarioId)
        {
            var cacheKeys = new[]
            {
            $"faturas:u={usuarioId}:q=all",
            $"faturas:u={usuarioId}:q=nao-pagas"
        };

            foreach (var cacheKey in cacheKeys)
                await cache.RemoveAsync(cacheKey);
        }
    }
}
