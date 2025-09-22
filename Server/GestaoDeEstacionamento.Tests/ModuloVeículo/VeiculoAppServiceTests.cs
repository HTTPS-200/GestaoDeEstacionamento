using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using AutoMapper;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Handlers;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloVeiculo;

[TestClass]
[TestCategory("Testes - Unidade/Aplicação - Veiculo")]
public sealed class VeiculoAppServiceTests
{
    private Mock<IRepositorioVeiculo>? repositorioVeiculoMock;
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<IMapper>? mapperMock;
    private Mock<IDistributedCache>? cacheMock;
    private Mock<IValidator<CadastrarVeiculoCommand>>? validatorMock;
    private Mock<ILogger<CadastrarVeiculoCommandHandler>>? loggerMock;

    private CadastrarVeiculoCommandHandler? cadastrarVeiculoHandler;

    [TestInitialize]
    public void Setup()
    {
        repositorioVeiculoMock = new Mock<IRepositorioVeiculo>();
        tenantProviderMock = new Mock<ITenantProvider>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        mapperMock = new Mock<IMapper>();
        cacheMock = new Mock<IDistributedCache>();
        validatorMock = new Mock<IValidator<CadastrarVeiculoCommand>>();
        loggerMock = new Mock<ILogger<CadastrarVeiculoCommandHandler>>();

        cadastrarVeiculoHandler = new CadastrarVeiculoCommandHandler(
            repositorioVeiculoMock.Object,
            tenantProviderMock.Object,
            unitOfWorkMock.Object,
            mapperMock.Object,
            cacheMock.Object,
            validatorMock.Object,
            loggerMock.Object
        );
    }

    [TestMethod]
    public async Task Deve_Retornar_OK_Quando_Veiculo_For_Valido()
    {
        // Arrange
        var command = new CadastrarVeiculoCommand(
            "ABC1234", "Fiesta", "Preto", "123.456.789-00", "Observações teste");

        var veiculo = new Veiculo("ABC1234", "Fiesta", "Preto", "123.456.789-00", "Observações teste");
        var resultado = new CadastrarVeiculoResult(veiculo.Id);

        mapperMock!
            .Setup(m => m.Map<CadastrarVeiculoResult>(veiculo))
            .Returns(resultado);

        validatorMock!
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        repositorioVeiculoMock!
            .Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Veiculo>());

        mapperMock!
            .Setup(m => m.Map<Veiculo>(command))
            .Returns(veiculo);

        mapperMock!
            .Setup(m => m.Map<CadastrarVeiculoResult>(veiculo))
            .Returns(resultado);

        tenantProviderMock!
            .Setup(t => t.UsuarioId)
            .Returns(Guid.NewGuid());

        // Act
        var resultadoHandler = await cadastrarVeiculoHandler!.Handle(command, CancellationToken.None);

        // Assert
        repositorioVeiculoMock?.Verify(r => r.CadastrarAsync(veiculo), Times.Once);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Once);

        Assert.IsNotNull(resultadoHandler);
        Assert.IsTrue(resultadoHandler.IsSuccess);
    }

    [TestMethod]
    public async Task Deve_Retornar_Fail_Quando_Veiculo_For_Duplicado()
    {
        // Arrange
        var command = new CadastrarVeiculoCommand(
            "ABC1234", "Fiesta", "Preto", "123.456.789-00");

        var veiculoExistente = new Veiculo("ABC1234", "Civic", "Azul", "987.654.321-00");

        validatorMock!
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        repositorioVeiculoMock!
            .Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Veiculo>() { veiculoExistente });

        // Act
        var resultadoHandler = await cadastrarVeiculoHandler!.Handle(command, CancellationToken.None);

        // Assert
        repositorioVeiculoMock?.Verify(r => r.CadastrarAsync(It.IsAny<Veiculo>()), Times.Never);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);

        Assert.IsNotNull(resultadoHandler);
        Assert.IsTrue(resultadoHandler.IsFailed);
        Assert.AreEqual(1, resultadoHandler.Errors.Count);
    }

    [TestMethod]
    public async Task Deve_Retornar_Fail_Quando_Excessao_For_Lancada()
    {
        // Arrange
        var command = new CadastrarVeiculoCommand(
            "ABC1234", "Fiesta", "Preto", "123.456.789-00");

        var veiculo = new Veiculo("ABC1234", "Fiesta", "Preto", "123.456.789-00");

        validatorMock!
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        repositorioVeiculoMock!
            .Setup(r => r.SelecionarRegistrosAsync())
            .ReturnsAsync(new List<Veiculo>());

        mapperMock!
            .Setup(m => m.Map<Veiculo>(command))
            .Returns(veiculo);

        repositorioVeiculoMock!
            .Setup(r => r.CadastrarAsync(It.IsAny<Veiculo>()))
            .ThrowsAsync(new Exception("Erro ao cadastrar veículo"));

        // Act
        var resultadoHandler = await cadastrarVeiculoHandler!.Handle(command, CancellationToken.None);

        // Assert
        repositorioVeiculoMock?.Verify(r => r.CadastrarAsync(veiculo), Times.Once);
        unitOfWorkMock?.Verify(u => u.CommitAsync(), Times.Never);
        unitOfWorkMock?.Verify(u => u.RollbackAsync(), Times.Once);

        Assert.IsNotNull(resultadoHandler);
        Assert.IsTrue(resultadoHandler.IsFailed);
        Assert.AreEqual(1, resultadoHandler.Errors.Count);
    }
}