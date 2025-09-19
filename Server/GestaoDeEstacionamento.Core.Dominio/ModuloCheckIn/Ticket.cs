using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloTicket;

public class Ticket : EntidadeBase<Ticket>
{
    public string NumeroTicket { get; set; }
    public Guid VeiculoId { get; set; }
    public DateTime DataCriacao { get; set; }
    public bool Ativo { get; set; }
    public Guid UsuarioId { get; set; }

    // Coluna persistida no banco
    public int Sequencial { get; set; }

    // Propriedade não mapeada para usar internamente
    [NotMapped]
    public TicketSequencialInfo SequencialInfo { get; private set; }

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
        SequencialInfo = new TicketSequencialInfo(sequencial);
    }

    public override void AtualizarRegistro(Ticket registroEditado)
    {
        NumeroTicket = registroEditado.NumeroTicket;
        VeiculoId = registroEditado.VeiculoId;
        Ativo = registroEditado.Ativo;
        SequencialInfo = registroEditado.SequencialInfo;
    }
    public void AtualizarSequencial(int novoSequencial)
    {
        Sequencial = novoSequencial;
        SequencialInfo = new TicketSequencialInfo(novoSequencial);
    }


    public void Encerrar()
    {
        Ativo = false;
    }
}
public class TicketSequencialInfo
{
    public int UltimoNumero { get; private set; }
    public DateTime DataAtualizacao { get; private set; }

    public TicketSequencialInfo(int ultimoNumero)
    {
        UltimoNumero = ultimoNumero;
        DataAtualizacao = DateTime.UtcNow;
    }

    public int ProximoNumero()
    {
        UltimoNumero++;
        DataAtualizacao = DateTime.UtcNow;
        return UltimoNumero;
    }
}