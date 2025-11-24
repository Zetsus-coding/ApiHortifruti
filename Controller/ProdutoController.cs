using System.ComponentModel.DataAnnotations;
using System.Data;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controller;

[ApiController]
[Route("api/[controller]")]

public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    // Construtor com injeção de dependência do serviço e do mapper
    public ProdutoController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    // Consulta de todos os produtos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetProdutoDTO>>> ObterTodosOsProdutos()
    {
        var listaProdutoDTO = await _produtoService.ObterTodosOsProdutosAsync(); // Chamada a camada de serviço para obter todos
        return Ok(listaProdutoDTO);
    }

    // Consulta de produtos com estoque atual menor ou igual à quantidade mínima
    [HttpGet("estoque-critico")]
    public async Task<ActionResult<IEnumerable<GetProdutoEstoqueCriticoDTO>>> ObterProdutosComEstoqueCritico()
    {
        var produtosEstoqueCriticoDTO = await _produtoService.ObterProdutosEstoqueCriticoAsync();
        
        return Ok(produtosEstoqueCriticoDTO);
    }

    // Consulta de um produto por ID
    // [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetProdutoDTO>> ObterProdutoPorId(int id)
    {
        var produtoDTO = await _produtoService.ObterProdutoPorIdAsync(id); // Chamada a camada de serviço para obter por ID
        return Ok(produtoDTO);
    }

    // Consulta de um produto por código (ex.: código de barras)
    [HttpGet("codigo/{codigo}")]
    public async Task<ActionResult<GetProdutoDTO>> ObterProdutoPorCodigo(string codigo)
    {
        var produtoDTO = await _produtoService.ObterProdutoPorCodigoAsync(codigo); // Chamada a camada de serviço para obter por código de barras
        return Ok(produtoDTO);
    }

    // Operação de consulta de todos os fornecedores que fornecem um determinado produto
    // [Authorize(Roles = "get(id)")]
    [HttpGet("{produtoId}/fornecedores")]
    public async Task<ActionResult<ProdutoComListaDeFornecedoresDTO>> ObterFornecedoresPorProdutoIdAsync([Range(1, int.MaxValue)] int produtoId)
    {
        var produtoComListaFornecedores = await _produtoService.ObterListaDeFornecedoresQueFornecemCertoProduto(produtoId);
        return Ok(produtoComListaFornecedores);
    }

    // Criação de um novo produto
    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<GetProdutoDTO>> CriarProduto(PostProdutoDTO postProdutoDTO)
    {
        var produtoCriada = await _produtoService.CriarProdutoAsync(postProdutoDTO); // Chamada a camada de serviço para criar
        return CreatedAtAction(nameof(ObterProdutoPorId), new { id = produtoCriada.Id },
            produtoCriada);
    }

    // Atualização de um produto existente
    // [Authorize(Roles = "put")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarProduto([Range(1, int.MaxValue)] int id, PutProdutoDTO putProdutoDTO)
    {
        await _produtoService.AtualizarProdutoAsync(id, putProdutoDTO); // Chamada a camada de serviço para atualizar
        return NoContent();
    }

    // Exclusão de um produto existente
    // [Authorize(Roles = "delete")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarProduto([Range(1, int.MaxValue)]int id) 
    { 
        await _produtoService.DeletarProdutoAsync(id); // Chamada a camada de serviço para deletar
        return NoContent(); 
    } 
}