using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
public interface IRepositorioTicket : IRepositorio<Ticket>
{
    Ticket? ObterPorNumero(string numeroTicket);
    List<Ticket> ObterTicketsAtivos();
    TicketSequencial ObterUltimoSequencial();
    void AtualizarSequencial(TicketSequencial sequencial);
}