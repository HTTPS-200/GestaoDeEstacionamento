using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;

public class OcuparVagaCommandHandler(
    IRepositorioVaga repositorioVaga,
    IRepositorioVeiculo repositorioVeiculo,
    IUnitOfWork unitOfWork,
    IDistributedCache cache,
    ILogger<OcuparVagaCommandHandler> logger
) : IRequestHandler<OcuparVagaCommand, Result<OcuparVagaResult>>
{
    public async Task<Result<OcuparVagaResult>> Handle(
        OcuparVagaCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Ocupando vaga {VagaId} com veículo de placa {PlacaVeiculo}",
                command.VagaId, command.PlacaVeiculo);

            var vaga = await repositorioVaga.SelecionarRegistroPorIdAsync(command.VagaId);
            if (vaga == null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.VagaId));

            if (vaga.Ocupada)
                return Result.Fail("Vaga já está ocupada");

            var veiculos = await repositorioVeiculo.ObterPorPlaca(command.PlacaVeiculo);
            var veiculo = veiculos.FirstOrDefault();
            if (veiculo == null)
                return Result.Fail($"Veículo com placa {command.PlacaVeiculo} não encontrado");

            // Ocupa a vaga
            vaga.Ocupar(veiculo.Id);
            await repositorioVaga.EditarAsync(vaga.Id, vaga);
            await unitOfWork.CommitAsync();

            await InvalidarCaches(vaga.UsuarioId);

            logger.LogInformation("Vaga {VagaId} ocupada com sucesso pelo veículo {PlacaVeiculo}",
                command.VagaId, command.PlacaVeiculo);

            var result = new OcuparVagaResult(
                vaga.Id,
                vaga.Identificador,
                vaga.Ocupada,
                veiculo.Id,
                veiculo.Placa
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(ex, "Erro ao ocupar vaga {VagaId}", command.VagaId);
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    private async Task InvalidarCaches(Guid usuarioId)
    {
        var cacheKeys = new[] { $"vagas:u={usuarioId}:q=all" };

        foreach (var cacheKey in cacheKeys)
            await cache.RemoveAsync(cacheKey);
    }
}
