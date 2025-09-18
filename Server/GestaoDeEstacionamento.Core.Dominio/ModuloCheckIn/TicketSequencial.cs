using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using System.Diagnostics.CodeAnalysis;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloTicket;

public class TicketSequencial : EntidadeBase<TicketSequencial>
{
    public int UltimoNumero { get; set; }
    public DateTime DataAtualizacao { get; set; }

    [ExcludeFromCodeCoverage]
    public TicketSequencial() { }

    public TicketSequencial(int ultimoNumero)
    {
        Id = Guid.NewGuid();
        UltimoNumero = ultimoNumero;
        DataAtualizacao = DateTime.Now;
    }

    public override void AtualizarRegistro(TicketSequencial registroEditado)
    {
        UltimoNumero = registroEditado.UltimoNumero;
        DataAtualizacao = registroEditado.DataAtualizacao;
    }

    public int ProximoNumero()
    {
        UltimoNumero++;
        DataAtualizacao = DateTime.Now;
        return UltimoNumero;
    }
}