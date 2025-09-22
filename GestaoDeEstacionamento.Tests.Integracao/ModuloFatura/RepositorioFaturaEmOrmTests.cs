using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using GestaoDeEstacionamento.Infraestrutura.Orm;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFatura;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Testes.Integracao.ModuloFatura;

[TestClass]
public class RepositorioFaturaEmOrmTests
{
    private AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [TestMethod]
    public async Task Deve_Cadastrar_Fatura_Com_Sucesso()
    {
        using var context = CriarContextoEmMemoria();
        var repositorio = new RepositorioFaturaEmOrm(context);

        var fatura = new Fatura(
            checkInId: Guid.NewGuid(),
            veiculoId: Guid.NewGuid(),
            ticketId: Guid.NewGuid(),
            numeroTicket: "000001",
            placaVeiculo: "ABC1234",
            modeloVeiculo: "Corolla",
            corVeiculo: "Prata",
            cpfHospede: "12345678900",
            identificadorVaga: "V01",
            zonaVaga: "A",
            dataHoraEntrada: DateTime.UtcNow.AddHours(-2),
            dataHoraSaida: DateTime.UtcNow,
            diarias: 1,
            valorDiaria: 50,
            valorTotal: 50,
            usuarioId: Guid.NewGuid()
        );

        await repositorio.CadastrarAsync(fatura);
        await context.SaveChangesAsync();

        var faturaCadastrada = await repositorio.SelecionarRegistroPorIdAsync(fatura.Id);

        Assert.IsNotNull(faturaCadastrada);
        Assert.AreEqual(fatura.NumeroTicket, faturaCadastrada.NumeroTicket);
        Assert.AreEqual(fatura.PlacaVeiculo, faturaCadastrada.PlacaVeiculo);
    }

    [TestMethod]
    public async Task Deve_Obter_Fatura_Por_NumeroTicket()
    {
        using var context = CriarContextoEmMemoria();
        var repositorio = new RepositorioFaturaEmOrm(context);

        var fatura = new Fatura(
            checkInId: Guid.NewGuid(),
            veiculoId: Guid.NewGuid(),
            ticketId: Guid.NewGuid(),
            numeroTicket: "000002",
            placaVeiculo: "XYZ9876",
            modeloVeiculo: "Civic",
            corVeiculo: "Preto",
            cpfHospede: "98765432100",
            identificadorVaga: "V02",
            zonaVaga: "B",
            dataHoraEntrada: DateTime.UtcNow.AddHours(-3),
            dataHoraSaida: DateTime.UtcNow,
            diarias: 1,
            valorDiaria: 50,
            valorTotal: 50,
            usuarioId: Guid.NewGuid()
        );

        await repositorio.CadastrarAsync(fatura);
        await context.SaveChangesAsync();

        var faturaObtida = await repositorio.ObterPorNumeroTicket("000002");

        Assert.IsNotNull(faturaObtida);
        Assert.AreEqual("XYZ9876", faturaObtida.PlacaVeiculo);
    }

    [TestMethod]
    public async Task Deve_Obter_Fatura_Por_Placa()
    {
        using var context = CriarContextoEmMemoria();
        var repositorio = new RepositorioFaturaEmOrm(context);

        var fatura = new Fatura(
            checkInId: Guid.NewGuid(),
            veiculoId: Guid.NewGuid(),
            ticketId: Guid.NewGuid(),
            numeroTicket: "000003",
            placaVeiculo: "LMN4567",
            modeloVeiculo: "Fiesta",
            corVeiculo: "Vermelho",
            cpfHospede: "11223344556",
            identificadorVaga: "V03",
            zonaVaga: "C",
            dataHoraEntrada: DateTime.UtcNow.AddHours(-4),
            dataHoraSaida: DateTime.UtcNow,
            diarias: 1,
            valorDiaria: 50,
            valorTotal: 50,
            usuarioId: Guid.NewGuid()
        );

        await repositorio.CadastrarAsync(fatura);
        await context.SaveChangesAsync();

        var faturaObtida = await repositorio.ObterPorPlaca("LMN4567");

        Assert.IsNotNull(faturaObtida);
        Assert.AreEqual("000003", faturaObtida.NumeroTicket);
    }

    [TestMethod]
    public async Task Deve_Obter_Faturas_Nao_Pagas()
    {
        using var context = CriarContextoEmMemoria();
        var repositorio = new RepositorioFaturaEmOrm(context);

        var faturaPaga = new Fatura(
            checkInId: Guid.NewGuid(),
            veiculoId: Guid.NewGuid(),
            ticketId: Guid.NewGuid(),
            numeroTicket: "000004",
            placaVeiculo: "AAA1111",
            modeloVeiculo: "Uno",
            corVeiculo: "Branco",
            cpfHospede: "11122233344",
            identificadorVaga: "V04",
            zonaVaga: "D",
            dataHoraEntrada: DateTime.UtcNow.AddHours(-5),
            dataHoraSaida: DateTime.UtcNow,
            diarias: 1,
            valorDiaria: 50,
            valorTotal: 50,
            usuarioId: Guid.NewGuid()
        );
        faturaPaga.MarcarComoPago();

        var faturaNaoPaga = new Fatura(
            checkInId: Guid.NewGuid(),
            veiculoId: Guid.NewGuid(),
            ticketId: Guid.NewGuid(),
            numeroTicket: "000005",
            placaVeiculo: "BBB2222",
            modeloVeiculo: "Ka",
            corVeiculo: "Azul",
            cpfHospede: "55566677788",
            identificadorVaga: "V05",
            zonaVaga: "E",
            dataHoraEntrada: DateTime.UtcNow.AddHours(-2),
            dataHoraSaida: DateTime.UtcNow,
            diarias: 1,
            valorDiaria: 50,
            valorTotal: 50,
            usuarioId: Guid.NewGuid()
        );

        await repositorio.CadastrarAsync(faturaPaga);
        await repositorio.CadastrarAsync(faturaNaoPaga);
        await context.SaveChangesAsync();

        var faturasNaoPagas = await repositorio.ObterNaoPagas();

        Assert.AreEqual(1, faturasNaoPagas.Count);
        Assert.AreEqual("BBB2222", faturasNaoPagas.First().PlacaVeiculo);
    }
}
