using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Data.Repository;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controllers;

[ApiController]
[Route("api/[controller]")]

public class FornecedorProdutoController : ControllerBase
{
    private readonly IFornecedorProdutoService _fornecedorProdutoService;
    private readonly IMapper _mapper;

    public FornecedorProdutoController(IFornecedorProdutoService fornecedorProdutoService, IMapper mapper)
    {
        _fornecedorProdutoService = fornecedorProdutoService;
        _mapper = mapper;
    }

    // Operação de consulta de todos os registro da tabela
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FornecedorProduto>>> ObterTodosFornecedorProduto()
    {
        var fornecedorProduto = await _fornecedorProdutoService.ObterTodosFornecedorProdutosAsync();
        return Ok(fornecedorProduto);
    }

    // Operação de consulta por ID
    // [Authorize(Roles = "get(id)")]
    [HttpGet("{fornecedorId}/{produtoId}")]
    public async Task<ActionResult<FornecedorProduto>> ObterFornecedorProduto([Range(1, int.MaxValue)] int fornecedorId, [Range(1, int.MaxValue)] int produtoId)
    {
        var getIdFornecedorProduto = await _fornecedorProdutoService.ObterFornecedorProdutoPorIdAsync(fornecedorId, produtoId);

        if (getIdFornecedorProduto == null) return NotFound();
        return Ok(getIdFornecedorProduto);
    }

    // Operação de criação do registro na tabela
    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<FornecedorProduto>> CriarFornecedorProduto(PostFornecedorProdutoDTO fornecedorProdutoDTO)
    {
        var fornecedorProduto = _mapper.Map<FornecedorProduto>(fornecedorProdutoDTO);

        var fornecedorProdutoCriada = await _fornecedorProdutoService.CriarFornecedorProdutoAsync(fornecedorProduto);

        return CreatedAtAction(nameof(ObterFornecedorProduto), new { fornecedorId = fornecedorProduto.FornecedorId, produtoId = fornecedorProduto.ProdutoId },
            fornecedorProdutoCriada);
    }

    // Operação de criação de vários registros na tabela
    // [Authorize(Roles = "post(varios)")]
    [HttpPost("batch")]
    public async Task<IActionResult> CriarVariosFornecedorProduto(List<PostFornecedorProdutoDTO> fornecedorProdutoDTOs)
    {
        var fornecedorProdutos = _mapper.Map<List<FornecedorProduto>>(fornecedorProdutoDTOs);

        await _fornecedorProdutoService.CriarVariosFornecedorProdutosAsync(fornecedorProdutos);

        return NoContent();
    }

    // Operação de alteração de algum registro na tabela
    // [Authorize(Roles = "put")]
    [HttpPut("{fornecedorId}/{produtoId}")]
    public async Task<IActionResult> AtualizarFornecedorProduto([Range(1, int.MaxValue)] int fornecedorId, [Range(1, int.MaxValue)] int produtoId, FornecedorProduto fornecedorProduto)
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