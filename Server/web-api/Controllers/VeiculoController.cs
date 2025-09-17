using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeEstacionamento.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("veiculos")]
public class VeiculoController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CadastrarVeiculoResponse>> Cadastrar(CadastrarVeiculoRequest request)
    {
        try
        {
            var command = mapper.Map<CadastrarVeiculoCommand>(request);
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

            var response = mapper.Map<CadastrarVeiculoResponse>(result.Value);

            return Created(string.Empty, response);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, statusCode: 500);
        }
    }

    [HttpPut("{ticket:guid}")]
    public async Task<ActionResult<EditarVeiculoResponse>> Editar(Guid ticket, EditarVeiculoRequest request)
    {
        var command = mapper.Map<(Guid, EditarVeiculoRequest), EditarVeiculoCommand>((ticket, request));

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

        var response = mapper.Map<EditarVeiculoResponse>(result.Value);

        return Ok(response);
    }

    [HttpDelete("{ticket:guid}")]
    public async Task<ActionResult> Excluir(Guid ticket)
    {
        var command = mapper.Map<ExcluirVeiculoCommand>(ticket);

        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest();

        return NoContent();
    }
    [HttpGet]
    public async Task<ActionResult<SelecionarVeiculosResponse>> SelecionarTodos([FromQuery] SelecionarVeiculosRequest request, CancellationToken cancellationToken)
    {
        var query = mapper.Map<SelecionarVeiculosQuery>(request);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailed)
            return BadRequest();

        var response = mapper.Map<SelecionarVeiculosResponse>(result.Value);

        return Ok(response);
    }

    [HttpGet("{ticket:guid}")]
    public async Task<ActionResult<SelecionarVeiculoPorTicketResult>> SelecionarPorId(Guid ticket)
    {
        var query = new SelecionarVeiculoPorTicketQuery(ticket);

        var result = await mediator.Send(query);

        if (result.IsFailed)
            return NotFound(ticket);

        return Ok(result.Value);
    }

}
