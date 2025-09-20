using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using System.Diagnostics.CodeAnalysis;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloVaga;

public class Vaga : EntidadeBase<Vaga>
{
    public string Identificador { get; set; }
    public string Zona { get; set; }
    public bool Ocupada { get; set; }
    public Guid? VeiculoId { get; set; }
    public Guid UsuarioId { get; set; }

    [ExcludeFromCodeCoverage]
    public Vaga() { }

    public Vaga(string identificador, string zona, Guid usuarioId)
    {
        Id = Guid.NewGuid();
        Identificador = identificador;
        Zona = zona;
        Ocupada = false;
        VeiculoId = null;
        UsuarioId = usuarioId;
    }

    public void Ocupar(Guid veiculoId)
    {
        Ocupada = true;
        VeiculoId = veiculoId;
    }

    public void Liberar()
    {
        Ocupada = false;
        VeiculoId = null;
    }

    public override void AtualizarRegistro(Vaga registroEditado)
    {
        Identificador = registroEditado.Identificador;
        Zona = registroEditado.Zona;
        Ocupada = registroEditado.Ocupada;
        VeiculoId = registroEditado.VeiculoId;
    }
}