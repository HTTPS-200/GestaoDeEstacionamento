namespace GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
public class Ticket
{
    public int Id { get; set; }
    public DateTime DataEntrada { get; set; }
    public Veiculo Veiculo { get; set; } = new();
}
