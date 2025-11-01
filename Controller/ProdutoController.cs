using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;
    private readonly IMapper _mapper;

    public ProdutoController(IProdutoService produtoService, IMapper mapper)
    {
        _produtoService = produtoService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> ObterProduto()
    {
        var produto = await _produtoService.ObterTodasProdutoAsync();
        return Ok(produto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> ObterProduto(int id)
    {
        var produto = await _produtoService.ObterProdutoPorIdAsync(id);

        if (produto == null) return NotFound();
        return Ok(produto);
    }

    [HttpPost]
    public async Task<ActionResult<Produto>> CriarProduto(PostProdutoDTO postProdutoDTO)
    {
        var produto = _mapper.Map<Produto>(postProdutoDTO); // Conversão de DTO para entidade

        var produtoCriada = await _produtoService.CriarProdutoAsync(produto);
        return CreatedAtAction(nameof(ObterProduto), new { id = produtoCriada.Id },
            produtoCriada);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarProduto(int id, Produto produto)
    {
        if (id != produto.Id) return BadRequest();
        await _produtoService.AtualizarProdutoAsync(id, produto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarProduto(int id) 
    { 
        await _produtoService.DeletarProdutoAsync(id); 
        return NoContent(); 
    } 
}