using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using AutoMapper;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using FluentValidation.Results;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloVaga;

[TestClass]
[TestCategory("Testes - Unidade/Aplicação - Vaga")]
public sealed class VagaAppServiceTests
{
    private Mock<IRepositorioVaga>? repositorioVagaMock;
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<IMapper>? mapperMock;
    private Mock<IDistributedCache>? cacheMock;
    private Mock<IValidator<Vaga>>? validatorMock;
    private Mock<ILogger<CriarVagaCommandHandler>>? loggerMock;

    private CriarVagaCommandHandler? criarVagaHandler;

    [TestInitialize]
    public void Setup()
    {
        repositorioVagaMock = new Mock<IRepositorioVaga>();
        tenantProviderMock = new Mock<ITenantProvider>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        mapperMock = new Mock<IMapper>();
        cacheMock = new Mock<IDistributedCache>();
        validatorMock = new Mock<IValidator<Vaga>>();
        loggerMock = new Mock<ILogger<CriarVagaCommandHandler>>();

        criarVagaHandler = new CriarVagaCommandHandler(
            repositorioVagaMock.Object,
            tenantProviderMock.Object,
            unitOfWorkMock.Object,
            mapperMock.Object,
            cacheMock.Object,
            validatorMock.Object,
            loggerMock.Object
        );
    }

    [TestMethod]
    public async Task Deve_Retornar_OK_Quando_Vaga_For_Valida()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var command = new CriarVagaCommand("A01", "Zona A", usuarioId);

        var vaga = new Vaga("A01", "Zona A", usuarioId);
        var resultado = new CriarVagaResult(vaga.Id, vaga.Identificador, vaga.Zona, vaga.Ocupada);

        validatorMock!
            .Setup(v => v.ValidateAsync(It.IsAny<Vaga>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        repositorioVagaMock!
            .Setup(r => r.ObterPorIdentificador(command.Identificador))
            .ReturnsAsync((Vaga?)null);

        mapperMock!
            .Setup(m => m.Map<CriarVagaResult>(It.IsAny<Vaga>()))
            .Returns(resultado);

        tenantProviderMock!
            .Setup(t => t.UsuarioId)
            .Returns(usuarioId);

        // Act
        var resultadoHandler = await criarVagaHandler!.Handle(command, CancellationToken.None);

        // Assert
        repositorioVagaMock?.Verify(r => r.CadastrarAsync(It.IsAny<Vaga>()), Times.Once);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);

        Assert.IsNotNull(resultadoHandler);
        Assert.IsTrue(resultadoHandler.IsSuccess);
    }

    [TestMethod]
    public async Task Deve_Retornar_Fail_Quando_Vaga_For_Duplicada()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var command = new CriarVagaCommand("A01", "Zona A", usuarioId);

        var vagaExistente = new Vaga("A01", "Zona A", usuarioId);

        repositorioVagaMock!
            .Setup(r => r.ObterPorIdentificador(command.Identificador))
            .ReturnsAsync(vagaExistente);

        // Act
        var resultadoHandler = await criarVagaHandler!.Handle(command, CancellationToken.None);

        // Assert
        repositorioVagaMock?.Verify(r => r.CadastrarAsync(It.IsAny<Vaga>()), Times.Never);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);

        Assert.IsNotNull(resultadoHandler);
        Assert.IsTrue(resultadoHandler.IsFailed);
        Assert.AreEqual(1, resultadoHandler.Errors.Count);
    }

    [TestMethod]
    public async Task Deve_Retornar_Fail_Quando_Excessao_For_Lancada()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var command = new CriarVagaCommand("A01", "Zona A", usuarioId);

        validatorMock!
            .Setup(v => v.ValidateAsync(It.IsAny<Vaga>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        repositorioVagaMock!
            .Setup(r => r.ObterPorIdentificador(command.Identificador))
            .ReturnsAsync((Vaga?)null);

        repositorioVagaMock!
            .Setup(r => r.CadastrarAsync(It.IsAny<Vaga>()))
            .ThrowsAsync(new Exception("Erro ao cadastrar vaga"));

        // Act
        var resultadoHandler = await criarVagaHandler!.Handle(command, CancellationToken.None);

        // Assert
        repositorioVagaMock?.Verify(r => r.CadastrarAsync(It.IsAny<Vaga>()), Times.Once);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
        unitOfWorkMock?.Verify(u => u.RollbackAsync(), Times.Once);

        Assert.IsNotNull(resultadoHandler);
        Assert.IsTrue(resultadoHandler.IsFailed);
        Assert.AreEqual(1, resultadoHandler.Errors.Count);
    }
}
