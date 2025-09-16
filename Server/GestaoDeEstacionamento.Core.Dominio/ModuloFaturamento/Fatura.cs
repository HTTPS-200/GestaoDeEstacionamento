using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
public class Fatura : EntidadeBase<Fatura>
{
    public Guid TicketId { get; set; }
    public Guid VeiculoId { get; set; }

    public Ticket Ticket { get; set; }
    public Veiculo Veiculo { get; set; }
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public int NumeroDiarias { get; set; }
    public decimal ValorDiaria { get; set; }
    public decimal ValorTotal { get; set; }
    public bool Pago { get; set; }

    public override void AtualizarRegistro(Fatura registroEditado)
    {
        registroEditado.TicketId = TicketId;
        registroEditado.VeiculoId = VeiculoId;
        registroEditado.DataEntrada = DataEntrada;
        registroEditado.DataSaida = DataSaida;
        registroEditado.NumeroDiarias = NumeroDiarias;
        registroEditado.ValorTotal = ValorTotal;
    }
}