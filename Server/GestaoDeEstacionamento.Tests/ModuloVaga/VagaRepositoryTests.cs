using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using Moq;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloVaga;

[TestClass]
[TestCategory("Testes - Unidade/Repositório - Vaga")]
public sealed class VagaRepositoryTests
{
    private Mock<IRepositorioVaga>? repositorioMock;
    private List<Vaga>? vagas;

    [TestInitialize]
    public void Setup()
    {
        repositorioMock = new Mock<IRepositorioVaga>();
        var usuarioId = Guid.NewGuid();

        vagas = new List<Vaga>
        {
            new Vaga("A01", "Zona A", usuarioId),
            new Vaga("A02", "Zona A", usuarioId),
            new Vaga("B01", "Zona B", usuarioId)
        };
    }

    [TestMethod]
    public async Task Deve_Obter_Vagas_Livres()
    {
        // Arrange
        var vagasLivres = vagas!.Where(v => !v.Ocupada).ToList();

        repositorioMock!
            .Setup(r => r.ObterVagasLivres())
            .ReturnsAsync(vagasLivres);

        // Act
        var resultado = await repositorioMock!.Object.ObterVagasLivres();

        // Assert
        Assert.AreEqual(3, resultado.Count);
        Assert.IsTrue(resultado.All(v => !v.Ocupada));
    }

    [TestMethod]
    public async Task Deve_Obter_Vagas_Ocupadas()
    {
        // Arrange
        var vagaOcupada = vagas![0];
        vagaOcupada.Ocupar(Guid.NewGuid());

        var vagasOcupadas = new List<Vaga> { vagaOcupada };

        repositorioMock!
            .Setup(r => r.ObterVagasOcupadas())
            .ReturnsAsync(vagasOcupadas);

        // Act
        var resultado = await repositorioMock!.Object.ObterVagasOcupadas();

        // Assert
        Assert.AreEqual(1, resultado.Count);
        Assert.IsTrue(resultado.All(v => v.Ocupada));
    }

    [TestMethod]
    public async Task Deve_Obter_Vaga_Por_Identificador()
    {
        // Arrange
        var identificador = "A01";
        var vaga = vagas![0];

        repositorioMock!
            .Setup(r => r.ObterPorIdentificador(identificador))
            .ReturnsAsync(vaga);

        // Act
        var resultado = await repositorioMock!.Object.ObterPorIdentificador(identificador);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual(identificador, resultado.Identificador);
    }

    [TestMethod]
    public async Task Deve_Verificar_Disponibilidade_Vaga()
    {
        // Arrange
        var identificador = "A01";
        var vaga = vagas![0];

        repositorioMock!
            .Setup(r => r.VerificarDisponibilidade(identificador))
            .ReturnsAsync(true);

        // Act
        var resultado = await repositorioMock!.Object.VerificarDisponibilidade(identificador);

        // Assert
        Assert.IsTrue(resultado);
    }
}