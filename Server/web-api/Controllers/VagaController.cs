using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloVaga;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeEstacionamento.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/vagas")]
public class VagaController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CriarVagaResponse>> Criar(CriarVagaRequest request)
    {
        var command = mapper.Map<CriarVagaCommand>(request);
        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Created(string.Empty, mapper.Map<CriarVagaResponse>(result.Value));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EditarVagaResponse>> Editar(Guid id, EditarVagaRequest request)
    {
        var command = new EditarVagaCommand(id, request.Identificador, request.Zona, request.Ocupada, null);
        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(mapper.Map<EditarVagaResponse>(result.Value));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Excluir(Guid id)
    {
        var command = new ExcluirVagaCommand(id);
        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<ObterTodasVagasResponse>> ObterTodas()
    {
        var query = new ObterTodasVagasQuery();
        var result = await mediator.Send(query);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(mapper.Map<ObterTodasVagasResponse>(result.Value));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ObterVagaPorIdResponse>> ObterPorId(Guid id)
    {
        var query = new ObterVagaPorIdQuery(id);
        var result = await mediator.Send(query);

        if (result.IsFailed)
            return NotFound();

        return Ok(mapper.Map<ObterVagaPorIdResponse>(result.Value));
    }

    [HttpGet("livres")]
    public async Task<ActionResult<ObterTodasVagasResponse>> ObterLivres()
    {
        var query = new ObterVagasLivresQuery();
        var result = await mediator.Send(query);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(mapper.Map<ObterTodasVagasResponse>(result.Value));
    }

    [HttpGet("ocupadas")]
    public async Task<ActionResult<ObterTodasVagasResponse>> ObterOcupadas()
    {
        var query = new ObterVagasOcupadasQuery();
        var result = await mediator.Send(query);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(mapper.Map<ObterTodasVagasResponse>(result.Value));
    }

    [HttpPost("{id:guid}/ocupar")]
    public async Task<ActionResult<OcuparVagaResponse>> Ocupar(Guid id, OcuparVagaRequest request)
    {
        var command = new OcuparVagaCommand(id, request.VeiculoId);
        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(mapper.Map<OcuparVagaResponse>(result.Value));
    }

    [HttpPost("{id:guid}/liberar")]
    public async Task<ActionResult<LiberarVagaResponse>> Liberar(Guid id)
    {
        var command = new LiberarVagaCommand(id);
        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(mapper.Map<LiberarVagaResponse>(result.Value));
    }
}