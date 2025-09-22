using AutoMapper;
using FluentResults;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Handlers;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloFatura
{
    [TestClass]
    [TestCategory("Handlers - Fatura")]
    public class FaturaAppServiceTests
    {
        private Mock<IRepositorioFatura> repoFaturaMock;
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IMapper> mapperMock;
        private Mock<IDistributedCache> cacheMock;
        private Mock<ILogger<CriarFaturaCommandHandler>> loggerCriarMock;
        private Mock<ILogger<MarcarFaturaComoPagaCommandHandler>> loggerMarcarPagaMock;
        private Mock<ILogger<ObterFaturaPorIdQueryHandler>> loggerObterMock;
        private Mock<IValidator<CriarFaturaCommand>> validatorCriarMock;
        private Mock<ITenantProvider> tenantProviderMock;

        private Guid usuarioId = Guid.NewGuid();

        [TestInitialize]
        public void Setup()
        {
            repoFaturaMock = new Mock<IRepositorioFatura>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();
            cacheMock = new Mock<IDistributedCache>();
            loggerCriarMock = new Mock<ILogger<CriarFaturaCommandHandler>>();
            loggerMarcarPagaMock = new Mock<ILogger<MarcarFaturaComoPagaCommandHandler>>();
            loggerObterMock = new Mock<ILogger<ObterFaturaPorIdQueryHandler>>();
            validatorCriarMock = new Mock<IValidator<CriarFaturaCommand>>();
            tenantProviderMock = new Mock<ITenantProvider>();
            tenantProviderMock.Setup(t => t.UsuarioId).Returns(usuarioId);
        }

        [TestMethod]
        public async Task CriarFatura_Sucesso()
        {
            // Arrange
            var command = new CriarFaturaCommand(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                1, 50.00m, 50.00m
            );

            validatorCriarMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = new CriarFaturaCommandHandler(
                repoFaturaMock.Object,
                tenantProviderMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object,
                cacheMock.Object,
                validatorCriarMock.Object,
                loggerCriarMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            repoFaturaMock.Verify(r => r.CadastrarAsync(It.IsAny<Fatura>()), Times.Once);
            unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task CriarFatura_ValidatorFalha()
        {
            // Arrange
            var command = new CriarFaturaCommand(
                Guid.Empty, Guid.Empty, Guid.Empty,
                "", "", "", "", "",
                null, null, DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
                0, 0m, 0m
            );

            validatorCriarMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(
                    new[] { new FluentValidation.Results.ValidationFailure("PlacaVeiculo", "Obrigatório") }
                ));

            var handler = new CriarFaturaCommandHandler(
                repoFaturaMock.Object,
                tenantProviderMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object,
                cacheMock.Object,
                validatorCriarMock.Object,
                loggerCriarMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsFailed);
        }

        [TestMethod]
        public async Task MarcarFaturaComoPaga_Sucesso()
        {
            // Arrange
            var faturaId = Guid.NewGuid();
            var fatura = new Fatura(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                1, 50.00m, 50.00m, usuarioId
            );

            repoFaturaMock.Setup(r => r.SelecionarRegistroPorIdAsync(faturaId))
                .ReturnsAsync(fatura);

            repoFaturaMock
    .Setup(r => r.EditarAsync(It.IsAny<Guid>(), It.IsAny<Fatura>()))
    .ReturnsAsync(true);

            var handler = new MarcarFaturaComoPagaCommandHandler(
                repoFaturaMock.Object,
                unitOfWorkMock.Object,
                loggerMarcarPagaMock.Object
            );

            // Act
            var result = await handler.Handle(new MarcarFaturaComoPagaCommand(faturaId), CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(fatura.Pago);
            Assert.IsNotNull(fatura.DataPagamento);

            unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }


        [TestMethod]
        public async Task MarcarFaturaComoPaga_NaoEncontrada()
        {
            // Arrange
            repoFaturaMock.Setup(r => r.SelecionarRegistroPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Fatura)null);

            var handler = new MarcarFaturaComoPagaCommandHandler(
                repoFaturaMock.Object,
                unitOfWorkMock.Object,
                loggerMarcarPagaMock.Object
            );

            // Act
            var result = await handler.Handle(new MarcarFaturaComoPagaCommand(Guid.NewGuid()), CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsFailed);
        }

        [TestMethod]
        public async Task ObterFaturaPorId_Sucesso()
        {
            // Arrange
            var faturaId = Guid.NewGuid();
            var fatura = new Fatura(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                1, 50.00m, 50.00m, usuarioId
            );

            repoFaturaMock.Setup(r => r.SelecionarRegistroPorIdAsync(faturaId)).ReturnsAsync(fatura);

            var handler = new ObterFaturaPorIdQueryHandler(
                repoFaturaMock.Object,
                mapperMock.Object
            );

            // Act
            var result = await handler.Handle(new ObterFaturaPorIdQuery(faturaId), CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task ObterFaturaPorId_NaoEncontrada()
        {
            // Arrange
            repoFaturaMock.Setup(r => r.SelecionarRegistroPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Fatura)null);

            var handler = new ObterFaturaPorIdQueryHandler(
                repoFaturaMock.Object,
                mapperMock.Object
            );

            // Act
            var result = await handler.Handle(new ObterFaturaPorIdQuery(Guid.NewGuid()), CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsFailed);
        }

        [TestMethod]
        public async Task ObterFaturaPorTicket_Sucesso()
        {
            // Arrange
            var fatura = new Fatura(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                1, 50.00m, 50.00m, usuarioId
            );

            repoFaturaMock.Setup(r => r.ObterPorNumeroTicket("TKT001")).ReturnsAsync(fatura);

            var handler = new ObterFaturaPorTicketQueryHandler(
                repoFaturaMock.Object,
                mapperMock.Object
            );

            // Act
            var result = await handler.Handle(new ObterFaturaPorTicketQuery("TKT001"), CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task ObterFaturasNaoPagas_Sucesso()
        {
            // Arrange
            var faturas = new List<Fatura>
            {
                new Fatura(
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                    "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                    "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                    1, 50.00m, 50.00m, usuarioId
                )
            };

            repoFaturaMock.Setup(r => r.ObterNaoPagas()).ReturnsAsync(faturas);

            var handler = new ObterFaturasNaoPagasQueryHandler(
                repoFaturaMock.Object,
                mapperMock.Object
            );

            // Act
            var result = await handler.Handle(new ObterFaturasNaoPagasQuery(), CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }
    }
}