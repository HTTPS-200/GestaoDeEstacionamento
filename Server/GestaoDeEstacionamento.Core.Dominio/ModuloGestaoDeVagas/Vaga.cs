using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloRecepcaoChekInVeiculo;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
public class Vaga : EntidadeBase<Vaga>
{
    public int Id { get; set; }
    public string Zona { get; set; }
    public bool Ocupada => VeiculoEstacionado != null;
    public Veiculo? VeiculoEstacionado { get; set; }

    public Vaga(int id, string zona)
    {
        Id = id;
        Zona = zona;
    }

    public override void AtualizarRegistro(Vaga registroEditado)
    {
        var vaga = registroEditado as Vaga;
        if (vaga == null) return;

        Zona = vaga.Zona;
        VeiculoEstacionado = vaga.VeiculoEstacionado;
    }

    public void EstacionarVeiculo(Veiculo veiculo)
    {
        if (Ocupada)
            throw new InvalidOperationException("Vaga já está ocupada.");

        VeiculoEstacionado = veiculo;
    }

    public void LiberarVaga()
    {
        VeiculoEstacionado?.RegistrarSaida();
        VeiculoEstacionado = null;
    }
}
