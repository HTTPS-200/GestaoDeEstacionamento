using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
public class Vaga : EntidadeBase<Vaga>
{
    public string NumeroVaga { get; set; }
    public string Zona { get; set; } = string.Empty;
    public bool Ocupada { get; set; }
    public Veiculo? VeiculoEstacionado { get; set; }

    public override void AtualizarRegistro(Vaga registroEditado)
    {
        registroEditado.Zona = Zona;
        registroEditado.Ocupada = Ocupada;
        registroEditado.VeiculoEstacionado = VeiculoEstacionado;
    }
}