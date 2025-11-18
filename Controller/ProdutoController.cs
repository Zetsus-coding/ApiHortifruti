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
    private readonly IMapper _mapper;

    // Construtor com injeção de dependência do serviço e do mapper
    public ProdutoController(IProdutoService produtoService, IMapper mapper)
    {
        _produtoService = produtoService;
        _mapper = mapper;
    }

    // Consulta de todos os produtos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> ObterTodosOsProdutos()
    {
        var produto = await _produtoService.ObterTodosProdutoAsync(); // Chamada a camada de serviço para obter todos
        return Ok(produto);
    }

    // Consulta de produtos com estoque atual menor ou igual à quantidade mínima
    [HttpGet("estoque-critico")]
    public async Task<ActionResult<IEnumerable<GetProdutoEstoqueCriticoDTO>>> ObterProdutosComEstoqueCritico()
    {
        var produtos = await _produtoService.ObterProdutosEstoqueCriticoAsync();
        var produtosDTO = _mapper.Map<IEnumerable<GetProdutoEstoqueCriticoDTO>>(produtos);
        return Ok(produtosDTO);
    }

    // Consulta de um produto por ID
    // [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> ObterProdutoPorId(int id)
    {
        var produto = await _produtoService.ObterProdutoPorIdAsync(id); // Chamada a camada de serviço para obter por ID
        return Ok(produto);
    }

    // Consulta de um produto por código (ex.: código de barras)
    [HttpGet("codigo/{codigo}")]
    public async Task<ActionResult<Produto>> ObterProdutoPorCodigo(string codigo)
    {
        var produto = await _produtoService.ObterProdutoPorCodigoAsync(codigo); // Chamada a camada de serviço para obter por código de barras
        return Ok(produto);
    }

    // Criação de um novo produto
    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<Produto>> CriarProduto(PostProdutoDTO postProdutoDTO)
    {
        var produto = _mapper.Map<Produto>(postProdutoDTO); // Conversão de DTO para entidade

        var produtoCriada = await _produtoService.CriarProdutoAsync(produto); // Chamada a camada de serviço para criar
        return CreatedAtAction(nameof(ObterProduto), new { id = produtoCriada.Id },
            produtoCriada);
    }

    // Atualização de um produto existente
    // [Authorize(Roles = "put")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarProduto([Range(1, int.MaxValue)] int id, PutProdutoDTO putProdutoDTO)
    {
        var produto = _mapper.Map<Produto>(putProdutoDTO);
        await _produtoService.AtualizarProdutoAsync(id, produto); // Chamada a camada de serviço para atualizar
        return NoContent();
    }

    // Exclusão de um produto existente
    //// [Authorize(Roles = "delete")]
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeletarProduto([Range(1, int.MaxValue)]int id) 
    // { 
    //     await _produtoService.DeletarProdutoAsync(id); // Chamada a camada de serviço para deletar
    //     return NoContent(); 
    // } 
}