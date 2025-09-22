
using GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloVeiculo;

[TestClass]
[TestCategory("Testes - Unidade/Validação - Veiculo")]
public sealed class VeiculoValidatorTests
{
    private CadastrarVeiculoCommandValidator? validator;

    [TestInitialize]
    public void Setup()
    {
        validator = new CadastrarVeiculoCommandValidator();
    }

    [TestMethod]
    public void Deve_Validar_Comando_Correto()
    {
        // Arrange
        var command = new CadastrarVeiculoCommand(
            "ABC1234", "Fiesta", "Preto", "123.456.789-00", "Observações");

        // Act
        var resultado = validator!.Validate(command);

        // Assert
        Assert.IsTrue(resultado.IsValid);
    }

    [TestMethod]
    public void Deve_Invalidar_Comando_Com_Placa_Vazia()
    {
        // Arrange
        var command = new CadastrarVeiculoCommand(
            "", "Fiesta", "Preto", "123.456.789-00");

        // Act
        var resultado = validator!.Validate(command);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.AreEqual("Placa é obrigatória", resultado.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Deve_Invalidar_Comando_Com_Placa_Longa_Demais()
    {
        // Arrange
        var command = new CadastrarVeiculoCommand(
            "ABC123456789", "Fiesta", "Preto", "123.456.789-00");

        // Act
        var resultado = validator!.Validate(command);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.AreEqual("Placa deve ter no máximo 10 caracteres", resultado.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Deve_Invalidar_Comando_Com_Modelo_Vazio()
    {
        // Arrange
        var command = new CadastrarVeiculoCommand(
            "ABC1234", "", "Preto", "123.456.789-00");

        // Act
        var resultado = validator!.Validate(command);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.AreEqual("Modelo é obrigatório", resultado.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Deve_Invalidar_Comando_Com_CPF_Vazio()
    {
        // Arrange
        var command = new CadastrarVeiculoCommand(
            "ABC1234", "Fiesta", "Preto", "");

        // Act
        var resultado = validator!.Validate(command);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.AreEqual("CPF do hóspede é obrigatório", resultado.Errors[0].ErrorMessage);
    }

    [DataTestMethod]
    [DataRow("AAAAAAAAAAA", "Modelo", "Cor", "12345678901", null, false, "Placa deve ter no máximo 10 caracteres")] // Placa inválida
    [DataRow("ABC1234", "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB", "Cor", "12345678901", null, false, "Modelo deve ter no máximo 50 caracteres")] // Modelo inválido
    [DataRow("ABC1234", "Modelo", "CCCCCCCCCCCCCCCCCCCCC", "12345678901", null, false, "Cor deve ter no máximo 20 caracteres")] // Cor inválida
    [DataRow("ABC1234", "Modelo", "Cor", "DDDDDDDDDDDDDDD", null, false, "CPF deve ter no máximo 14 caracteres")] // CPF inválido
    [DataRow("ABC1234", "Modelo", "Cor", "12345678901", "Eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee", false, "Observações deve ter no máximo 500 caracteres")] // Observações inválidas
    [DataRow("ABC1234", "Modelo", "Cor", "12345678901", "Observação válida", true, null)] 
    public void Deve_Validar_Campos_Do_Command(
           string placa,
           string modelo,
           string cor,
           string cpf,
           string observacoes,
           bool esperadoValido,
           string? mensagemErroEsperada)
    {
        // Arrange
        var command = new CadastrarVeiculoCommand(placa, modelo, cor, cpf, observacoes);

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.AreEqual(esperadoValido, result.IsValid);

        if (!esperadoValido)
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == mensagemErroEsperada));
    }
}