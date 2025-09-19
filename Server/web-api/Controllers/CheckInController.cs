using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloCheckIn;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Comands;

namespace GestaoDeEstacionamento.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/checkin")]
public class CheckInController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<RealizarCheckInResponse>> RealizarCheckIn(RealizarCheckInRequest request)
    {
        var command = mapper.Map<RealizarCheckInCommand>(request);

        var result = await mediator.Send(command);

        if (result.IsFailed)
        {
            if (result.HasError(e => e.HasMetadataKey("TipoErro")))
            {
                var errosDeValidacao = result.Errors
                    .SelectMany(e => e.Reasons.OfType<IError>())
                    .Select(e => e.Message);

                return BadRequest(errosDeValidacao);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var response = mapper.Map<RealizarCheckInResponse>(result.Value);

        return Created(string.Empty, response);
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarCheckInsResponse>> SelecionarCheckIns(
        [FromQuery] SelecionarCheckInsRequest? request,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<SelecionarCheckInsQuery>(request);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailed)
            return BadRequest();

        var response = mapper.Map<SelecionarCheckInsResponse>(result.Value);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarCheckInPorIdResponse>> SelecionarCheckInPorId(Guid id)
    {
        var query = mapper.Map<SelecionarCheckInPorIdQuery>(id);

        var result = await mediator.Send(query);

        if (result.IsFailed)
            return NotFound(id);

        var response = mapper.Map<SelecionarCheckInPorIdResponse>(result.Value);

        return Ok(response);
    }
}