using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloTicket
{
    [TestClass]
    [TestCategory("Repositorio - Ticket")]
    public class TicketRepositoryTests
    {
        private Mock<IRepositorioTicket>? repoMock;
        private List<Ticket>? tickets;

        [TestInitialize]
        public void Setup()
        {
            repoMock = new Mock<IRepositorioTicket>();
            tickets = new List<Ticket>
            {
                new Ticket("000001", Guid.NewGuid(), 1),
                new Ticket("000002", Guid.NewGuid(), 2),
                new Ticket("000003", Guid.NewGuid(), 3) { Ativo = false }
            };
        }

        [TestMethod]
        public async Task ObterTicketsAtivos_RetornaApenasAtivos()
        {
            repoMock!.Setup(r => r.ObterTicketsAtivos()).ReturnsAsync(tickets!.Where(t => t.Ativo).ToList());
            var resultado = await repoMock.Object.ObterTicketsAtivos();
            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod]
        public async Task ObterPorVeiculoId_RetornaTicketsCorretos()
        {
            var veiculoId = tickets![0].VeiculoId;
            repoMock!.Setup(r => r.ObterPorVeiculoId(veiculoId))
                     .ReturnsAsync(tickets.Where(t => t.VeiculoId == veiculoId).ToList());

            var resultado = await repoMock.Object.ObterPorVeiculoId(veiculoId);
            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual(veiculoId, resultado[0].VeiculoId);
        }

        [TestMethod]
        public async Task ObterMaiorNumeroSequencial_RetornaCorreto()
        {
            repoMock!.Setup(r => r.ObterMaiorNumeroSequencial()).ReturnsAsync(tickets!.Max(t => t.Sequencial));
            var resultado = await repoMock.Object.ObterMaiorNumeroSequencial();
            Assert.AreEqual(3, resultado);
        }
    }
}
