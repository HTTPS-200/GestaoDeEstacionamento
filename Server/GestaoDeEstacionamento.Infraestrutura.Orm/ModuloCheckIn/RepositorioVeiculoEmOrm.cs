using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVeiculo;

public class RepositorioVeiculoEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<Veiculo>(contexto), IRepositorioVeiculo
{
    public async Task<List<Veiculo>> ObterPorPlaca(string placa)
    {
        return await registros
            .Where(v => v.Placa.Contains(placa))
            .ToListAsync();
    }

    public async Task<List<Veiculo>> ObterVeiculosEstacionados()
    {
        return await registros
            .Where(v => v.DataSaida == null)
            .ToListAsync();
    }

    public async Task<Veiculo?> ObterPorId(Guid id)
    {
        return await registros
            .FirstOrDefaultAsync(v => v.Id == id);
    }
}