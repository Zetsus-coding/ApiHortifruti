using System.ComponentModel.DataAnnotations;
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
    public async Task<ActionResult<IEnumerable<GetMotivoMovimentacaoDTO>>> ObterTodosOsMotivosMovimentacao()
    {
        var motivoMovimentacao = await _motivoMovimentacaoService.ObterTodosOsMotivosMovimentacaoAsync();
        return Ok(motivoMovimentacao);
    }

    // [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetMotivoMovimentacaoDTO>> ObterMotivoMovimentacaoPorId([Range(1, int.MaxValue)] int id)
    {
        var getIdMotivoMovimentacao = await _motivoMovimentacaoService.ObterMotivoMovimentacaoPorIdAsync(id);

        if (getIdMotivoMovimentacao == null) return NotFound();
        return Ok(getIdMotivoMovimentacao);
    }

    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<GetMotivoMovimentacaoDTO>> CriarMotivoMovimentacao(PostMotivoMovimentacaoDTO postMotivoMovimentacaoDTO)
    {
        var motivoMovimentacaoCriada = await _motivoMovimentacaoService.CriarMotivoMovimentacaoAsync(postMotivoMovimentacaoDTO);
        return CreatedAtAction(nameof(ObterMotivoMovimentacaoPorId), new { id = motivoMovimentacaoCriada.Id },
            motivoMovimentacaoCriada);
    }

    // [Authorize(Roles = "put")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarMotivoMovimentacao([Range(1, int.MaxValue)] int id, PutMotivoMovimentacaoDTO dto)
    {
        dto.Id = id;
        await _motivoMovimentacaoService.AtualizarMotivoMovimentacaoAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarMotivoMovimentacao([Range(1, int.MaxValue)]int id) 
    { 
        await _motivoMovimentacaoService.DeletarMotivoMovimentacaoAsync(id); 
        return NoContent(); 
    } 
}
