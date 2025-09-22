using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckOut.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GestaoDeEstacionamento.TestsUnitarios.ModuloCheckOut
{
    [TestClass]
    public class CheckoutRepositoryTests
    {
        private Mock<IRepositorioRegistroCheckIn> repositorioCheckIn;
        private Mock<IRepositorioTicket> repositorioTicket;
        private Mock<IRepositorioVeiculo> repositorioVeiculo;

        [TestInitialize]
        public void Setup()
        {
            repositorioCheckIn = new Mock<IRepositorioRegistroCheckIn>();
            repositorioTicket = new Mock<IRepositorioTicket>();
            repositorioVeiculo = new Mock<IRepositorioVeiculo>();
        }

        [TestMethod]
        public async Task Deve_Retornar_CheckIn_Ativo_Por_VeiculoId()
        {
            var veiculoId = Guid.NewGuid();
            var veiculo = new Veiculo("ABC123", "Ford", "Ka", "Preto", "12345678901") { Id = veiculoId };
            var ticket = new Ticket(numeroTicket: "ABC123-0001", veiculoId: veiculo.Id, sequencial: 1);
            var checkInAtivo = new RegistroCheckIn(veiculo, ticket);

            repositorioCheckIn.Setup(x => x.ObterCheckInsPorVeiculoId(veiculoId))
                .ReturnsAsync(new List<RegistroCheckIn> { checkInAtivo });

            var resultado = await repositorioCheckIn.Object.ObterCheckInsPorVeiculoId(veiculoId);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count);
            Assert.IsTrue(resultado.First().Ativo);
        }

        [TestMethod]
        public async Task Deve_Retornar_CheckIn_Nao_Encontrado_Para_VeiculoId_Invalido()
        {
            var veiculoId = Guid.NewGuid();
            repositorioCheckIn.Setup(x => x.ObterCheckInsPorVeiculoId(veiculoId))
                .ReturnsAsync(new List<RegistroCheckIn>());

            var resultado = await repositorioCheckIn.Object.ObterCheckInsPorVeiculoId(veiculoId);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
        }
    }
}
