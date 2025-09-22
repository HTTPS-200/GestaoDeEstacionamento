using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloTicket
{
    [TestClass]
    [TestCategory("Domínio - Ticket")]
    public class TicketTests
    {
        [TestMethod]
        public void CriarTicket_Deve_ConfigurarPropriedadesCorretamente()
        {
            var veiculoId = Guid.NewGuid();
            var ticket = new Ticket("000001", veiculoId, 1);

            Assert.AreEqual("000001", ticket.NumeroTicket);
            Assert.AreEqual(veiculoId, ticket.VeiculoId);
            Assert.IsTrue(ticket.Ativo);
            Assert.AreEqual(1, ticket.Sequencial);
            Assert.AreNotEqual(Guid.Empty, ticket.Id);
            Assert.IsTrue(ticket.DataCriacao <= DateTime.UtcNow);
        }

        [TestMethod]
        public void AtualizarRegistro_Deve_AtualizarPropriedades()
        {
            var ticketOriginal = new Ticket("000001", Guid.NewGuid(), 1);
            var ticketEditado = new Ticket("000002", Guid.NewGuid(), 2) { Ativo = false };

            ticketOriginal.AtualizarRegistro(ticketEditado);

            Assert.AreEqual("000002", ticketOriginal.NumeroTicket);
            Assert.AreEqual(ticketEditado.VeiculoId, ticketOriginal.VeiculoId);
            Assert.AreEqual(2, ticketOriginal.Sequencial);
            Assert.IsFalse(ticketOriginal.Ativo);
        }

        [TestMethod]
        public void Encerrar_Deve_DesativarTicket()
        {
            var ticket = new Ticket("000001", Guid.NewGuid(), 1);
            ticket.Encerrar();

            Assert.IsFalse(ticket.Ativo);
        }
    }
}