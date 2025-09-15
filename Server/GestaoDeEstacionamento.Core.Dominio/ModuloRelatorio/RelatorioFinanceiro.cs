using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloRelatorio
{
    public class RelatorioFinanceiro : EntidadeBase<RelatorioFinanceiro>
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public List<Fatura> Faturas { get; set; } = new List<Fatura>();
        public decimal ValorTotalConsolidado
        {
            get
            {
                decimal total = 0;
                foreach (var fatura in Faturas)
                {
                    total += fatura.ValorTotal;
                }
                return total;
            }
        }

        public override void AtualizarRegistro(RelatorioFinanceiro registroEditado)
        {
            registroEditado.DataInicio = DataInicio;
            registroEditado.DataFim = DataFim;
            registroEditado.Faturas = Faturas;
        }
    }
}