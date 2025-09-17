using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloRecepcaoChekInVeiculo;
public class Veiculo : EntidadeBase<Veiculo>
{
    public Guid TicketId { get; set; } = Guid.NewGuid();
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Cor { get; set; }
    public string CpfHospede { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataEntrada { get; set; } = DateTime.Now;
    public DateTime? DataSaida {  get; set; }

    public Veiculo(Guid ticketId,
        string placa,
        string modelo,
        string cor, string cpfHospede,
        string? observacoes,
        DateTime dataEntrada,
        DateTime? dataSaida)
    {
        TicketId = ticketId;
        Placa = placa;
        Modelo = modelo;
        Cor = cor;
        CpfHospede = cpfHospede;
        Observacoes = observacoes;
        DataEntrada = dataEntrada;
        DataSaida = dataSaida;
    }

    public void RegistrarSaida()
    {
        DataSaida = DateTime.Now;
    }

    public void AdicionarObservacao(string observacao)
    {
        Observacoes = observacao;
    }

    public override void AtualizarRegistro(Veiculo entidadeAtualizada)
    {
        var veiculo = entidadeAtualizada as Veiculo;
        if (veiculo == null) return;

        Modelo = veiculo.Modelo;
        Cor = veiculo.Cor;
        Observacoes = veiculo.Observacoes;
        CpfHospede = veiculo.CpfHospede;
        DataSaida = veiculo.DataSaida;
    }
}

