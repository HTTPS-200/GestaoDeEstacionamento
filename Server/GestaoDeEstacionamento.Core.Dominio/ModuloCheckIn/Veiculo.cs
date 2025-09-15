namespace GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
public class Veiculo
{
    public int TicketId { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Cor { get; set; }
    public string CpfHospede { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataEntrada { get; set; }
}
