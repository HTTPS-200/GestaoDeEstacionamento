using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;

namespace GestaoDeEstacionamento.Tests.ModuloChekIn;

[TestClass]
[TestCategory("UnitTest - Veículo")]
public sealed class VeiculoTests
{
    private Veiculo? veiculo;

    [TestMethod]
    public void Veiculo_Deve_Ser_Invalid_SeCampos_Obrigatorios_Forrem_Nullos()
    {
        veiculo = new Veiculo
        {
            Placa = null!,
            Modelo = null!,
            Cor = null!,
            CpfHospede = null!
        };

        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            if (string.IsNullOrWhiteSpace(veiculo.Placa)) throw new ArgumentNullException(nameof(veiculo.Placa));
            if (string.IsNullOrWhiteSpace(veiculo.Modelo)) throw new ArgumentNullException(nameof(veiculo.Modelo));
            if (string.IsNullOrWhiteSpace(veiculo.Cor)) throw new ArgumentNullException(nameof(veiculo.Cor));
            if (string.IsNullOrWhiteSpace(veiculo.CpfHospede)) throw new ArgumentNullException(nameof(veiculo.CpfHospede));
        });
    }

    [TestMethod]
    public void AtualizarRegistro_Deve_Copiar_Campos_Corretamente()
    {
        veiculo = new Veiculo
        {
            TicketId = 1,
            Placa = "ABC1234",
            Modelo = "Civic",
            Cor = "Prata",
            CpfHospede = "123.456.789-00",
            Observacoes = "Sem danos",
            DataEntrada = new DateTime(2025, 9, 15)
        };

        var destino = new Veiculo();

        veiculo.AtualizarRegistro(destino);

        Assert.AreEqual(veiculo.Placa, destino.Placa);
        Assert.AreEqual(veiculo.Modelo, destino.Modelo);
        Assert.AreEqual(veiculo.Cor, destino.Cor);
        Assert.AreEqual(veiculo.CpfHospede, destino.CpfHospede);
        Assert.AreEqual(veiculo.Observacoes, destino.Observacoes);
        Assert.AreEqual(veiculo.DataEntrada, destino.DataEntrada);
    }
}
