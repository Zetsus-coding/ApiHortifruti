using System.ComponentModel.DataAnnotations;
using ApiHortifruti.DTO.FornecedorProduto;
using ApiHortifruti.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controllers;

[ApiController]
[Route("api/[controller]")]

public class FornecedorProdutoController : ControllerBase
{
    private readonly IFornecedorProdutoService _fornecedorProdutoService;

    // Construtor com injeção de dependência do serviço
    public FornecedorProdutoController(IFornecedorProdutoService fornecedorProdutoService)
    {
        _fornecedorProdutoService = fornecedorProdutoService;
    }

    // Operação de consulta de todos os registro da tabela
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetFornecedorProdutoDTO>>> ObterTodosOsFornecedorProduto()
    {
        var fornecedorProduto = await _fornecedorProdutoService.ObterTodosOsFornecedorProdutoAsync();
        return Ok(fornecedorProduto);
    }

    // Operação de consulta por ID
    // [Authorize(Roles = "get(id)")]
    [HttpGet("{fornecedorId}/{produtoId}")]
    public async Task<ActionResult<GetFornecedorProdutoDTO>> ObterFornecedorProdutoPorId([Range(1, int.MaxValue)] int fornecedorId, [Range(1, int.MaxValue)] int produtoId)
    {
        var getIdFornecedorProduto = await _fornecedorProdutoService.ObterFornecedorProdutoPorIdAsync(fornecedorId, produtoId);

        if (getIdFornecedorProduto == null) return NotFound();
        return Ok(getIdFornecedorProduto);
    }

    // Operação de criação do registro na tabela
    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<GetFornecedorProdutoDTO>> CriarFornecedorProduto(PostFornecedorProdutoDTO fornecedorProdutoDTO)
    {
        var fornecedorProdutoCriada = await _fornecedorProdutoService.CriarFornecedorProdutoAsync(fornecedorProdutoDTO);

        return CreatedAtAction(nameof(ObterFornecedorProdutoPorId), new { fornecedorId = fornecedorProdutoCriada.FornecedorId, produtoId = fornecedorProdutoCriada.ProdutoId },
            fornecedorProdutoCriada);
    }

    // Operação de criação de vários registros na tabela
    // [Authorize(Roles = "post(varios)")]
    [HttpPost("batch")]
    public async Task<IEnumerable<GetFornecedorProdutoDTO>> CriarVariosFornecedorProduto(IEnumerable<PostFornecedorProdutoDTO> fornecedorProdutoDTOs)
    {
        var fornecedorProdutosCriadas = await _fornecedorProdutoService.CriarVariosFornecedorProdutosAsync(fornecedorProdutoDTOs);
        return fornecedorProdutosCriadas;
    }

    // Operação de alteração de algum registro na tabela
    // [Authorize(Roles = "put")]
    [HttpPut("{fornecedorId}/{produtoId}")]
    public async Task<IActionResult> AtualizarFornecedorProduto([Range(1, int.MaxValue)] int fornecedorId, [Range(1, int.MaxValue)] int produtoId, PutFornecedorProdutoDTO fornecedorProduto)
    {
        if (fornecedorId != fornecedorProduto.FornecedorId || produtoId != fornecedorProduto.ProdutoId) return BadRequest();

        await _fornecedorProdutoService.AtualizarFornecedorProdutoAsync(fornecedorId, produtoId, fornecedorProduto);
        return NoContent();
    }

    // Operação de exclusão de algum registro na tabela
    // [Authorize(Roles = "delete")]
    [HttpDelete("{fornecedorId}/{produtoId}")]
    public async Task<IActionResult> DeletarFornecedorProduto([Range(1, int.MaxValue)] int fornecedorId, [Range(1, int.MaxValue)] int produtoId)
    {
        await _fornecedorProdutoService.DeletarFornecedorProdutoAsync(fornecedorId, produtoId);
        return NoContent();
    }
}
