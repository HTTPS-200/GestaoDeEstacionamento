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

namespace GestaoDeEstacionamento.Testes.Integracao.ModuloCheckOut;

[TestClass]
public class RepositorioCheckOutEmOrmTests
{
    private AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [TestMethod]
    public async Task Deve_Realizar_CheckOut_Com_Sucesso()
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
        var ticket = new Ticket("000003", veiculo.Id, 1);
        context.Veiculos.Add(veiculo);
        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();

        var registro = new RegistroCheckIn(veiculo, ticket);
        await context.AddAsync(registro);
        await context.SaveChangesAsync();

        registro.EncerrarCheckIn();
        context.Update(registro);
        await context.SaveChangesAsync();

        var registroFinalizado = await repositorioCheckIn.ObterPorIdAsync(registro.Id);
        Assert.IsFalse(registroFinalizado.Ativo);
        Assert.IsFalse(registroFinalizado.Ticket.Ativo);
    }
}
