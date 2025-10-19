using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controller;

[ApiController]
[Route("api/[controller]")]

public class Motivo_movimentacaoController : ControllerBase
{
    private readonly IMotivo_movimentacaoService _motivo_movimentacaoService;

    // CONSTRUTOR + INJEÇÃO DE DEPENDÊNCIA
    public Motivo_movimentacaoController(IMotivo_movimentacaoService motivo_movimentacaoService)
    {
        _motivo_movimentacaoService = motivo_movimentacaoService;
    }

    // OPERAÇÕES
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Motivo_movimentacao>>> ObterMotivo_movimentacao()
    {
        var motivo_movimentacao = await _motivo_movimentacaoService.ObterTodosMotivo_movimentacaoAsync();
        return Ok(motivo_movimentacao);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Motivo_movimentacao>> ObterMotivo_movimentacao(int id)
    {
        var motivo_movimentacao = await _motivo_movimentacaoService.ObterMotivo_movimentacaoPorIdAsync(id);

        if (motivo_movimentacao == null) return NotFound();
        return Ok(motivo_movimentacao);
    }

    // get produtos associados a motivo_movimentacao (aqui [/motivo_movimentacao/idmotivo_movimentacao/produtos] ou em produtos [/produtos?motivo_movimentacao=x])?

    [HttpPost]
    public async Task<ActionResult<Motivo_movimentacao>> CriarMotivo_movimentacao(Motivo_movimentacao motivo_movimentacao)
    {
        var motivo_movimentacaoCriada = await _motivo_movimentacaoService.CriarMotivo_movimentacaoAsync(motivo_movimentacao);
        return CreatedAtAction(nameof(ObterMotivo_movimentacao), new { id = motivo_movimentacaoCriada.Id },
            motivo_movimentacaoCriada);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarMotivo_movimentacao(int id, Motivo_movimentacao motivo_movimentacao)
    {
        if (id != motivo_movimentacao.Id) return BadRequest();
        await _motivo_movimentacaoService.AtualizarMotivo_movimentacaoAsync(id, motivo_movimentacao);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarMotivo_movimentacao(int id) 
    { 
        await _motivo_movimentacaoService.DeletarMotivo_movimentacaoAsync(id); 
        return NoContent(); 
    } 
}