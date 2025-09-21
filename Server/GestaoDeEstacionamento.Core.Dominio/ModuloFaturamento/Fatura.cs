using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using System.Diagnostics.CodeAnalysis;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloFatura;

public class Fatura : EntidadeBase<Fatura>
{
    public Guid CheckInId { get; set; }
    public Guid VeiculoId { get; set; }
    public Guid TicketId { get; set; }
    public string NumeroTicket { get; set; }
    public string PlacaVeiculo { get; set; }
    public string ModeloVeiculo { get; set; }
    public string CorVeiculo { get; set; }
    public string CPFHospede { get; set; }
    public string? IdentificadorVaga { get; set; }
    public string? ZonaVaga { get; set; }
    public DateTime DataHoraEntrada { get; set; }
    public DateTime DataHoraSaida { get; set; }
    public int Diarias { get; set; }
    public decimal ValorDiaria { get; set; }
    public decimal ValorTotal { get; set; }
    public bool Pago { get; set; }
    public DateTime? DataPagamento { get; set; }
    public Guid UsuarioId { get; set; }

    [ExcludeFromCodeCoverage]
    public Fatura() { }

    public Fatura(
        Guid checkInId,
        Guid veiculoId,
        Guid ticketId,
        string numeroTicket,
        string placaVeiculo,
        string modeloVeiculo,
        string corVeiculo,
        string cpfHospede,
        string? identificadorVaga,
        string? zonaVaga,
        DateTime dataHoraEntrada,
        DateTime dataHoraSaida,
        int diarias,
        decimal valorDiaria,
        decimal valorTotal,
        Guid usuarioId)
    {
        Id = Guid.NewGuid();
        CheckInId = checkInId;
        VeiculoId = veiculoId;
        TicketId = ticketId;
        NumeroTicket = numeroTicket;
        PlacaVeiculo = placaVeiculo;
        ModeloVeiculo = modeloVeiculo;
        CorVeiculo = corVeiculo;
        CPFHospede = cpfHospede;
        IdentificadorVaga = identificadorVaga;
        ZonaVaga = zonaVaga;
        DataHoraEntrada = dataHoraEntrada;
        DataHoraSaida = dataHoraSaida;
        Diarias = diarias;
        ValorDiaria = valorDiaria;
        ValorTotal = valorTotal;
        Pago = false;
        DataPagamento = null;
        UsuarioId = usuarioId;
    }

    public override void AtualizarRegistro(Fatura registroEditado)
    {
        CheckInId = registroEditado.CheckInId;
        VeiculoId = registroEditado.VeiculoId;
        TicketId = registroEditado.TicketId;
        NumeroTicket = registroEditado.NumeroTicket;
        PlacaVeiculo = registroEditado.PlacaVeiculo;
        ModeloVeiculo = registroEditado.ModeloVeiculo;
        CorVeiculo = registroEditado.CorVeiculo;
        CPFHospede = registroEditado.CPFHospede;
        IdentificadorVaga = registroEditado.IdentificadorVaga;
        ZonaVaga = registroEditado.ZonaVaga;
        DataHoraEntrada = registroEditado.DataHoraEntrada;
        DataHoraSaida = registroEditado.DataHoraSaida;
        Diarias = registroEditado.Diarias;
        ValorDiaria = registroEditado.ValorDiaria;
        ValorTotal = registroEditado.ValorTotal;
        Pago = registroEditado.Pago;
        DataPagamento = registroEditado.DataPagamento;
        UsuarioId = registroEditado.UsuarioId;
    }

    public void MarcarComoPago()
    {
        Pago = true;
        DataPagamento = DateTime.UtcNow;
    }
}