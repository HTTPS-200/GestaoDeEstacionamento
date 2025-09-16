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
    public void Deve_Associar_Ticket_Corretamente()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var ticket = new Ticket { Id = ticketId };
        var saida = new Saida();

        // Act
        saida.TicketId = ticket;

        // Assert
        Assert.AreEqual(ticketId, saida.TicketId.Id);
    }

    [TestMethod]
    public void Deve_Atualizar_Registro_Corretamente()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var original = new Saida
        {
            DataSaida = new DateTime(2025, 9, 16, 15, 0, 0),
            TicketId = new Ticket { Id = ticketId }
        };

        var editado = new Saida();

        // Act
        original.AtualizarRegistro(editado);

        // Assert
        Assert.AreEqual(original.DataSaida, editado.DataSaida);
        Assert.AreEqual(original.TicketId.Id, editado.TicketId.Id);
    }
}
