using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloRecepcaoChekInVeiculo;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
public class Fatura : EntidadeBase<Fatura>
{
    public Guid TicketId { get; set; }
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public int Diarias { get; set; }
    public decimal ValorDiaria { get; set; }
    public decimal ValorTotal => Diarias * ValorDiaria;

    public Fatura(){ }

    public Fatura(Veiculo veiculo)
    {
        if (veiculo.DataSaida == null)
            throw new InvalidOperationException("Veículo ainda não saiu.");

        TicketId = veiculo.TicketId;
        DataEntrada = veiculo.DataEntrada;
        DataSaida = veiculo.DataSaida.Value;
        Diarias = CalcularDiarias(DataEntrada, DataSaida);
    }

    private int CalcularDiarias(DateTime entrada, DateTime saida)
    {
        var dias = (saida.Date - entrada.Date).Days + 1;
        return dias < 1 ? 1 : dias;
    }

    public override void AtualizarRegistro(Fatura entidadeAtualizada)
    {
        var fatura = entidadeAtualizada as Fatura;
        if (fatura == null) return;

        DataEntrada = fatura.DataEntrada;
        DataSaida = fatura.DataSaida;
        Diarias = fatura.Diarias;
        ValorDiaria = fatura.ValorDiaria;
    }
}
