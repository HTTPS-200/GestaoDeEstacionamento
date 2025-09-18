using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVeiculo;

public class RepositorioVeiculoEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<Veiculo>(contexto), IRepositorioVeiculo
{
    public override async Task<List<Veiculo>> SelecionarRegistrosAsync()
    {
        return await registros.Include(x => x.Tickets).ToListAsync();
    }

    public override async Task<Veiculo?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return await registros.Include(x => x.Tickets).FirstOrDefaultAsync(x => x.Id == idRegistro);
    }

    public async Task<List<Veiculo>> ObterPorPlaca(string placa)
    {
        return await registros
            .Include(x => x.Tickets)
            .Where(v => v.Placa.Contains(placa))
            .ToListAsync();
    }

    public async Task<List<Veiculo>> ObterVeiculosEstacionados()
    {
        return await registros
            .Include(x => x.Tickets)
            .Where(v => v.DataSaida == null)
            .ToListAsync();
    }

    public async Task<Veiculo?> ObterPorTicket(string numeroTicket)
    {
        return await registros
            .Include(x => x.Tickets)
            .FirstOrDefaultAsync(v => v.Tickets.Any(t => t.NumeroTicket == numeroTicket));
    }
}