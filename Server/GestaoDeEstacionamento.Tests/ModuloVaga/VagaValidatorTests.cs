using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Validators;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloVaga;

[TestClass]
[TestCategory("Testes - Unidade/Validação - Vaga")]
public sealed class VagaValidatorTests
{
    private VagaValidator? validator;

    [TestInitialize]
    public void Setup()
    {
        validator = new VagaValidator();
    }

    [TestMethod]
    public void Deve_Validar_Vaga_Correta()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var vaga = new Vaga("A01", "Zona A", usuarioId);

        // Act
        var resultado = validator!.Validate(vaga);

        // Assert
        Assert.IsTrue(resultado.IsValid);
    }

    [TestMethod]
    public void Deve_Invalidar_Vaga_Com_Identificador_Vazio()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var vaga = new Vaga("", "Zona A", usuarioId);

        // Act
        var resultado = validator!.Validate(vaga);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.AreEqual("Identificador da vaga é obrigatório", resultado.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Deve_Invalidar_Vaga_Com_Identificador_Longo_Demais()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var vaga = new Vaga("A01A01A01A01A01A01A01", "Zona A", usuarioId);

        // Act
        var resultado = validator!.Validate(vaga);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.AreEqual("Identificador deve ter no máximo 20 caracteres", resultado.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Deve_Invalidar_Vaga_Com_Zona_Vazia()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var vaga = new Vaga("A01", "", usuarioId);

        // Act
        var resultado = validator!.Validate(vaga);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.AreEqual("Zona da vaga é obrigatória", resultado.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Deve_Invalidar_Vaga_Com_Zona_Longa_Demais()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        // Gerando uma string com mais de 50 caracteres
        var zonaMuitoLonga = new string('A', 51);
        var vaga = new Vaga("A01", zonaMuitoLonga, usuarioId);

        // Act
        var resultado = validator!.Validate(vaga);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.AreEqual("Zona deve ter no máximo 50 caracteres", resultado.Errors[0].ErrorMessage);
    }

    [DataTestMethod]
    [DataRow("", "Zona A", false, "Identificador da vaga é obrigatório")]
    [DataRow("A01A01A01A01A01A01A01", "Zona A", false, "Identificador deve ter no máximo 20 caracteres")]
    [DataRow("A01", "", false, "Zona da vaga é obrigatória")]
    [DataRow("A01", "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", false, "Zona deve ter no máximo 50 caracteres")] // 51 caracteres
    [DataRow("A01", "Zona A", true, null)]
    public void Deve_Validar_Campos_Da_Vaga(
    string identificador,
    string zona,
    bool esperadoValido,
    string? mensagemErroEsperada)
    {
        var usuarioId = Guid.NewGuid();
        var vaga = new Vaga(identificador, zona, usuarioId);

        var result = validator!.Validate(vaga);

        Assert.AreEqual(esperadoValido, result.IsValid);

        if (!esperadoValido)
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == mensagemErroEsperada));
    }
}