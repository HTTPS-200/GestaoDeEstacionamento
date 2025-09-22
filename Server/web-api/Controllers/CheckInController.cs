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
       [FromQuery] int? Quantidade,
       CancellationToken cancellationToken)
    {
        var query = new SelecionarCheckInsQuery(Quantidade);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailed)
        {
            if (result.Errors.Any(e => e.Message.Contains("não encontrado") ||
                                      e.Message.Contains("not found") ||
                                      e.HasMetadataKey("TipoErro") && e.Metadata["TipoErro"] as string == "NotFound"))
                return NotFound();

            return BadRequest(result.Errors.Select(e => e.Message));
        }

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