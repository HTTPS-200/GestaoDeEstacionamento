using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using System.Diagnostics.CodeAnalysis;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloTicket;

public class Ticket : EntidadeBase<Ticket>
{
    public string NumeroTicket { get; set; }
    public Guid VeiculoId { get; set; }
    public DateTime DataCriacao { get; set; }
    public bool Ativo { get; set; }
    public Guid UsuarioId { get; set; }

    // Persistido no banco
    public int Sequencial { get; set; }

    [ExcludeFromCodeCoverage]
    public Ticket() { }

    public Ticket(string numeroTicket, Guid veiculoId, int sequencial)
    {
        Id = Guid.NewGuid();
        NumeroTicket = numeroTicket;
        VeiculoId = veiculoId;
        DataCriacao = DateTime.UtcNow;
        Ativo = true;
        Sequencial = sequencial;
    }

    public override void AtualizarRegistro(Ticket registroEditado)
    {
        NumeroTicket = registroEditado.NumeroTicket;
        VeiculoId = registroEditado.VeiculoId;
        Ativo = registroEditado.Ativo;
        Sequencial = registroEditado.Sequencial;
    }

    public void Encerrar()
    {
        Ativo = false;
        VeiculoId = Guid.Empty;
    }
}
