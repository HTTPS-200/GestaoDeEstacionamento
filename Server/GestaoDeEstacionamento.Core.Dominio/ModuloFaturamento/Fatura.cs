using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
public class Fatura : EntidadeBase<Fatura>
{
    public Guid TicketId { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataFaturamento { get; set; }
    public bool Pago { get; set; }

    public Fatura(Guid ticketId, decimal valorTotal)
    {
        TicketId = ticketId;
        ValorTotal = valorTotal;
        DataFaturamento = DateTime.UtcNow;
        Pago = false;
    }

    public override void AtualizarRegistro(Fatura registro)
    {
        if (registro is Fatura atualizacao)
        {
            ValorTotal = atualizacao.ValorTotal != 0 ? atualizacao.ValorTotal : ValorTotal;
            if (atualizacao.Pago && !Pago)
                RegistrarPagamento();
        }
    }

    public void RegistrarPagamento()
    {
        if (Pago)
            throw new InvalidOperationException("Fatura já paga.");
        Pago = true;
    }
}
