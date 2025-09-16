using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
public class Ticket : EntidadeBase<Ticket>
{
    public Guid VeiculoId { get; set; }
    public int Numero { get; set; }
    public bool Status { get; set; }
    public DateTime DataEntrada { get; set; }
    public Veiculo Veiculo { get; set; } = new(); 

    public override void AtualizarRegistro(Ticket registroEditado)
    {
        registroEditado.DataEntrada = DateTime.Now;
        registroEditado.Veiculo = Veiculo;
    }
}

