using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckOut.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GestaoDeEstacionamento.TestsUnitarios.ModuloCheckOut;

[TestClass]
public class CheckOutHandlerTests
{
    private Mock<IRepositorioRegistroCheckIn> repositorioCheckIn;
    private Mock<IRepositorioVeiculo> repositorioVeiculo;
    private Mock<IRepositorioTicket> repositorioTicket;
    private Mock<IRepositorioVaga> repositorioVaga;
    private Mock<IMediator> mediator;
    private Mock<IDistributedCache> cache;
    private Mock<IUnitOfWork> unitOfWork;

    [TestInitialize]
    public void Setup()
    {
        repositorioCheckIn = new Mock<IRepositorioRegistroCheckIn>();
        repositorioVeiculo = new Mock<IRepositorioVeiculo>();
        repositorioTicket = new Mock<IRepositorioTicket>();
        repositorioVaga = new Mock<IRepositorioVaga>();
        mediator = new Mock<IMediator>();
        cache = new Mock<IDistributedCache>();
        unitOfWork = new Mock<IUnitOfWork>();
    }

    [TestMethod]
    public async Task Deve_Realizar_CheckOut_Valido()
    {
        var veiculo = new Veiculo("ABC123", "Ford", "Ka", "Preto", "12345678901");
        var ticket = new Ticket(numeroTicket: "ABC123-0001", veiculoId: veiculo.Id, sequencial: 1);
        var checkIn = new RegistroCheckIn(veiculo, ticket);

        repositorioVeiculo.Setup(x => x.ObterPorPlaca(It.IsAny<string>()))
            .ReturnsAsync(new List<Veiculo> { veiculo });
        repositorioCheckIn.Setup(x => x.ObterCheckInsPorVeiculoId(veiculo.Id))
            .ReturnsAsync(new List<RegistroCheckIn> { checkIn });
        repositorioTicket.Setup(x => x.SelecionarRegistroPorIdAsync(ticket.Id))
            .ReturnsAsync(ticket);
        repositorioVaga.Setup(x => x.ObterPorVeiculoId(veiculo.Id))
            .ReturnsAsync((GestaoDeEstacionamento.Core.Dominio.ModuloVaga.Vaga?)null);
        mediator.Setup(x => x.Send(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        var handler = new RealizarCheckOutCommandHandler(
            repositorioCheckIn.Object,
            repositorioVeiculo.Object,
            repositorioTicket.Object,
            repositorioVaga.Object,
            mediator.Object,
            cache.Object,
            unitOfWork.Object
        );

        var command = new RealizarCheckOutCommand("12345678901", "ABC123");
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsFalse(checkIn.Ativo);
        Assert.IsFalse(ticket.Ativo);
    }
}
