using Hortifruti.Domain;
using Hortifruti.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hortifruti.Controllers;

[ApiController]
[Route("api/[controller]")]

public class Historico_produtoController : ControllerBase
{
    private readonly IHistorico_produtoService _historico_produtoService;

    public Historico_produtoController(IHistorico_produtoService historico_produtoService)
    {
        _historico_produtoService = historico_produtoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Historico_produto>>> ObterHistorico_produtos()
    {
        var historico_produto = await _historico_produtoService.ObterTodasHistorico_produtosAsync();
        return Ok(historico_produto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Historico_produto>> ObterHistorico_produto(int id)
    {
        var historico_produto = await _historico_produtoService.ObterHistorico_produtoPorIdAsync(id);

        if (historico_produto == null) return NotFound();
        return Ok(historico_produto);
    }

    [HttpPost]
    public async Task<ActionResult<Historico_produto>> CriarHistorico_produto(Historico_produto historico_produto)
    {
        var produtoCriada = await _historico_produtoService.CriarHistorico_produtoAsync(historico_produto);
        return CreatedAtAction(nameof(ObterHistorico_produto), new { id = produtoCriada.Id },
            produtoCriada);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarHistorico_produto(int id, Historico_produto historico_produto)
    {
        if (id != historico_produto.Id) return BadRequest();
        await _historico_produtoService.AtualizarHistorico_produtoAsync(id, historico_produto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarHistorico_produto(int id) 
    { 
        await _historico_produtoService.DeletarHistorico_produtoAsync(id); 
        return NoContent(); 
    } 
}