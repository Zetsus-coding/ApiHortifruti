using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controller;

[ApiController]
[Route("api/[controller]")]

public class MotivoMovimentacaoController : ControllerBase
{
    private readonly IMotivoMovimentacaoService _motivoMovimentacaoService;

    // CONSTRUTOR + INJEÇÃO DE DEPENDÊNCIA
    public MotivoMovimentacaoController(IMotivoMovimentacaoService motivoMovimentacaoService)
    {
        _motivoMovimentacaoService = motivoMovimentacaoService;
    }

    // OPERAÇÕES
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MotivoMovimentacao>>> ObterTodosMotivoMovimentacao()
    {
        var getAllMotivoMovimentacao = await _motivoMovimentacaoService.ObterTodosMotivoMovimentacaoAsync();
        return Ok(getAllMotivoMovimentacao);
    }

    [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<MotivoMovimentacao>> ObterMotivoMovimentacao([Range(1, int.MaxValue)] int id)
    {
        var getIdMotivoMovimentacao = await _motivoMovimentacaoService.ObterMotivoMovimentacaoPorIdAsync(id);

        if (getIdMotivoMovimentacao == null) return NotFound();
        return Ok(getIdMotivoMovimentacao);
    }

    // get produtos associados a motivoMovimentacao (aqui [/motivoMovimentacao/idmotivoMovimentacao/produtos] ou em produtos [/produtos?motivoMovimentacao=x])?

    [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<MotivoMovimentacao>> CriarMotivoMovimentacao(MotivoMovimentacao motivoMovimentacao)
    {
        var motivoMovimentacaoCriada = await _motivoMovimentacaoService.CriarMotivoMovimentacaoAsync(motivoMovimentacao);
        return CreatedAtAction(nameof(ObterMotivoMovimentacao), new { id = motivoMovimentacaoCriada.Id },
            motivoMovimentacaoCriada);
    }

    [Authorize(Roles = "put")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarMotivoMovimentacao([Range(1, int.MaxValue)] int id, MotivoMovimentacao motivoMovimentacao)
    {
        if (id != motivoMovimentacao.Id) return BadRequest();
        await _motivoMovimentacaoService.AtualizarMotivoMovimentacaoAsync(id, motivoMovimentacao);
        return NoContent();
    }

    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeletarMotivoMovimentacao([Range(1, int.MaxValue)]int id) 
    // { 
    //     await _motivoMovimentacaoService.DeletarMotivoMovimentacaoAsync(id); 
    //     return NoContent(); 
    // } 
}