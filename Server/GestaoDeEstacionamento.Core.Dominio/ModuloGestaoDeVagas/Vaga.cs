using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using System.Diagnostics.CodeAnalysis;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloVaga;

public class Vaga : EntidadeBase<Vaga>
{
    public string Identificador { get; set; }
    public string Zona { get; set; }
    public StatusVaga Status { get; set; }
    public Guid? VeiculoId { get; set; }
    public Veiculo? Veiculo { get; set; }

    [ExcludeFromCodeCoverage]
    public Vaga() { }

    public Vaga(string identificador, string zona)
    {
        Id = Guid.NewGuid();
        Identificador = identificador;
        Zona = zona;
        Status = StatusVaga.Livre;
    }

    public override void AtualizarRegistro(Vaga registroEditado)
    {
        Identificador = registroEditado.Identificador;
        Zona = registroEditado.Zona;
        Status = registroEditado.Status;
        VeiculoId = registroEditado.VeiculoId;
    }

    public bool Ocupar(Guid veiculoId)
    {
        if (Status == StatusVaga.Ocupada)
            return false;

        Status = StatusVaga.Ocupada;
        VeiculoId = veiculoId;
        return true;
    }

    public void Liberar()
    {
        Status = StatusVaga.Livre;
        VeiculoId = null;
        Veiculo = null;
    }
}

public enum StatusVaga
{
    Livre,
    Ocupada
}