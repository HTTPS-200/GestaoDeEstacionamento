using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloSaidaLiberacao;
public class Saida
{
    public int Id { get; set; }
    public DateTime DataSaida { get; set; }
    public Ticket Ticket { get; set; } = new();
}