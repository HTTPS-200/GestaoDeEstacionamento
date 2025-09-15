using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloRelatorio;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloRelatorio;

public class RepositorioRelatorioFinanceiroEmOrm : RepositorioBaseEmOrm<RelatorioFinanceiro>, IRepositorioRelatorioFinanceiro
{
    private readonly AppDbContext context;

    public RepositorioRelatorioFinanceiroEmOrm(AppDbContext contexto)
        : base(contexto)
    {
        context = contexto;
    }

    public async Task<RelatorioFinanceiro> GerarRelatorioAsync(DateTime dataInicio, DateTime dataFim)
    {
        var faturas = await context.Set<Fatura>()
            .Where(f => f.DataEntrada >= dataInicio && f.DataSaida <= dataFim)
            .ToListAsync();

        var relatorio = new RelatorioFinanceiro
        {
            DataInicio = dataInicio,
            DataFim = dataFim,
            Faturas = faturas
        };

        return relatorio;
    }
}
