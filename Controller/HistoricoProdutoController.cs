using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controllers;

[ApiController]
[Route("api/[controller]")]

public class HistoricoProdutoController : ControllerBase
{
    private readonly IHistoricoProdutoService _historicoProdutoService;

    public HistoricoProdutoController(IHistoricoProdutoService historicoProdutoService)
    {
        _historicoProdutoService = historicoProdutoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HistoricoProduto>>> ObterTodosOsHistoricosProdutos()
    {
        var historicoProduto = await _historicoProdutoService.ObterTodosHistoricoProdutosAsync();
        return Ok(historicoProduto);
    }

    // [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<HistoricoProduto>> ObterHistoricoProdutoPorId([Range(1, int.MaxValue)] int id) // Faz sentido? Ou deveria ser por produto?
    {
        var getIdHistoricoProduto = await _historicoProdutoService.ObterHistoricoProdutoPorIdAsync(id);

        if (getIdHistoricoProduto == null) return NotFound();
        return Ok(getIdHistoricoProduto);
    }

    // Consulta de histórico de produto por ID do produto
    // [Authorize(Roles = "id")]
    [HttpGet("produto/{produtoId}")]
    public async Task<ActionResult<IEnumerable<HistoricoProduto>>> ObterHistoricoProdutoPorProdutoId([Range(1, int.MaxValue)] int produtoId)
    {
        var historicoProduto = await _historicoProdutoService.ObterHistoricoProdutoPorProdutoIdAsync(produtoId); // MARCADO

        if (historicoProduto == null || !historicoProduto.Any()) return NotFound();
        return Ok(historicoProduto);
    }

    // [HttpPost]
    // public async Task<ActionResult<HistoricoProduto>> CriarHistoricoProduto(HistoricoProduto historicoProduto)
    // {
    //     var produtoCriada = await _historicoProdutoService.CriarHistoricoProdutoAsync(historicoProduto);
    //     return CreatedAtAction(nameof(ObterHistoricoProduto), new { id = produtoCriada.Id },
    //         produtoCriada);
    // }

    // [HttpPut("{id}")]
    // public async Task<IActionResult> AtualizarHistoricoProduto([Range(1, int.MaxValue)] int id, HistoricoProduto historicoProduto)
    // {
    //     if (id != historicoProduto.Id) return BadRequest();
    //     await _historicoProdutoService.AtualizarHistoricoProdutoAsync(id, historicoProduto);
    //     return NoContent();
    // }

    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeletarHistoricoProduto([Range(1, int.MaxValue)] int id) 
    // { 
    //     await _historicoProdutoService.DeletarHistoricoProdutoAsync(id); 
    //     return NoContent(); 
    // } 
}