using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloCheckIn;

public class RepositorioRegistroCheckInEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<RegistroCheckIn>(contexto), IRepositorioRegistroCheckIn
{
    public override async Task<List<RegistroCheckIn>> SelecionarRegistrosAsync()
    {
        return await registros
            .Include(x => x.Veiculo)
            .Include(x => x.Ticket)
            .ToListAsync();
    }

    public override async Task<RegistroCheckIn?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return await registros
            .Include(x => x.Veiculo)
            .Include(x => x.Ticket)
            .FirstOrDefaultAsync(x => x.Id == idRegistro);
    }

    public async Task<RegistroCheckIn?> ObterPorNumeroTicket(string numeroTicket)
    {
        return await registros
            .Include(x => x.Veiculo)
            .Include(x => x.Ticket)
            .FirstOrDefaultAsync(x => x.NumeroTicket == numeroTicket);
    }

    public async Task<RegistroCheckIn?> ObterPorPlacaVeiculo(string placa)
    {
        return await registros
            .Include(x => x.Veiculo)
            .Include(x => x.Ticket)
            .FirstOrDefaultAsync(x => x.Veiculo.Placa == placa && x.Ativo);
    }

    public async Task<List<RegistroCheckIn>> ObterCheckInsAtivos()
    {
        return await registros
            .Include(x => x.Veiculo)
            .Include(x => x.Ticket)
            .Where(x => x.Ativo)
            .ToListAsync();
    }

    public async Task<List<RegistroCheckIn>> ObterCheckInsPorVeiculoId(Guid veiculoId)
    {
        return await registros
            .Include(x => x.Veiculo)
            .Include(x => x.Ticket)
            .Where(x => x.VeiculoId == veiculoId)
            .ToListAsync();
    }

    public new async Task<List<RegistroCheckIn>> ObterTodosAsync()
    {
        return await registros
            .Include(x => x.Veiculo)
            .Include(x => x.Ticket)
            .ToListAsync();
    }

    public new async Task<RegistroCheckIn?> ObterPorIdAsync(Guid id)
    {
        return await registros
            .Include(x => x.Veiculo)
            .Include(x => x.Ticket)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}