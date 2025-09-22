using AutoMapper;
using FluentResults;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Handlers;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloTicket
{
    [TestClass]
    [TestCategory("Handlers - Ticket")]
    public class TicketAppServiceTests
    {
        private Mock<IRepositorioTicket> repoTicketMock;
        private Mock<IRepositorioVeiculo> repoVeiculoMock;
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IMapper> mapperMock;
        private Mock<IDistributedCache> cacheMock;
        private Mock<ILogger<CadastrarTicketCommandHandler>> loggerCadastrarMock;
        private Mock<ILogger<EditarTicketCommandHandler>> loggerEditarMock;
        private Mock<ILogger<ExcluirTicketCommandHandler>> loggerExcluirMock;
        private Mock<ILogger<ObterTicketPorNumeroQueryHandler>> loggerObterMock;
        private Mock<IValidator<CadastrarTicketCommand>> validatorCadastrarMock;
        private Mock<IValidator<EditarTicketCommand>> validatorEditarMock;
        private Mock<ITenantProvider> tenantProviderMock;

        private Guid usuarioId = Guid.NewGuid();

        [TestInitialize]
        public void Setup()
        {
            repoTicketMock = new Mock<IRepositorioTicket>();
            repoVeiculoMock = new Mock<IRepositorioVeiculo>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();
            cacheMock = new Mock<IDistributedCache>();
            loggerCadastrarMock = new Mock<ILogger<CadastrarTicketCommandHandler>>();
            loggerEditarMock = new Mock<ILogger<EditarTicketCommandHandler>>();
            loggerExcluirMock = new Mock<ILogger<ExcluirTicketCommandHandler>>();
            loggerObterMock = new Mock<ILogger<ObterTicketPorNumeroQueryHandler>>();
            validatorCadastrarMock = new Mock<IValidator<CadastrarTicketCommand>>();
            validatorEditarMock = new Mock<IValidator<EditarTicketCommand>>();
            tenantProviderMock = new Mock<ITenantProvider>();
            tenantProviderMock.Setup(t => t.UsuarioId).Returns(usuarioId);
        }

        [TestMethod]
        public async Task CadastrarTicket_Sucesso()
        {
            var veiculo = new Veiculo("ABC123", "Ford", "Ka", "Preto", null) { Id = Guid.NewGuid() };
            repoVeiculoMock.Setup(r => r.ObterPorPlaca("ABC123")).ReturnsAsync(new List<Veiculo> { veiculo });
            repoTicketMock.Setup(r => r.ObterPorVeiculoId(veiculo.Id)).ReturnsAsync(new List<Ticket>());
            repoTicketMock.Setup(r => r.ObterMaiorNumeroSequencial()).ReturnsAsync(0);
            validatorCadastrarMock.Setup(v => v.ValidateAsync(It.IsAny<CadastrarTicketCommand>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = new CadastrarTicketCommandHandler(
                repoTicketMock.Object,
                repoVeiculoMock.Object,
                tenantProviderMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object,
                cacheMock.Object,
                validatorCadastrarMock.Object,
                loggerCadastrarMock.Object
            );

            var command = new CadastrarTicketCommand("ABC123");
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            repoTicketMock.Verify(r => r.CadastrarAsync(It.IsAny<Ticket>()), Times.Once);
            unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task CadastrarTicket_VeiculoNaoEncontrado()
        {
            repoVeiculoMock.Setup(r => r.ObterPorPlaca("ABC123")).ReturnsAsync(new List<Veiculo>());
            validatorCadastrarMock.Setup(v => v.ValidateAsync(It.IsAny<CadastrarTicketCommand>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = new CadastrarTicketCommandHandler(
                repoTicketMock.Object,
                repoVeiculoMock.Object,
                tenantProviderMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object,
                cacheMock.Object,
                validatorCadastrarMock.Object,
                loggerCadastrarMock.Object
            );

            var result = await handler.Handle(new CadastrarTicketCommand("ABC123"), CancellationToken.None);
            Assert.IsTrue(result.IsFailed);
        }

        [TestMethod]
        public async Task CadastrarTicket_ValidatorFalha()
        {
            validatorCadastrarMock.Setup(v => v.ValidateAsync(It.IsAny<CadastrarTicketCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(
                    new[] { new FluentValidation.Results.ValidationFailure("PlacaVeiculo", "Obrigatório") }
                ));

            var handler = new CadastrarTicketCommandHandler(
                repoTicketMock.Object,
                repoVeiculoMock.Object,
                tenantProviderMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object,
                cacheMock.Object,
                validatorCadastrarMock.Object,
                loggerCadastrarMock.Object
            );

            var result = await handler.Handle(new CadastrarTicketCommand(""), CancellationToken.None);
            Assert.IsTrue(result.IsFailed);
        }

        [TestMethod]
        public async Task EditarTicket_Sucesso()
        {
            var ticket = new Ticket("000001", Guid.NewGuid(), 1);
            repoTicketMock.Setup(r => r.SelecionarRegistroPorIdAsync(ticket.Id)).ReturnsAsync(ticket);
            repoVeiculoMock.Setup(r => r.ObterPorPlaca("ABC123"))
                .ReturnsAsync(new List<Veiculo> { new Veiculo("ABC123", "Ford", "Ka", "Preto", null) { Id = Guid.NewGuid() } });
            validatorEditarMock.Setup(v => v.ValidateAsync(It.IsAny<EditarTicketCommand>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = new EditarTicketCommandHandler(
                repoTicketMock.Object,
                repoVeiculoMock.Object,
                tenantProviderMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object,
                cacheMock.Object,
                validatorEditarMock.Object,
                loggerEditarMock.Object
            );

            var command = new EditarTicketCommand(ticket.Id, "ABC123", true);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task EditarTicket_TicketNaoEncontrado()
        {
            repoTicketMock.Setup(r => r.SelecionarRegistroPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Ticket)null);
            validatorEditarMock.Setup(v => v.ValidateAsync(It.IsAny<EditarTicketCommand>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = new EditarTicketCommandHandler(
                repoTicketMock.Object,
                repoVeiculoMock.Object,
                tenantProviderMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object,
                cacheMock.Object,
                validatorEditarMock.Object,
                loggerEditarMock.Object
            );

            var command = new EditarTicketCommand(Guid.NewGuid(), "ABC123", true);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsFailed);
        }

        [TestMethod]
        public async Task ExcluirTicket_Sucesso()
        {
            var handler = new ExcluirTicketCommandHandler(
                repoTicketMock.Object,
                tenantProviderMock.Object,
                unitOfWorkMock.Object,
                cacheMock.Object,
                loggerExcluirMock.Object
            );

            var command = new ExcluirTicketCommand(Guid.NewGuid());
            repoTicketMock.Setup(r => r.ExcluirAsync(command.Id)).ReturnsAsync(true);
            unitOfWorkMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ObterTicketPorNumero_Sucesso()
        {
            var ticket = new Ticket("000001", Guid.NewGuid(), 1);
            repoTicketMock.Setup(r => r.ObterPorNumero("000001")).ReturnsAsync(ticket);
            repoVeiculoMock.Setup(r => r.ObterPorId(ticket.VeiculoId))
                .ReturnsAsync(new Veiculo("ABC123", "Ford", "Ka", "Preto", null) { Id = ticket.VeiculoId });

            var handler = new ObterTicketPorNumeroQueryHandler(
                repoTicketMock.Object,
                repoVeiculoMock.Object,
                mapperMock.Object,
                loggerObterMock.Object
            );

            var result = await handler.Handle(new ObterTicketPorNumeroQuery("000001"), CancellationToken.None);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task ObterTicketPorNumero_NaoEncontrado()
        {
            repoTicketMock.Setup(r => r.ObterPorNumero("000001")).ReturnsAsync((Ticket)null);

            var handler = new ObterTicketPorNumeroQueryHandler(
                repoTicketMock.Object,
                repoVeiculoMock.Object,
                mapperMock.Object,
                loggerObterMock.Object
            );

            var result = await handler.Handle(new ObterTicketPorNumeroQuery("000001"), CancellationToken.None);
            Assert.IsTrue(result.IsFailed);
        }
    }
}
