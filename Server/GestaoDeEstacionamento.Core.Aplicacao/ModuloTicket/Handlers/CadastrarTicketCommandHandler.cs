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
        // 1. Validação do comando
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);
        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        try
        {
            // 2. Busca veículo pela placa
            var veiculo = (await repositorioVeiculo.ObterPorPlaca(command.PlacaVeiculo)).FirstOrDefault();
            if (veiculo == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(
                    $"Veículo com placa {command.PlacaVeiculo} não encontrado"));

            // 3. Verifica tickets ativos
            var ticketsAtivos = (await repositorioTicket.ObterPorVeiculoId(veiculo.Id)).Where(t => t.Ativo).ToList();
            if (ticketsAtivos.Any())
                return Result.Fail(ResultadosErro.RegistroDuplicadoErro(
                    $"Já existe um ticket ativo para o veículo com placa {command.PlacaVeiculo}"));

            // 4. Calcula próximo número sequencial
            var maiorSequencial = await repositorioTicket.ObterMaiorNumeroSequencial();
            var proximoNumero = maiorSequencial + 1;
            var numeroTicket = proximoNumero.ToString("D6");

            // 5. Cria ticket
            var ticket = new Ticket(numeroTicket, veiculo.Id, proximoNumero);
            ticket.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();

            await repositorioTicket.CadastrarAsync(ticket);
            await unitOfWork.CommitAsync();

            // 6. Invalida cache
            await InvalidarCaches(ticket.UsuarioId);

            // 7. Retorna resultado
            return Result.Ok(new CadastrarTicketResult(ticket.Id, numeroTicket, veiculo.Placa));
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(ex, "Erro ao cadastrar ticket {@Registro}", command);
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
            await cache.RemoveAsync(cacheKey);
    }
}
