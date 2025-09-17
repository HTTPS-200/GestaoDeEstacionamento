using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloRecepcaoChekInVeiculo;
using System.Drawing;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloTicket;

public class Ticket : EntidadeBase<Ticket>
{
    public Guid VeiculoId { get; set; }
    public DateTime Entrada { get; set; }
    public DateTime? Saida { get; set; }
    public decimal? Valor { get; set; }
    public string Status { get; set; } // "aberto" ou "encerrado"

    public Ticket(Guid veiculoId)
    {
        VeiculoId = veiculoId;
        Entrada = DateTime.UtcNow;
        Status = "aberto";
    }

    public void Encerrar(DateTime saida, decimal valor)
    {
        if (Status == "encerrado")
            throw new InvalidOperationException("Ticket já encerrado.");

        Saida = saida;
        Valor = valor;
        Status = "encerrado";
    }

    public override void AtualizarRegistro(Ticket registro)
    {
        if (Status == "encerrado")
            throw new InvalidOperationException("Ticket já encerrado.");

        if (registro is Ticket atualizacao)
        {
            Saida = atualizacao.Saida ?? Saida;
            Valor = atualizacao.Valor ?? Valor;
            Status = string.IsNullOrWhiteSpace(atualizacao.Status) ? Status : atualizacao.Status;
        }
    }
}