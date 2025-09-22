using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Infraestrutura.Orm;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloCheckIn;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Testes.Integracao.ModuloCheckIn;

[TestClass]
public class RepositorioRegistroCheckInEmOrmTests
{
    private AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [TestMethod]
    public async Task Deve_Cadastrar_CheckIn_Com_Sucesso()
    {
        using var context = CriarContextoEmMemoria();
        var repositorioCheckIn = new RepositorioRegistroCheckInEmOrm(context);

        var veiculo = new Veiculo(
   placa: "ABC908",
   modelo: "Fox",
   cor: "Preto",
   cpfHospede: "12345678999",
   observacoes: "carro batido 2x"

);
        var ticket = new Ticket("000001", veiculo.Id, 1);
        context.Veiculos.Add(veiculo);
        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();

        var registro = new RegistroCheckIn(veiculo, ticket);
        await context.AddAsync(registro);
        await context.SaveChangesAsync();

        var checkInCadastrado = await repositorioCheckIn.ObterPorNumeroTicket(ticket.NumeroTicket);

        Assert.IsNotNull(checkInCadastrado);
        Assert.AreEqual(veiculo.Id, checkInCadastrado.VeiculoId);
        Assert.IsTrue(checkInCadastrado.Ativo);
    }

    [TestMethod]
    public async Task Deve_Encerrar_CheckIn_Com_Sucesso()
    {
        using var context = CriarContextoEmMemoria();
        var repositorioCheckIn = new RepositorioRegistroCheckInEmOrm(context);

        var veiculo = new Veiculo(
     placa: "ABC123",
     modelo: "Corolla",
     cor: "Prata",
     cpfHospede: "12345678900",
     observacoes: "carro batido"
    
);
        var ticket = new Ticket("000002", veiculo.Id, 1);
        context.Veiculos.Add(veiculo);
        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();

        var registro = new RegistroCheckIn(veiculo, ticket);
        await context.AddAsync(registro);
        await context.SaveChangesAsync();

        // Encerrar check-in
        registro.EncerrarCheckIn();
        context.Update(registro);
        await context.SaveChangesAsync();

        var registroAtualizado = await repositorioCheckIn.ObterPorIdAsync(registro.Id);
        Assert.IsFalse(registroAtualizado.Ativo);
        Assert.IsFalse(registroAtualizado.Ticket.Ativo);
    }
}
