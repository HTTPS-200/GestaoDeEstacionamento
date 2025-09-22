using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloTicket;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace GestaoDeEstacionamento.Testes.Integracao.ModuloTicket
{
    [TestClass]
    [TestCategory("Integração - Ticket")]
    public class RepositorioTicketEmOrmTests
    {
        private AppDbContext contexto;
        private RepositorioTicketEmOrm repositorio;

        [TestInitialize]
        public void Inicializar()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            contexto = new AppDbContext(options);
            repositorio = new RepositorioTicketEmOrm(contexto);
        }

        [TestMethod]
        public async Task Deve_Cadastrar_Ticket()
        {
            var ticket = new Ticket("000001", Guid.NewGuid(), 1);

            await repositorio.CadastrarAsync(ticket);
            await contexto.SaveChangesAsync();

            var ticketDb = await contexto.Set<Ticket>().FindAsync(ticket.Id);
            Assert.IsNotNull(ticketDb);
            Assert.AreEqual(ticket.NumeroTicket, ticketDb.NumeroTicket);
        }

        [TestMethod]
        public async Task Deve_Obter_Ticket_Por_Id()
        {
            var ticket = new Ticket("000002", Guid.NewGuid(), 2);
            await repositorio.CadastrarAsync(ticket);
            await contexto.SaveChangesAsync();

            var ticketDb = await repositorio.SelecionarRegistroPorIdAsync(ticket.Id);
            Assert.IsNotNull(ticketDb);
            Assert.AreEqual(ticket.Id, ticketDb.Id);
        }

        [TestMethod]
        public async Task Deve_Obter_Tickets_Por_Veiculo()
        {
            var veiculoId = Guid.NewGuid();
            var ticket1 = new Ticket("000003", veiculoId, 3);
            var ticket2 = new Ticket("000004", veiculoId, 4);
            var ticket3 = new Ticket("000005", Guid.NewGuid(), 5);

            await repositorio.CadastrarAsync(ticket1);
            await repositorio.CadastrarAsync(ticket2);
            await repositorio.CadastrarAsync(ticket3);
            await contexto.SaveChangesAsync();

            var ticketsVeiculo = await repositorio.ObterPorVeiculoId(veiculoId);
            Assert.AreEqual(2, ticketsVeiculo.Count);
            CollectionAssert.AreEquivalent(new[] { ticket1.Id, ticket2.Id }, ticketsVeiculo.Select(t => t.Id).ToArray());
        }

        [TestMethod]
        public async Task Deve_Obter_Ultimo_Numero_Sequencial()
        {
            var ticket1 = new Ticket("000006", Guid.NewGuid(), 6);
            var ticket2 = new Ticket("000007", Guid.NewGuid(), 7);

            await repositorio.CadastrarAsync(ticket1);
            await repositorio.CadastrarAsync(ticket2);
            await contexto.SaveChangesAsync();

            var ultimoSequencial = await repositorio.ObterUltimoNumeroSequencial();
            Assert.AreEqual(7, ultimoSequencial);
        }

        [TestMethod]
        public async Task Deve_Obter_Maior_Numero_Sequencial()
        {
            var ticket1 = new Ticket("000008", Guid.NewGuid(), 8);
            var ticket2 = new Ticket("000009", Guid.NewGuid(), 9);

            await repositorio.CadastrarAsync(ticket1);
            await repositorio.CadastrarAsync(ticket2);
            await contexto.SaveChangesAsync();

            var maiorSequencial = await repositorio.ObterMaiorNumeroSequencial();
            Assert.AreEqual(9, maiorSequencial);
        }
    }
}
