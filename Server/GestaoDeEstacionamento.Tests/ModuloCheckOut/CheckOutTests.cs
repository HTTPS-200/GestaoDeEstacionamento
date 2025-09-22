using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckOut.Commands;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;
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
        var cpf = "12345678901";
        var placa = "ABC123";

        var veiculo = new Veiculo(placa, "Ford", "Ka", "Preto", "qualquer");
        veiculo.CPFHospede = cpf;

        var ticket = new Ticket(numeroTicket: $"{placa}-0001", veiculoId: veiculo.Id, sequencial: 1);
        var checkIn = new RegistroCheckIn(veiculo, ticket);

        repositorioVeiculo.Setup(x => x.ObterPorPlaca(placa))
            .ReturnsAsync(new List<Veiculo> { veiculo });

        repositorioCheckIn.Setup(x => x.ObterCheckInsPorVeiculoId(veiculo.Id))
            .ReturnsAsync(new List<RegistroCheckIn> { checkIn });

        repositorioTicket.Setup(x => x.SelecionarRegistroPorIdAsync(ticket.Id))
            .ReturnsAsync(ticket);

        repositorioTicket.Setup(x => x.EditarAsync(ticket.Id, It.IsAny<Ticket>()))
            .ReturnsAsync(true);
        repositorioCheckIn.Setup(x => x.EditarAsync(checkIn.Id, It.IsAny<RegistroCheckIn>()))
            .ReturnsAsync(true);
        repositorioVeiculo.Setup(x => x.EditarAsync(veiculo.Id, It.IsAny<Veiculo>()))
            .ReturnsAsync(true);

        repositorioVaga.Setup(x => x.ObterPorVeiculoId(veiculo.Id))
            .ReturnsAsync((Vaga?)null);

        mediator.Setup(x => x.Send(It.IsAny<CriarFaturaCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        unitOfWork.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

        var handler = new RealizarCheckOutCommandHandler(
            repositorioCheckIn.Object,
            repositorioVeiculo.Object,
            repositorioTicket.Object,
            repositorioVaga.Object,
            mediator.Object,
            cache.Object,
            unitOfWork.Object
        );

        var command = new RealizarCheckOutCommand(cpf, placa);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess, $"Checkout falhou: {string.Join(", ", result.Errors.Select(e => e.Message))}");
        Assert.IsFalse(checkIn.Ativo);
        Assert.IsFalse(ticket.Ativo);
    }


}
