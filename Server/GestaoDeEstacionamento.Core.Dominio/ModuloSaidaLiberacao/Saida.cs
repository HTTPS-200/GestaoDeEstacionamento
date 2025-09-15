using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloSaidaLiberacao;
public class Saida : EntidadeBase<Saida>
{
    public DateTime DataSaida { get; set; }
    public Ticket Ticket { get; set; } = new();

    public override void AtualizarRegistro(Saida registroEditado)
    {
        registroEditado.DataSaida = DataSaida;
        registroEditado.Ticket = Ticket;
    }
}