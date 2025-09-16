using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloSaidaLiberacao;

namespace GestaoDeEstacionamento.TestsUnitarios.ModuloSaidaLiberacao;

[TestClass]
[TestCategory("unitTests - Saida")]
public sealed class SaidaTests
{
    [TestMethod]
    public void Deve_Registrar_Data_De_Saida_Corretamente()
    {
        // Arrange
        var dataEsperada = new DateTime(2025, 9, 16, 14, 30, 0);
        var saida = new Saida();

        // Act
        saida.DataSaida = dataEsperada;

        // Assert
        Assert.AreEqual(dataEsperada, saida.DataSaida);
    }

    [TestMethod]
    public void Deve_Associar_TicketId_Corretamente()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var saida = new Saida();

        // Act
        saida.TicketId = ticketId;

        // Assert
        Assert.AreEqual(ticketId, saida.TicketId);
    }

    [TestMethod]
    public void Deve_Atualizar_Registro_Corretamente()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var original = new Saida
        {
            DataSaida = new DateTime(2025, 9, 16, 15, 0, 0),
            TicketId = ticketId
        };

        var editado = new Saida();

        // Act
        original.AtualizarRegistro(editado);

        // Assert
        Assert.AreEqual(original.DataSaida, editado.DataSaida);
        Assert.AreEqual(original.TicketId, editado.TicketId);
    }
}
