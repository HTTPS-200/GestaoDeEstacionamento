using FizzWare.NBuilder;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Tests.Integracao.Compartilhado;

namespace GestaoDeEstacionamento.Tests.Integracao.ModuloVeiculo;

[TestClass]
[TestCategory("Integração - Veículo")]
public class RepositorioVeiculoEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Veiculo()
    {
        // arrange
        var veiculo = Builder<Veiculo>.CreateNew()
            .With(v => v.Id = Guid.NewGuid())
            .With(v => v.Placa = "ABC1234")
            .With(v => v.Modelo = "Fusca")
            .With(v => v.Cor = "Azul")
            .With(v => v.CPFHospede = "12345678900")
            .With(v => v.UsuarioId = Guid.NewGuid())
            .With(v => v.DataEntrada = DateTime.UtcNow) 
            .With(v => v.DataSaida = null)
            .Build();

        // act
        await repositorioVeiculo!.CadastrarAsync(veiculo);
        await dbContext!.SaveChangesAsync();

        var veiculoDb = await repositorioVeiculo.ObterPorId(veiculo.Id);

        // assert
        Assert.IsNotNull(veiculoDb);
        Assert.AreEqual("ABC1234", veiculoDb!.Placa);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Veiculo_Por_Placa()
    {
        // arrange
        var veiculo = Builder<Veiculo>.CreateNew()
            .With(v => v.Id = Guid.NewGuid())
            .With(v => v.Placa = "XYZ9876")
            .With(v => v.Modelo = "Gol")
            .With(v => v.Cor = "Preto")
            .With(v => v.CPFHospede = "98765432100")
            .With(v => v.UsuarioId = Guid.NewGuid())
            .With(v => v.DataEntrada = DateTime.UtcNow)
            .With(v => v.DataSaida = null)
            .Build();

        await repositorioVeiculo!.CadastrarAsync(veiculo);
        await dbContext!.SaveChangesAsync();

        // act
        var veiculos = await repositorioVeiculo.ObterPorPlaca("XYZ");

        // assert
        Assert.AreEqual(1, veiculos.Count);
        Assert.AreEqual("Gol", veiculos.First().Modelo);
    }

    [TestMethod]
    public async Task Deve_Selecionar_Veiculos_Estacionados()
    {
        // arrange
        var veiculoEstacionado = Builder<Veiculo>.CreateNew()
            .With(v => v.Id = Guid.NewGuid())
            .With(v => v.DataEntrada = DateTime.UtcNow)
            .With(v => v.DataSaida = null)
            .With(v => v.UsuarioId = Guid.NewGuid())
            .With(v => v.Placa = "EST1234")
            .Build();

        var veiculoSaiu = Builder<Veiculo>.CreateNew()
            .With(v => v.Id = Guid.NewGuid())
            .With(v => v.DataEntrada = DateTime.UtcNow.AddHours(-1))
            .With(v => v.DataSaida = DateTime.UtcNow)
            .With(v => v.UsuarioId = Guid.NewGuid())
            .With(v => v.Placa = "SAI5678")
            .Build();

        await repositorioVeiculo!.CadastrarAsync(veiculoEstacionado);
        await repositorioVeiculo!.CadastrarAsync(veiculoSaiu);
        await dbContext!.SaveChangesAsync();

        // act
        var veiculosEstacionados = await repositorioVeiculo.ObterVeiculosEstacionados();

        // assert
        Assert.AreEqual(1, veiculosEstacionados.Count);
        Assert.AreEqual(veiculoEstacionado.Placa, veiculosEstacionados.First().Placa);
    }
}
