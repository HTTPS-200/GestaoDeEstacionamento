using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFatura;

public class RepositorioFaturaEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<Fatura>(contexto), IRepositorioFatura
{
    public override async Task<List<Fatura>> SelecionarRegistrosAsync()
    {
        return await registros.ToListAsync();
    }

    public override async Task<Fatura?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return await registros.FirstOrDefaultAsync(x => x.Id == idRegistro);
    }

    public async Task<Fatura?> ObterPorNumeroTicket(string numeroTicket)
    {
        return await registros
            .FirstOrDefaultAsync(f => f.NumeroTicket == numeroTicket);
    }

    public async Task<Fatura?> ObterPorPlaca(string placa)
    {
        return await registros
            .Where(f => f.PlacaVeiculo == placa)
            .OrderByDescending(f => f.DataHoraSaida)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Fatura>> ObterPorPeriodo(DateTime inicio, DateTime fim)
    {
        return await registros
            .Where(f => f.DataHoraSaida >= inicio && f.DataHoraSaida <= fim)
            .ToListAsync();
    }

    public async Task<List<Fatura>> ObterPorPeriodo(DateTime inicio, DateTime fim, Guid usuarioId)
    {
        return await registros
            .Where(f => f.UsuarioId == usuarioId &&
                       f.DataHoraSaida >= inicio &&
                       f.DataHoraSaida <= fim)
            .ToListAsync();
    }

    public async Task<List<Fatura>> ObterPorVeiculoId(Guid veiculoId)
    {
        return await registros
            .Where(f => f.VeiculoId == veiculoId)
            .ToListAsync();
    }

    public async Task<List<Fatura>> ObterNaoPagas()
    {
        return await registros
            .Where(f => !f.Pago)
            .ToListAsync();
    }
}