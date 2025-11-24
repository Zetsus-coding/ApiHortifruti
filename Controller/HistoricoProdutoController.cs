using System.ComponentModel.DataAnnotations;
using ApiHortifruti.DTO.HistoricoProduto;
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
    public async Task<ActionResult<IEnumerable<GetHistoricoProdutoDTO>>> ObterTodosOsHistoricosProdutos()
    {
        var historicoProduto = await _historicoProdutoService.ObterTodosOsHistoricosProdutosAsync();
        return Ok(historicoProduto);
    }

    // [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetHistoricoProdutoDTO>> ObterHistoricoProdutoPorId([Range(1, int.MaxValue)] int id)
    {
        var getIdHistoricoProduto = await _historicoProdutoService.ObterHistoricoProdutoPorIdAsync(id);

        if (getIdHistoricoProduto == null) return NotFound();
        return Ok(getIdHistoricoProduto);
    }

    // Consulta de histórico de produto por ID do produto
    // [Authorize(Roles = "id")]
    [HttpGet("produto/{produtoId}")]
    public async Task<ActionResult<IEnumerable<GetHistoricoProdutoDTO>>> ObterHistoricoProdutoPorProdutoId([Range(1, int.MaxValue)] int produtoId)
    {
        var historicoProduto = await _historicoProdutoService.ObterHistoricoProdutoPorProdutoIdAsync(produtoId);

        if (historicoProduto == null || !historicoProduto.Any()) return NotFound();
        return Ok(historicoProduto);
    }
}
