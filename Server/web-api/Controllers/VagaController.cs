using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Command;
using GestaoDeEstacionamento.WebApi.Models.ModuloGestaoDeVagas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeEstacionamento.WebApi.Controllers;

[ApiController]
[Route("vagas")]
public class VagaController(IMediator mediator, IMapper mapper) : Controller
{
    [HttpPost]
    public async Task<ActionResult<CadastrarVagaResult>> Cadastrar(CadastrarVagaRequest request)
    {
        var command = mapper.Map<CadastrarVagaCommand>(request);
        var result = await mediator.Send(command);

        if (result.IsFailed) return BadRequest();

        var response = mapper.Map<CadastrarVagaResponse>(result.Value);

        return Created(string.Empty, response);
    }

    public async Task<ActionResult<EditarVagaResponse>> Editar(Guid id, EditarVagaRequest request)
    {
        var command = mapper.Map<(Guid, EditarVagaRequest), EditarVagaCommand>((id, request));

        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest();

        var response = mapper.Map<EditarVagaResponse>(result.Value);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarVagasResponse>> SelecionarRegistros(
    [FromQuery] SelecionarVagasRequest? request)
    {
        var query = mapper.Map<SelecionarVagasQuery>(request);

        var result = await mediator.Send(query);

        if (result.IsFailed)
            return BadRequest();

        var response = mapper.Map<SelecionarVagasResponse>(result.Value);

        return Ok(response);
    }
}
