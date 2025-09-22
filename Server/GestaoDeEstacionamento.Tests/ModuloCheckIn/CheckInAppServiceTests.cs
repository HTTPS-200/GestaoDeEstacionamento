using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Handlers;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GestaoDeEstacionamento.TestsUnitarios.ModuloCheckIn
{
    [TestClass]
    public class CheckInAppServiceTests
    {
        private Mock<IRepositorioVeiculo> repositorioVeiculo;
        private Mock<IRepositorioTicket> repositorioTicket;
        private Mock<IRepositorioRegistroCheckIn> repositorioCheckIn;
        private Mock<ITenantProvider> tenantProvider;
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IDistributedCache> cache;
        private Mock<IValidator<RealizarCheckInCommand>> validator;
        private Mock<ILogger<RealizarCheckInCommandHandler>> logger;

        [TestInitialize]
        public void Setup()
        {
            repositorioVeiculo = new Mock<IRepositorioVeiculo>();
            repositorioTicket = new Mock<IRepositorioTicket>();
            repositorioCheckIn = new Mock<IRepositorioRegistroCheckIn>();
            tenantProvider = new Mock<ITenantProvider>();
            unitOfWork = new Mock<IUnitOfWork>();
            cache = new Mock<IDistributedCache>();
            validator = new Mock<IValidator<RealizarCheckInCommand>>();
            logger = new Mock<ILogger<RealizarCheckInCommandHandler>>();
        }

        [TestMethod]
        public async Task Deve_Realizar_CheckIn_Valido()
        {
            var veiculo = new Veiculo("ABC123", "Ford", "Ka", "Preto", "12345678901");
            var ticket = new Ticket(
                numeroTicket: "ABC123-0001", 
                veiculoId: veiculo.Id,
                sequencial: 1
            );
            tenantProvider.Setup(x => x.UsuarioId).Returns(Guid.NewGuid());

            repositorioVeiculo.Setup(x => x.ObterPorPlaca("ABC123"))
                .ReturnsAsync(new List<Veiculo> { veiculo });
            repositorioTicket.Setup(x => x.ObterPorVeiculoId(veiculo.Id))
                .ReturnsAsync(new List<Ticket> { ticket });
            repositorioCheckIn.Setup(x => x.ObterPorNumeroTicket(ticket.NumeroTicket))
                .ReturnsAsync((RegistroCheckIn?)null);
            validator.Setup(x => x.ValidateAsync(It.IsAny<RealizarCheckInCommand>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = new RealizarCheckInCommandHandler(
                repositorioVeiculo.Object,
                repositorioTicket.Object,
                repositorioCheckIn.Object,
                tenantProvider.Object,
                unitOfWork.Object,
                null!,
                cache.Object,
                validator.Object,
                logger.Object
            );

            var command = new RealizarCheckInCommand("ABC123", "12345678901");
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            repositorioCheckIn.Verify(x => x.CadastrarAsync(It.IsAny<RegistroCheckIn>()), Times.Once);
            unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task Deve_Falhar_Quando_Veiculo_Nao_Existe()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            tenantProvider.Setup(x => x.UsuarioId).Returns(usuarioId);

            repositorioVeiculo.Setup(x => x.ObterPorPlaca("XYZ999"))
                .ReturnsAsync(new List<Veiculo>());

            validator.Setup(x => x.ValidateAsync(It.IsAny<RealizarCheckInCommand>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = new RealizarCheckInCommandHandler(
                repositorioVeiculo.Object,
                repositorioTicket.Object,
                repositorioCheckIn.Object,
                tenantProvider.Object,
                unitOfWork.Object,
                null!,          
                cache.Object,
                validator.Object,
                logger.Object
            );

            var command = new RealizarCheckInCommand("XYZ999", "12345678901");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsFailed);
            Assert.IsTrue(result.Errors.Any(e => e.Message.Contains("não encontrado")));

            repositorioCheckIn.Verify(x => x.CadastrarAsync(It.IsAny<RegistroCheckIn>()), Times.Never);
            unitOfWork.Verify(x => x.CommitAsync(), Times.Never);
        }

    }
}
