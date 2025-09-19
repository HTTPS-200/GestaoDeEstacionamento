using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVaga;

public class RepositorioVagaEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<Vaga>(contexto), IRepositorioVaga
{
    public override async Task<List<Vaga>> SelecionarRegistrosAsync()
    {
        return await registros.ToListAsync();
    }

    public override async Task<Vaga?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return await registros.FirstOrDefaultAsync(x => x.Id == idRegistro);
    }

    public async Task<List<Vaga>> ObterVagasLivres()
    {
        return await registros
            .Where(v => !v.Ocupada)
            .ToListAsync();
    }

    public async Task<List<Vaga>> ObterVagasOcupadas()
    {
        return await registros
            .Where(v => v.Ocupada)
            .ToListAsync();
    }

    public async Task<Vaga?> ObterPorIdentificador(string identificador)
    {
        return await registros
            .FirstOrDefaultAsync(v => v.Identificador == identificador);
    }

    public async Task<Vaga?> ObterPorVeiculoId(Guid veiculoId)
    {
        return await registros
            .FirstOrDefaultAsync(v => v.VeiculoId == veiculoId);
    }

    public async Task<bool> VerificarDisponibilidade(string identificador)
    {
        var vaga = await ObterPorIdentificador(identificador);
        return vaga != null && !vaga.Ocupada;
    }
}