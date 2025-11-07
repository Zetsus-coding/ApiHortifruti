using System.ComponentModel.DataAnnotations;
using System.Data;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;
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
    public async Task<ActionResult<IEnumerable<Produto>>> ObterProdutos()
    {
        var produto = await _produtoService.ObterTodosProdutoAsync(); // Chamada a camada de serviço para obter todos
        return Ok(produto);
    }

    // Consulta de um produto por ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> ObterProduto(int id)
    {
        var produto = await _produtoService.ObterProdutoPorIdAsync(id); // Chamada a camada de serviço para obter por ID
        return Ok(produto);
    }

    // Criação de um novo produto
    [HttpPost]
    public async Task<ActionResult<Produto>> CriarProduto(PostProdutoDTO postProdutoDTO)
    {
        var produto = _mapper.Map<Produto>(postProdutoDTO); // Conversão de DTO para entidade

        var produtoCriada = await _produtoService.CriarProdutoAsync(produto); // Chamada a camada de serviço para criar
        return CreatedAtAction(nameof(ObterProduto), new { id = produtoCriada.Id },
            produtoCriada);
    }

    // Atualização de um produto existente
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarProduto([Range(1, int.MaxValue)] int id, Produto produto)
    {
        await _produtoService.AtualizarProdutoAsync(id, produto); // Chamada a camada de serviço para atualizar
        return NoContent();
    }

    // Exclusão de um produto existente
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeletarProduto([Range(1, int.MaxValue)]int id) 
    // { 
    //     await _produtoService.DeletarProdutoAsync(id); // Chamada a camada de serviço para deletar
    //     return NoContent(); 
    // } 
}