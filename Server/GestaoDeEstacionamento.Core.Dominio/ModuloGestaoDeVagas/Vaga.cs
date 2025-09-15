using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
public class Vaga
{
    public int Id { get; set; }
    public string Zona { get; set; } = string.Empty;
    public bool Ocupada { get; set; }
    public Veiculo? VeiculoEstacionado { get; set; }
}