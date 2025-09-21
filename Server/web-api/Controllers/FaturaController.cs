using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloFatura;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeEstacionamento.WebApi.Controllers;

[Route("api/faturas")]
[ApiController]
[Authorize]
public class FaturaController : ControllerBase
{
    private readonly IMediator _mediator;

    public FaturaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ObterFaturaResponse>> ObterPorId(Guid id)
    {
        var query = new ObterFaturaPorIdQuery(id);
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            return NotFound(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("ticket/{numeroTicket}")]
    public async Task<ActionResult<ObterFaturaResponse>> ObterPorTicket(string numeroTicket)
    {
        var query = new ObterFaturaPorTicketQuery(numeroTicket);
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            return NotFound(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("veiculo/{placa}")]
    public async Task<ActionResult<ObterFaturaResponse>> ObterPorPlaca(string placa)
    {
        var query = new ObterFaturaPorPlacaQuery(placa);
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            return NotFound(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("relatorio")]
    public async Task<ActionResult<ObterRelatorioFaturamentoResponse>> ObterRelatorio(
        [FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        var query = new ObterRelatorioFaturamentoQuery(inicio, fim);
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("nao-pagas")]
    public async Task<ActionResult<ObterFaturasResponse>> ObterNaoPagas()
    {
        var query = new ObterFaturasNaoPagasQuery();
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/pagar")]
    public async Task<ActionResult<MarcarFaturaComoPagaResponse>> MarcarComoPaga(Guid id)
    {
        var command = new MarcarFaturaComoPagaCommand(id);
        var result = await _mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}