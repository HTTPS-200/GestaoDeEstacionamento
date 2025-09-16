using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloRelatorio;

namespace GestaoDeEstacionamento.TestsUnitarios.ModuloRelatorio;

[TestClass]
[TestCategory("UnitTests - Relatorio")]
public class RelatorioFinanceiroTests
{
    [TestMethod]
    public void Deve_Calcular_ValorTotalConsolidado_Corretamente()
    {
        // Arrange
        var relatorio = new RelatorioFinanceiro
        {
            Faturas = new List<Fatura>
            {
                new Fatura { ValorTotal = 100m },
                new Fatura { ValorTotal = 250m },
                new Fatura { ValorTotal = 150m }
            }
        };

        // Act
        var total = relatorio.ValorTotalConsolidado;

        // Assert
        Assert.AreEqual(500m, total);
    }

    [TestMethod]
    public void Deve_Atualizar_Registro_Corretamente()
    {
        // Arrange
        var faturas = new List<Fatura>
        {
            new Fatura { ValorTotal = 200m }
        };

        var original = new RelatorioFinanceiro
        {
            DataInicio = new DateTime(2025, 9, 1),
            DataFim = new DateTime(2025, 9, 15),
            Faturas = faturas
        };

        var editado = new RelatorioFinanceiro();

        // Act
        original.AtualizarRegistro(editado);

        // Assert
        Assert.AreEqual(original.DataInicio, editado.DataInicio);
        Assert.AreEqual(original.DataFim, editado.DataFim);
        Assert.AreEqual(original.Faturas.Count, editado.Faturas.Count);
        Assert.AreEqual(original.Faturas[0].ValorTotal, editado.Faturas[0].ValorTotal);
    }

    [TestMethod]
    public void Deve_Incluir_Faturas_No_Relatorio()
    {
        // Arrange
        var relatorio = new RelatorioFinanceiro();

        var novaFatura = new Fatura { ValorTotal = 300m };

        // Act
        relatorio.Faturas.Add(novaFatura);

        // Assert
        Assert.AreEqual(1, relatorio.Faturas.Count);
        Assert.AreEqual(300m, relatorio.ValorTotalConsolidado);
    }

    [TestMethod]
    public void Deve_Definir_Periodo_Corretamente()
    {
        // Arrange
        var inicio = new DateTime(2025, 9, 1);
        var fim = new DateTime(2025, 9, 30);

        var relatorio = new RelatorioFinanceiro
        {
            DataInicio = inicio,
            DataFim = fim
        };

        // Assert
        Assert.AreEqual(inicio, relatorio.DataInicio);
        Assert.AreEqual(fim, relatorio.DataFim);
    }
}
