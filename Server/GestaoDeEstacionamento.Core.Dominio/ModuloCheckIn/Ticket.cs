
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using System.Diagnostics.CodeAnalysis;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloTicket;

public class Ticket : EntidadeBase<Ticket>
{
    public string NumeroTicket { get; set; }
    public Guid VeiculoId { get; set; }
    public Veiculo Veiculo { get; set; }
    public DateTime DataCriacao { get; set; }
    public bool Ativo { get; set; }

    [ExcludeFromCodeCoverage]
    public Ticket() { }

    public Ticket(string numeroTicket, Guid veiculoId)
    {
        Id = Guid.NewGuid();
        NumeroTicket = numeroTicket;
        VeiculoId = veiculoId;
        DataCriacao = DateTime.Now;
        Ativo = true;
    }

    public override void AtualizarRegistro(Ticket registroEditado)
    {
        NumeroTicket = registroEditado.NumeroTicket;
        VeiculoId = registroEditado.VeiculoId;
        Ativo = registroEditado.Ativo;
    }

    public void Encerrar()
    {
        Ativo = false;
    }
}