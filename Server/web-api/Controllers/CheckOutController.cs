using GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Comands;
using GestaoDeEstacionamento.WebApi.Models.ModuloRegistroCheckIn;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeEstacionamento.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckOutController : Controller
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
                result.Value.TickeId,
                result.Value.Placa,
                result.Value.NumeroTicket,
                result.Value.DataHoraCheckIn,
                result.Value.DataHoraCheckOut,
                result.Value.Diarias,
                result.Value.ValorTotal,
                result.Value.Ativo,
                result.Value.Vaga != null ? new VagaInfoResponse(
                    result.Value.Vaga.Identificador, result.Value.Vaga.Zona) : null
        );

        return Ok(response);
    }
}
