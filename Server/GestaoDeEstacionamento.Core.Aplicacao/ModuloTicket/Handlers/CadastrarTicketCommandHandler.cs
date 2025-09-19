using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Handlers;

public class CadastrarTicketCommandHandler(
    IRepositorioTicket repositorioTicket,
    IRepositorioVeiculo repositorioVeiculo,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IDistributedCache cache,
    IValidator<CadastrarTicketCommand> validator,
    ILogger<CadastrarTicketCommandHandler> logger
) : IRequestHandler<CadastrarTicketCommand, Result<CadastrarTicketResult>>
{
    public async Task<Result<CadastrarTicketResult>> Handle(
        CadastrarTicketCommand command,
        CancellationToken cancellationToken)
    {
        // Validação do comando
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);
        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        try
        {
            // 1. Busca o veículo pela placa
            var veiculos = await repositorioVeiculo.ObterPorPlaca(command.PlacaVeiculo);
            var veiculo = veiculos.FirstOrDefault();
            if (veiculo == null)
            {
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(
                    $"Veículo com placa {command.PlacaVeiculo} não encontrado"));
            }

            // 2. Verifica se já existe ticket ativo para este veículo
            var ticketsDoVeiculo = await repositorioTicket.ObterPorVeiculoId(veiculo.Id);
            if (ticketsDoVeiculo.Any())
            {
                return Result.Fail(ResultadosErro.RegistroDuplicadoErro(
                    $"Já existe um ticket para o veículo com placa {command.PlacaVeiculo}"));
            }

            // 3. Gera número sequencial único
            var ultimoNumero = await repositorioTicket.ObterUltimoNumeroSequencial();
            var proximoNumero = ultimoNumero + 1;
            var numeroTicket = proximoNumero.ToString("D6");

            // 4. Cria e persiste o ticket
            var ticket = new Ticket(numeroTicket, veiculo.Id, proximoNumero);
            ticket.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();

            await repositorioTicket.CadastrarAsync(ticket);
            await unitOfWork.CommitAsync();

            // 5. Invalida cache
            await InvalidarCaches(tenantProvider.UsuarioId.GetValueOrDefault());

            // 6. Retorna resultado
            var result = new CadastrarTicketResult(
                ticket.Id,
                numeroTicket,
                veiculo.Placa
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(ex, "Ocorreu um erro durante o cadastro do ticket {@Registro}.", command);
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    private async Task InvalidarCaches(Guid usuarioId)
    {
        var cacheKeys = new[]
        {
            $"tickets:u={usuarioId}:q=all",
            $"veiculos:u={usuarioId}:q=all"
        };

        foreach (var cacheKey in cacheKeys)
        {
            await cache.RemoveAsync(cacheKey);
        }
    }
}
