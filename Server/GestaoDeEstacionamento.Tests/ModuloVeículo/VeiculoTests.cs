using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloVeiculo;

[TestClass]
[TestCategory("Testes - Unidade - Veiculo")]
public sealed class VeiculoTests
{
    private Veiculo? veiculo;

    // Teste campos obrigatorios - CT001 / CT002 / CT003 / CT004
    [TestMethod]
    public void Deve_Impedir_Cadastro_Veiculo_Com_Campos_Obrigatorios_Vazios()
    {
        // Arrange
        veiculo = new Veiculo("", "", "", "");

        // Act
        var placaVazia = string.IsNullOrWhiteSpace(veiculo.Placa);
        var modeloVazio = string.IsNullOrWhiteSpace(veiculo.Modelo);
        var corVazia = string.IsNullOrWhiteSpace(veiculo.Cor);
        var cpfVazio = string.IsNullOrWhiteSpace(veiculo.CPFHospede);

        // Assert
        Assert.IsTrue(placaVazia, "A placa do veículo não pode estar vazia.");
        Assert.IsTrue(modeloVazio, "O modelo do veículo não pode estar vazio.");
        Assert.IsTrue(corVazia, "A cor do veículo não pode estar vazia.");
        Assert.IsTrue(cpfVazio, "O CPF do hóspede não pode estar vazio.");
    }

    // Teste placa duplicada - CT005
    [TestMethod]
    public void Deve_Impedir_Cadastro_Veiculo_Com_Placa_Duplicada()
    {
        // Arrange
        var listaVeiculos = new List<Veiculo>
        {
            new Veiculo("ABC1234", "Fiesta", "Preto", "123.456.789-00"),
            new Veiculo("XYZ5678", "Civic", "Azul", "987.654.321-00"),
            new Veiculo("DEF9012", "Corolla", "Branco", "111.222.333-44")
        };

        var novoVeiculo = new Veiculo("ABC1234", "Golf", "Vermelho", "555.666.777-88");

        // Act
        var placaDuplicada = listaVeiculos.Any(v =>
            v.Placa.Equals(novoVeiculo.Placa, StringComparison.OrdinalIgnoreCase));

        // Assert
        Assert.IsTrue(placaDuplicada, "Já existe um veículo registrado com esta placa.");
    }

    // Teste registro de saída - CT006
    [TestMethod]
    public void Deve_Registrar_Saida_Veiculo_Corretamente()
    {
        // Arrange
        var veiculo = new Veiculo("ABC1234", "Fiesta", "Preto", "123.456.789-00");
        var dataSaidaAnterior = veiculo.DataSaida;

        // Act
        veiculo.RegistrarSaida();

        // Assert
        Assert.IsNull(dataSaidaAnterior, "Data de saída deve ser nula antes do registro");
        Assert.IsNotNull(veiculo.DataSaida, "Data de saída não pode ser nula após o registro");
        Assert.IsTrue(veiculo.DataSaida.Value <= DateTime.UtcNow,
            "Data de saída deve ser menor ou igual à data atual");
    }

    // Teste atualização de registro - CT007
    [TestMethod]
    public void Deve_Atualizar_Registro_Veiculo_Corretamente()
    {
        // Arrange
        var veiculoOriginal = new Veiculo("ABC1234", "Fiesta", "Preto", "123.456.789-00", "Observação original");
        var veiculoEditado = new Veiculo("XYZ5678", "Civic", "Azul", "987.654.321-00", "Observação editada");

        // Act
        veiculoOriginal.AtualizarRegistro(veiculoEditado);

        // Assert
        Assert.AreEqual("XYZ5678", veiculoOriginal.Placa);
        Assert.AreEqual("Civic", veiculoOriginal.Modelo);
        Assert.AreEqual("Azul", veiculoOriginal.Cor);
        Assert.AreEqual("987.654.321-00", veiculoOriginal.CPFHospede);
        Assert.AreEqual("Observação editada", veiculoOriginal.Observacoes);
    }
  
}