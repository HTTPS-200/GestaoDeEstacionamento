using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;

namespace GestaoDeEstacionamento.TestsUnitarios.Faturamento;

[TestClass]
[TestCategory("UnitTest - Faturamento")]
public sealed class FaturamentoTests
{
    [TestMethod]
    public void Deve_Calcular_Numero_De_Diarias_Corretamente()
    {
        // Arrange
        var fatura = new Fatura
        {
            DataEntrada = new DateTime(2025, 9, 14, 10, 0, 0),
            DataSaida = new DateTime(2025, 9, 16, 9, 0, 0),
            ValorDiaria = 120m
        };

        // Act
        fatura.NumeroDiarias = (fatura.DataSaida.Date - fatura.DataEntrada.Date).Days;
        fatura.ValorTotal = fatura.NumeroDiarias * fatura.ValorDiaria;

        // Assert
        Assert.AreEqual(2, fatura.NumeroDiarias);
        Assert.AreEqual(240m, fatura.ValorTotal);
    }

    [TestMethod]
    public void Deve_Atualizar_Registro_Corretamente()
    {
        // Arrange
        var id = Guid.NewGuid();

        var original = new Fatura
        {
            TicketId = new Ticket {Id = id},
            DataEntrada = new DateTime(2025, 9, 14),
            DataSaida = new DateTime(2025, 9, 15),
            NumeroDiarias = 1,
            ValorTotal = 100m
        };

        var editado = new Fatura();

        // Act
        original.AtualizarRegistro(editado);

        // Assert
        Assert.AreEqual(original.TicketId.Id, editado.TicketId.Id);
        Assert.AreEqual(original.DataEntrada, editado.DataEntrada);
        Assert.AreEqual(original.DataSaida, editado.DataSaida);
        Assert.AreEqual(original.NumeroDiarias, editado.NumeroDiarias);
        Assert.AreEqual(original.ValorTotal, editado.ValorTotal);
    }

    [TestMethod]
    public void Deve_Marcar_Fatura_Como_Paga()
    {
        // Arrange
        var fatura = new Fatura { Pago = false };

        // Act
        fatura.Pago = true;

        // Assert
        Assert.IsTrue(fatura.Pago);
    }

    [TestMethod]
    public void Deve_Calcular_ValorTotal_Para_Uma_Diaria()
    {
        // Arrange
        var fatura = new Fatura
        {
            DataEntrada = new DateTime(2025, 9, 14, 8, 0, 0),
            DataSaida = new DateTime(2025, 9, 14, 22, 0, 0),
            ValorDiaria = 150m
        };

        // Act
        fatura.NumeroDiarias = 1;
        fatura.ValorTotal = fatura.NumeroDiarias * fatura.ValorDiaria;

        // Assert
        Assert.AreEqual(1, fatura.NumeroDiarias);
        Assert.AreEqual(150m, fatura.ValorTotal);
    }
}
