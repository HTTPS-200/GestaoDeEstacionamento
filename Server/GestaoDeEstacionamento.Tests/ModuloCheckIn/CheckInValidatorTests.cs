using FluentValidation.TestHelper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;
using GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloCheckIn;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestaoDeEstacionamento.TestsUnitarios.ModuloCheckIn;

[TestClass]
public class CheckInValidatorTests
{
    private RealizarCheckInCommandValidator validator;

    [TestInitialize]
    public void Setup() => validator = new RealizarCheckInCommandValidator();

    [TestMethod]
    public void Deve_Falhar_Quando_Placa_Eh_Vazia()
    {
        var command = new RealizarCheckInCommand("", "12345678901");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PlacaVeiculo);
    }

    [TestMethod]
    public void Deve_Falhar_Quando_CPF_Eh_Vazio()
    {
        var command = new RealizarCheckInCommand("ABC123", "");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CPFHospede);
    }

    [TestMethod]
    public void Deve_Passar_Quando_Todos_Campos_Preenchidos()
    {
        var command = new RealizarCheckInCommand("ABC123", "12345678901");
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
