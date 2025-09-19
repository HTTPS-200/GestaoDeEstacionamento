using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloTicket;

public class RepositorioTicketEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<Ticket>(contexto), IRepositorioTicket
{
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
        // Busca o ticket mais recente e pega o último número sequencial
        var ultimoTicket = await registros
            .OrderByDescending(t => t.SequencialInfo.DataAtualizacao)
            .FirstOrDefaultAsync();

        return ultimoTicket?.SequencialInfo.UltimoNumero ?? 0;
    }

    public async Task<int> GerarProximoNumeroSequencial()
    {
        var ultimoNumero = await ObterUltimoNumeroSequencial();
        return ultimoNumero + 1;
    }
}