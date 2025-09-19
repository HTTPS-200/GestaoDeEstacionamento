using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloTicket;

public class RepositorioTicketEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<Ticket>(contexto), IRepositorioTicket
{
    public override async Task<List<Ticket>> SelecionarRegistrosAsync()
    {
        return await registros.ToListAsync();
    }

    public override async Task<Ticket?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return await registros.FirstOrDefaultAsync(x => x.Id == idRegistro);
    }

    public async Task<Ticket?> ObterPorNumero(string numeroTicket)
    {
        return await registros
            .FirstOrDefaultAsync(t => t.NumeroTicket == numeroTicket);
    }

    public async Task<List<Ticket>> ObterTicketsAtivos()
    {
        return await registros
            .Where(t => t.Ativo)
            .ToListAsync();
    }

    public async Task<List<Ticket>> ObterPorVeiculoId(Guid veiculoId)
    {
        return await registros
            .Where(t => t.VeiculoId == veiculoId)
            .ToListAsync();
    }

    public async Task<int> ObterUltimoNumeroSequencial()
    {
        var ultimoTicket = await registros
            .OrderByDescending(t => t.Sequencial)
            .FirstOrDefaultAsync();

        return ultimoTicket?.Sequencial ?? 0;
    }

    public async Task AtualizarUltimoNumeroSequencial(int ultimoNumero)
    {
        await Task.CompletedTask;
    }
}