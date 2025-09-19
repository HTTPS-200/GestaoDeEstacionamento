using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloSaidaLiberacao;
public class Saida : EntidadeBase<Saida>
{
    public Guid TicketId { get; set; } 
    public DateTime DataSaida { get; set; }
    public Ticket Ticket { get; set; } 

    public override void AtualizarRegistro(Saida registroEditado)
    {
        registroEditado.DataSaida = DataSaida;
        registroEditado.TicketId = TicketId;
    }
}