using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloRelatorio
{
    public interface IRepositorioRelatorioFinanceiro
    {
        Task<RelatorioFinanceiro> GerarRelatorioAsync(DateTime dataInicio, DateTime dataFim);
    }
}
