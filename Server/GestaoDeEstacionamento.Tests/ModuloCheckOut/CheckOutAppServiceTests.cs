using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckOut.Commands;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GestaoDeEstacionamento.TestsUnitarios.ModuloCheckOut
{
    [TestClass]
    public class CheckOutAppServiceTests
    {
        private Mock<IRepositorioRegistroCheckIn> repositorioCheckIn;
        private Mock<IRepositorioVeiculo> repositorioVeiculo;
        private Mock<IRepositorioTicket> repositorioTicket;
        private Mock<IRepositorioVaga> repositorioVaga;
        private Mock<IMediator> mediator;
        private Mock<IDistributedCache> cache;
        private Mock<IUnitOfWork> unitOfWork;

        [TestInitialize]
        public void Setup()
        {
            repositorioCheckIn = new Mock<IRepositorioRegistroCheckIn>();
            repositorioVeiculo = new Mock<IRepositorioVeiculo>();
            repositorioTicket = new Mock<IRepositorioTicket>();
            repositorioVaga = new Mock<IRepositorioVaga>();
            mediator = new Mock<IMediator>();
            cache = new Mock<IDistributedCache>();
            unitOfWork = new Mock<IUnitOfWork>();
        }

        [TestMethod]
        public async Task Deve_Realizar_CheckOut_Valido()
        {
            // Arrange
            var veiculo = new Veiculo("ABC123", "Ford", "Ka", "Preto", "12345678901");
            var ticket = new Ticket(numeroTicket: "ABC123-0001", veiculoId: veiculo.Id, sequencial: 1);
            var checkIn = new RegistroCheckIn(veiculo, ticket);
            var vaga = new Vaga
            {
                Id = Guid.NewGuid(),
                VeiculoId = veiculo.Id,
                Identificador = "V01",
                Zona = "A"
            };
            repositorioVeiculo.Setup(x => x.ObterPorPlaca(veiculo.Placa))
                .ReturnsAsync(new List<Veiculo> { veiculo });

            repositorioCheckIn.Setup(x => x.ObterCheckInsPorVeiculoId(veiculo.Id))
                .ReturnsAsync(new List<RegistroCheckIn> { checkIn });

            repositorioTicket.Setup(x => x.SelecionarRegistroPorIdAsync(checkIn.Ticket.Id))
                .ReturnsAsync(ticket);

            repositorioVaga.Setup(x => x.ObterPorVeiculoId(veiculo.Id))
                .ReturnsAsync(vaga);

            repositorioTicket.Setup(x => x.EditarAsync(ticket.Id, ticket))
                .ReturnsAsync(true);
            repositorioCheckIn.Setup(x => x.EditarAsync(checkIn.Id, checkIn))
                .ReturnsAsync(true);
            repositorioVeiculo.Setup(x => x.EditarAsync(veiculo.Id, veiculo))
                .ReturnsAsync(true);
            repositorioVaga.Setup(x => x.EditarAsync(vaga.Id, vaga))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.IsAny<CriarFaturaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok());

            unitOfWork.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            var handler = new RealizarCheckOutCommandHandler(
                repositorioCheckIn.Object,
                repositorioVeiculo.Object,
                repositorioTicket.Object,
                repositorioVaga.Object,
                mediator.Object,
                cache.Object,
                unitOfWork.Object
            );

            var command = new RealizarCheckOutCommand(veiculo.CPFHospede, veiculo.Placa);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(checkIn.Ativo);
            Assert.IsFalse(ticket.Ativo);
            Assert.AreEqual(veiculo.Id, result.Value.VeiculoId);
            Assert.AreEqual(ticket.NumeroTicket, result.Value.NumeroTicket);
            Assert.AreEqual(vaga.Identificador, result.Value.Vaga?.Identificador);
        }
    }
}
