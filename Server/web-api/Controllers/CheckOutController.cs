using Microsoft.AspNetCore.Mvc;
using MediatR;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckOut.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloCheckOut;

namespace GestaoDeEstacionamento.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckOutController : ControllerBase
{
    private readonly IMediator _mediator;

    public CheckOutController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> RealizarCheckOut([FromBody] RealizarCheckOutRequest request)
    {
        var command = new RealizarCheckOutCommand(request.CPFHospede, request.PlacaVeiculo);
        var result = await _mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var response = new RealizarCheckOutResponse(
            result.Value.CheckInId,
            result.Value.VeiculoId,
            result.Value.TicketId,
            result.Value.Placa,
            result.Value.NumeroTicket,
            result.Value.Vaga != null ? new VagaInfoResponse(
                result.Value.Vaga.Identificador,
                result.Value.Vaga.Zona
            ) : null,
            result.Value.DataHoraCheckIn,
            result.Value.DataHoraCheckOut,
            result.Value.Diarias,
            result.Value.ValorTotal,
            result.Value.Ativo
        );

        return Ok(response);
    }
}