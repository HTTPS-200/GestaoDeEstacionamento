using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloFatura
{
    [TestClass]
    [TestCategory("Repositorio - Fatura")]
    public class FaturaRepositoryTests
    {
        private Mock<IRepositorioFatura>? repoMock;
        private List<Fatura>? faturas;
        private Guid usuarioId = Guid.NewGuid();

        [TestInitialize]
        public void Setup()
        {
            repoMock = new Mock<IRepositorioFatura>();
            faturas = new List<Fatura>
            {
                new Fatura(
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                    "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
                    "A01", "Zona A", DateTime.UtcNow.AddHours(-3), DateTime.UtcNow.AddHours(-1),
                    1, 50.00m, 50.00m, usuarioId
                ),
                new Fatura(
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                    "TKT002", "XYZ5678", "Civic", "Azul", "987.654.321-00",
                    "B01", "Zona B", DateTime.UtcNow.AddHours(-5), DateTime.UtcNow.AddHours(-2),
                    2, 50.00m, 100.00m, usuarioId
                ) { Pago = true, DataPagamento = DateTime.UtcNow },
                new Fatura(
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                    "TKT003", "DEF9012", "Corolla", "Branco", "111.222.333-44",
                    "C01", "Zona C", DateTime.UtcNow.AddHours(-8), DateTime.UtcNow.AddHours(-4),
                    3, 50.00m, 150.00m, usuarioId
                )
            };
        }

        [TestMethod]
        public async Task ObterPorNumeroTicket_RetornaFaturaCorreta()
        {
            var numeroTicket = "TKT001";
            repoMock!.Setup(r => r.ObterPorNumeroTicket(numeroTicket))
                     .ReturnsAsync(faturas!.Find(f => f.NumeroTicket == numeroTicket));

            var resultado = await repoMock.Object.ObterPorNumeroTicket(numeroTicket);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(numeroTicket, resultado.NumeroTicket);
        }

        [TestMethod]
        public async Task ObterPorPlaca_RetornaUltimaFatura()
        {
            var placa = "ABC1234";
            repoMock!.Setup(r => r.ObterPorPlaca(placa))
                     .ReturnsAsync(faturas!.Where(f => f.PlacaVeiculo == placa)
                                          .OrderByDescending(f => f.DataHoraSaida)
                                          .FirstOrDefault());

            var resultado = await repoMock.Object.ObterPorPlaca(placa);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(placa, resultado.PlacaVeiculo);
        }

        [TestMethod]
        public async Task ObterNaoPagas_RetornaApenasNaoPagas()
        {
            repoMock!.Setup(r => r.ObterNaoPagas())
                     .ReturnsAsync(faturas!.Where(f => !f.Pago).ToList());

            var resultado = await repoMock.Object.ObterNaoPagas();
            Assert.AreEqual(2, resultado.Count);
            Assert.IsTrue(resultado.All(f => !f.Pago));
        }

        [TestMethod]
        public async Task ObterPorPeriodo_RetornaFaturasNoPeriodo()
        {
            var inicio = DateTime.UtcNow.AddHours(-6);
            var fim = DateTime.UtcNow.AddHours(-1);

            faturas = new List<Fatura>
    {
        new Fatura(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            "TKT001", "ABC1234", "Fiesta", "Preto", "123.456.789-00",
            "A01", "Zona A",
            DateTime.UtcNow.AddHours(-5), DateTime.UtcNow.AddHours(-4), 
            1, 50.00m, 50.00m, usuarioId
        ),
        new Fatura(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            "TKT002", "DEF5678", "Corsa", "Branco", "987.654.321-00",
            "B02", "Zona B",
            DateTime.UtcNow.AddHours(-3), DateTime.UtcNow.AddHours(-2), 
            1, 60.00m, 60.00m, usuarioId
        ),
        new Fatura(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            "TKT003", "GHI9999", "Onix", "Prata", "555.444.333-22",
            "C03", "Zona C",
            DateTime.UtcNow.AddHours(-10), DateTime.UtcNow.AddHours(-9), 
            1, 70.00m, 70.00m, usuarioId
        )
    };

            repoMock!.Setup(r => r.ObterPorPeriodo(inicio, fim, usuarioId))
                     .ReturnsAsync(faturas.Where(f => f.DataHoraSaida >= inicio &&
                                                      f.DataHoraSaida <= fim &&
                                                      f.UsuarioId == usuarioId).ToList());

            var resultado = await repoMock.Object.ObterPorPeriodo(inicio, fim, usuarioId);

            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod]
        public async Task ObterPorVeiculoId_RetornaFaturasDoVeiculo()
        {
            var veiculoId = faturas![0].VeiculoId;
            repoMock!.Setup(r => r.ObterPorVeiculoId(veiculoId))
                     .ReturnsAsync(faturas.Where(f => f.VeiculoId == veiculoId).ToList());

            var resultado = await repoMock.Object.ObterPorVeiculoId(veiculoId);
            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual(veiculoId, resultado[0].VeiculoId);
        }
    }
}