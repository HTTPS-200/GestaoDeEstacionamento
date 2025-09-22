using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using Moq;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloVeiculo;

[TestClass]
[TestCategory("Testes - Unidade/Repositório - Veiculo")]
public sealed class VeiculoRepositoryTests
{
    private Mock<IRepositorioVeiculo>? repositorioMock;
    private List<Veiculo>? veiculos;

    [TestInitialize]
    public void Setup()
    {
        repositorioMock = new Mock<IRepositorioVeiculo>();
        veiculos = new List<Veiculo>
        {
            new Veiculo("ABC1234", "Fiesta", "Preto", "123.456.789-00"),
            new Veiculo("XYZ5678", "Civic", "Azul", "987.654.321-00"),
            new Veiculo("DEF9012", "Corolla", "Branco", "111.222.333-44")
        };
    }

    [TestMethod]
    public async Task Deve_Obter_Veiculos_Por_Placa()
    {
        // Arrange
        var placa = "ABC1234";

        repositorioMock!
            .Setup(r => r.ObterPorPlaca(placa))
            .ReturnsAsync(veiculos!.Where(v => v.Placa.Contains(placa)).ToList());

        // Act
        var resultado = await repositorioMock!.Object.ObterPorPlaca(placa);

        // Assert
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual("ABC1234", resultado[0].Placa);
    }

    [TestMethod]
    public async Task Deve_Obter_Veiculos_Estacionados()
    {
        // Arrange
        var veiculoComSaida = new Veiculo("GHI3456", "Golf", "Vermelho", "555.666.777-88");
        veiculoComSaida.RegistrarSaida();

        veiculos!.Add(veiculoComSaida);

        repositorioMock!
            .Setup(r => r.ObterVeiculosEstacionados())
            .ReturnsAsync(veiculos.Where(v => v.DataSaida == null).ToList());

        // Act
        var resultado = await repositorioMock!.Object.ObterVeiculosEstacionados();

        // Assert
        Assert.AreEqual(3, resultado.Count); // Apenas os 3 primeiros estão estacionados
        Assert.IsTrue(resultado.All(v => v.DataSaida == null));
    }

    [TestMethod]
    public async Task Deve_Obter_Veiculo_Por_Id()
    {
        // Arrange
        var veiculo = veiculos![0];
        var id = veiculo.Id;

        repositorioMock!
            .Setup(r => r.ObterPorId(id))
            .ReturnsAsync(veiculo);

        // Act
        var resultado = await repositorioMock!.Object.ObterPorId(id);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual(id, resultado.Id);
        Assert.AreEqual("ABC1234", resultado.Placa);
    }
}