using FluentValidation.TestHelper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloFatura
{
    [TestClass]
    [TestCategory("Validação - Fatura")]
    public class FaturaValidatorTests
    {
        private CriarFaturaCommandValidator? criarValidator;

        [TestInitialize]
        public void Setup()
        {
            criarValidator = new CriarFaturaCommandValidator();
        }

        [TestMethod]
        public void CriarFaturaCommand_Valido()
        {
            // Arrange
            var command = new CriarFaturaCommand(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                1, 50.00m, 50.00m
            );

            // Act
            var result = criarValidator!.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void CriarFaturaCommand_PlacaVazia_Invalido()
        {
            // Arrange
            var command = new CriarFaturaCommand(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                1, 50.00m, 50.00m
            );

            // Act
            var result = criarValidator!.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PlacaVeiculo);
        }

        [TestMethod]
        public void CriarFaturaCommand_PlacaLonga_Invalido()
        {
            // Arrange
            var command = new CriarFaturaCommand(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC123456789", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                1, 50.00m, 50.00m
            );

            // Act
            var result = criarValidator!.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PlacaVeiculo);
        }

        [TestMethod]
        public void CriarFaturaCommand_DataEntradaPosteriorSaida_Invalido()
        {
            // Arrange
            var command = new CriarFaturaCommand(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow, DateTime.UtcNow.AddHours(-2),
                1, 50.00m, 50.00m
            );

            // Act
            var result = criarValidator!.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DataHoraEntrada);
        }

        [TestMethod]
        public void CriarFaturaCommand_DiariasZero_Invalido()
        {
            // Arrange
            var command = new CriarFaturaCommand(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                0, 50.00m, 0m
            );

            // Act
            var result = criarValidator!.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Diarias);
        }

        [TestMethod]
        public void CriarFaturaCommand_ValorTotalIncorreto_Invalido()
        {
            // Arrange
            var command = new CriarFaturaCommand(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                2, 50.00m, 75.00m // Deveria ser 100.00m
            );

            // Act
            var result = criarValidator!.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ValorTotal);
        }

        [TestMethod]
        public void CriarFaturaCommand_NumeroTicketVazio_Invalido()
        {
            // Arrange
            var command = new CriarFaturaCommand(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                1, 50.00m, 50.00m
            );

            // Act
            var result = criarValidator!.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NumeroTicket);
        }

        [TestMethod]
        public void CriarFaturaCommand_CPFVazio_Invalido()
        {
            // Arrange
            var command = new CriarFaturaCommand(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                1, 50.00m, 50.00m
            );

            // Act
            var result = criarValidator!.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CPFHospede);
        }
    }
}