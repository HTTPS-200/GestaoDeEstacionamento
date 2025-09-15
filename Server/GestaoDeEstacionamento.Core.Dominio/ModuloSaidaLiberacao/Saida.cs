using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloSaidaLiberacao;
public class Saida : EntidadeBase<Saida>
{
    public DateTime DataSaida { get; set; }
    public Ticket TicketId { get; set; } = new();

    public override void AtualizarRegistro(Saida registroEditado)
    {
        registroEditado.DataSaida = DataSaida;
        registroEditado.TicketId = TicketId;
    }
}