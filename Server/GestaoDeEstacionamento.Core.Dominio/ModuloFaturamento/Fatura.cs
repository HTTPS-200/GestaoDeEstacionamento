using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
public class Fatura
{
    public int Id { get; set; }
    public Ticket Ticket { get; set; } = new();
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public int NumeroDiarias { get; set; }
    public decimal ValorTotal { get; set; }
}