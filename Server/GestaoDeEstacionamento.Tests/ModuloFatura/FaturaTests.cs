using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloFatura
{
    [TestClass]
    [TestCategory("Domínio - Fatura")]
    public class FaturaTests
    {
        private Guid usuarioId = Guid.NewGuid();

        [TestMethod]
        public void CriarFatura_Deve_ConfigurarPropriedadesCorretamente()
        {
            // Arrange & Act
            var fatura = new Fatura(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                1, 50.00m, 50.00m, usuarioId
            );

            // Assert
            Assert.AreEqual("TKT001", fatura.NumeroTicket);
            Assert.AreEqual("ABC1234", fatura.PlacaVeiculo);
            Assert.AreEqual("Fiesta", fatura.ModeloVeiculo);
            Assert.AreEqual("Preto", fatura.CorVeiculo);
            Assert.AreEqual("123.456.789-00", fatura.CPFHospede);
            Assert.AreEqual("A01", fatura.IdentificadorVaga);
            Assert.AreEqual("Zona A", fatura.ZonaVaga);
            Assert.AreEqual(1, fatura.Diarias);
            Assert.AreEqual(50.00m, fatura.ValorDiaria);
            Assert.AreEqual(50.00m, fatura.ValorTotal);
            Assert.IsFalse(fatura.Pago);
            Assert.IsNull(fatura.DataPagamento);
            Assert.AreEqual(usuarioId, fatura.UsuarioId);
            Assert.AreNotEqual(Guid.Empty, fatura.Id);
        }

        [TestMethod]
        public void MarcarComoPago_Deve_AtualizarPropriedades()
        {
            // Arrange
            var fatura = new Fatura(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                1, 50.00m, 50.00m, usuarioId
            );

            // Act
            fatura.MarcarComoPago();

            // Assert
            Assert.IsTrue(fatura.Pago);
            Assert.IsNotNull(fatura.DataPagamento);
            Assert.IsTrue(fatura.DataPagamento.Value <= DateTime.UtcNow);
        }

        [TestMethod]
        public void AtualizarRegistro_Deve_AtualizarTodasPropriedades()
        {
            // Arrange
            var faturaOriginal = new Fatura(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-3), DateTime.UtcNow.AddHours(-1),
                1, 50.00m, 50.00m, usuarioId
            );

            var faturaEditada = new Fatura(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT002", "XYZ5678", "Civic", "Azul", "987.654.321-00",
                "B01", "Zona B", DateTime.UtcNow.AddHours(-5), DateTime.UtcNow.AddHours(-2),
                2, 60.00m, 120.00m, Guid.NewGuid()
            )
            {
                Pago = true,
                DataPagamento = DateTime.UtcNow
            };

            // Act
            faturaOriginal.AtualizarRegistro(faturaEditada);

            // Assert
            Assert.AreEqual(faturaEditada.CheckInId, faturaOriginal.CheckInId);
            Assert.AreEqual(faturaEditada.VeiculoId, faturaOriginal.VeiculoId);
            Assert.AreEqual(faturaEditada.TicketId, faturaOriginal.TicketId);
            Assert.AreEqual(faturaEditada.NumeroTicket, faturaOriginal.NumeroTicket);
            Assert.AreEqual(faturaEditada.PlacaVeiculo, faturaOriginal.PlacaVeiculo);
            Assert.AreEqual(faturaEditada.ModeloVeiculo, faturaOriginal.ModeloVeiculo);
            Assert.AreEqual(faturaEditada.CorVeiculo, faturaOriginal.CorVeiculo);
            Assert.AreEqual(faturaEditada.CPFHospede, faturaOriginal.CPFHospede);
            Assert.AreEqual(faturaEditada.IdentificadorVaga, faturaOriginal.IdentificadorVaga);
            Assert.AreEqual(faturaEditada.ZonaVaga, faturaOriginal.ZonaVaga);
            Assert.AreEqual(faturaEditada.DataHoraEntrada, faturaOriginal.DataHoraEntrada);
            Assert.AreEqual(faturaEditada.DataHoraSaida, faturaOriginal.DataHoraSaida);
            Assert.AreEqual(faturaEditada.Diarias, faturaOriginal.Diarias);
            Assert.AreEqual(faturaEditada.ValorDiaria, faturaOriginal.ValorDiaria);
            Assert.AreEqual(faturaEditada.ValorTotal, faturaOriginal.ValorTotal);
            Assert.AreEqual(faturaEditada.Pago, faturaOriginal.Pago);
            Assert.AreEqual(faturaEditada.DataPagamento, faturaOriginal.DataPagamento);
            Assert.AreEqual(faturaEditada.UsuarioId, faturaOriginal.UsuarioId);
        }

        [TestMethod]
        public void ValorTotal_Deve_SerCalculadoCorretamente()
        {
            // Arrange & Act
            var fatura = new Fatura(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                "A01", "Zona A", DateTime.UtcNow.AddHours(-2), DateTime.UtcNow,
                3, 50.00m, 150.00m, usuarioId
            );

            // Assert
            Assert.AreEqual(150.00m, fatura.ValorTotal);
            Assert.AreEqual(3 * 50.00m, fatura.ValorTotal);
        }
    }
}