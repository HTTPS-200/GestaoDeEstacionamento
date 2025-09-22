using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GestaoDeEstacionamento.TestsUnitarios.ModuloCheckIn
{
    [TestClass]
    public class CheckInRepositoryTests
    {
        private Mock<IRepositorioRegistroCheckIn> repositorio;

        [TestInitialize]
        public void Setup()
        {
            repositorio = new Mock<IRepositorioRegistroCheckIn>();
        }

        [TestMethod]
        public async Task Deve_Retornar_CheckIn_Por_NumeroTicket()
        {
            var veiculo = new GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo.Veiculo("ABC123", "Ford", "Ka", "Preto", "12345678901");
            var ticket = new GestaoDeEstacionamento.Core.Dominio.ModuloTicket.Ticket(
    numeroTicket: "ABC123-0001", 
    veiculoId: veiculo.Id,
    sequencial: 1
);
            var checkIn = new RegistroCheckIn(veiculo, ticket);

            repositorio.Setup(x => x.ObterPorNumeroTicket(ticket.NumeroTicket))
                .ReturnsAsync(checkIn);

            var resultado = await repositorio.Object.ObterPorNumeroTicket(ticket.NumeroTicket);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(ticket.NumeroTicket, resultado.NumeroTicket);
        }

        [TestMethod]
        public async Task Deve_Retornar_CheckIns_Ativos()
        {
            var veiculo = new GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo.Veiculo("ABC123", "Ford", "Ka", "Preto", "12345678901");
            var ticket = new GestaoDeEstacionamento.Core.Dominio.ModuloTicket.Ticket(
    numeroTicket: "ABC123-0001", 
    veiculoId: veiculo.Id,
    sequencial: 0
);
            var checkInAtivo = new RegistroCheckIn(veiculo, ticket);

            repositorio.Setup(x => x.ObterCheckInsAtivos())
                .ReturnsAsync(new List<RegistroCheckIn> { checkInAtivo });

            var resultado = await repositorio.Object.ObterCheckInsAtivos();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count);
            Assert.IsTrue(resultado.First().Ativo);
        }

        [TestMethod]
        public async Task Deve_Retornar_CheckIns_Por_VeiculoId()
        {
            var veiculoId = Guid.NewGuid();
            var veiculo = new GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo.Veiculo("ABC123", "Ford", "Ka", "Preto", "12345678901") { Id = veiculoId };
            var ticket = new GestaoDeEstacionamento.Core.Dominio.ModuloTicket.Ticket(
    numeroTicket: "ABC123-0001",
    veiculoId: veiculo.Id,
    sequencial: 1
);
            var checkIn = new RegistroCheckIn(veiculo, ticket);

            repositorio.Setup(x => x.ObterCheckInsPorVeiculoId(veiculoId))
                .ReturnsAsync(new List<RegistroCheckIn> { checkIn });

            var resultado = await repositorio.Object.ObterCheckInsPorVeiculoId(veiculoId);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual(veiculoId, resultado.First().VeiculoId);
        }
    }
}
