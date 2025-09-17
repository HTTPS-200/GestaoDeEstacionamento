using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn
{
    public interface IRepositorioMovimentacao
    {
        Task<List<Movimentacao>> SelecionarPorTicketAsync(Guid ticketId);
        Task<List<Movimentacao>> SelecionarPorVagaAsync(Guid vagaId);
        Task<List<Movimentacao>> SelecionarPorPeriodoAsync(DateTime inicio, DateTime fim);
    }
}
