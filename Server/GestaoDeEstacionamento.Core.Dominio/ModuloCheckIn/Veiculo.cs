using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
public class Veiculo : EntidadeBase<Veiculo>
{
    public int TicketId { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Cor { get; set; }
    public string CpfHospede { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataEntrada { get; set; }

    public override void AtualizarRegistro(Veiculo registroEditado)
    {
        registroEditado.TicketId = TicketId;
        registroEditado.Placa = Placa;
        registroEditado.Modelo = Modelo;
        registroEditado.Cor = Cor;
        registroEditado.CpfHospede = CpfHospede;
        registroEditado.Observacoes = Observacoes;
        registroEditado.DataEntrada = DataEntrada;
    }
}
