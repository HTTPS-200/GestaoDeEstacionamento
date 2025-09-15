using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
public class Ticket : EntidadeBase<Ticket>
{
    public DateTime DataEntrada { get; set; }
    public Veiculo VeiculoId { get; set; } = new();

    public override void AtualizarRegistro(Ticket registroEditado)
    {
        registroEditado.DataEntrada = DateTime.Now;
        registroEditado.VeiculoId = VeiculoId;
    }
}
