using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using System.Diagnostics.CodeAnalysis;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;

public class RegistroCheckIn : EntidadeBase<RegistroCheckIn>
{
    public Veiculo Veiculo { get; set; }
    public Guid VeiculoId { get; set; }
    public Ticket Ticket { get; set; }
    public Guid TicketId { get; set; }
    public DateTime DataHoraCheckIn { get; set; }
    public string NumeroTicket { get; set; }
    public bool Ativo { get; set; }

    [ExcludeFromCodeCoverage]
    public RegistroCheckIn() { }

    public RegistroCheckIn(Veiculo veiculo, Ticket ticket)
    {
        Id = Guid.NewGuid();
        Veiculo = veiculo;
        VeiculoId = veiculo.Id;
        Ticket = ticket;
        TicketId = ticket.Id;
        DataHoraCheckIn = DateTime.UtcNow;
        NumeroTicket = ticket.NumeroTicket;
        Ativo = true;
    }

    public override void AtualizarRegistro(RegistroCheckIn registroEditado)
    {
        //não editavel
       
    }

    public void EncerrarCheckIn()
    {
        Ativo = false;
        Ticket.Encerrar();
    }
}