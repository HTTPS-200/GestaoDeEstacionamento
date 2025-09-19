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
    public TicketSequencialInfo SequencialInfo { get; set; }

    [ExcludeFromCodeCoverage]
    public Ticket() { }

    public Ticket(string numeroTicket, Guid veiculoId, int ultimoNumeroSequencial)
    {
        Id = Guid.NewGuid();
        NumeroTicket = numeroTicket;
        VeiculoId = veiculoId;
        DataCriacao = DateTime.UtcNow;
        Ativo = true;
        SequencialInfo = new TicketSequencialInfo(ultimoNumeroSequencial);
    }

    public override void AtualizarRegistro(Ticket registroEditado)
    {
        NumeroTicket = registroEditado.NumeroTicket;
        VeiculoId = registroEditado.VeiculoId;
        Ativo = registroEditado.Ativo;
        SequencialInfo = registroEditado.SequencialInfo;
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