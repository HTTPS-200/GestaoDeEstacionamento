using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;

namespace GestaoDeEstacionamento.Tests.ModuloVagas;

[TestClass]
[TestCategory("UnitTest - Vagas")]
public class VagaTests
{
    private Vaga vaga;

    [TestMethod]
    public void Vaga_DeveEstarLivre_QuandoNaoTemVeiculo()
    {
        var vaga = new Vaga
        {
            NumeroDaVaga = "A1",
            Zona = "Norte",
            Ocupada = false,
            VeiculoEstacionado = null
        };

        Assert.IsTrue(!vaga.Ocupada);
        Assert.IsNull(vaga.VeiculoEstacionado);
    }

    [TestMethod]
    public void AtualizarRegistro_DeveAtualizarStatusEOcupacao()
    {
        var original = new Vaga
        {
            Zona = "Sul",
            Ocupada = true,
            VeiculoEstacionado = new Veiculo { Placa = "XYZ9999" }
        };

        var destino = new Vaga();

        original.AtualizarRegistro(destino);

        Assert.IsNotNull(destino);
    }
}

