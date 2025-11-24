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

public class FornecedorController : ControllerBase
{
    private readonly IFornecedorService _fornecedorService;

    // Construtor com injeção de dependência do serviço e do mapper
    public FornecedorController(IFornecedorService fornecedorService)
    {
        _fornecedorService = fornecedorService;
    }

    // Consulta de todos os fornecedores
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetFornecedorDTO>>> ObterTodosOsFornecedores()
    {
        var fornecedor = await _fornecedorService.ObterTodosOsFornecedoresAsync(); // Chamada a camada de serviço para obter todos
        return Ok(fornecedor); // Retorna a lista de fornecedores
    }

    // Consulta de um fornecedor por ID
    // [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetFornecedorDTO>> ObterFornecedorPorId([Range(1, int.MaxValue)] int id)
    {
        var fornecedor = await _fornecedorService.ObterFornecedorPorIdAsync(id); // Chamada a camada de serviço para obter por ID

        return Ok(fornecedor); // Retorna o DTO do fornecedor
    }

    // Consulta de um fornecedor específico com a lista de produtos que ele fornece
    [HttpGet("{id}/produtos")]
    public async Task<ActionResult<FornecedorComListaProdutosDTO>> ObterPorFornecedorIdSuaListaDeProdutos([Range(1, int.MaxValue)] int id)
    {
        var fornecedorComListaProdutosDTO = await _fornecedorService.ObterProdutosPorFornecedorIdAsync(id); // Chamada a camada de serviço para obter os produtos do fornecedor
        return Ok(fornecedorComListaProdutosDTO); // Retorna o DTO com a lista de produtos
    }

    // Criação de um novo fornecedor
    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<GetFornecedorDTO>> CriarFornecedor(PostFornecedorDTO postFornecedorDTO)
    {
        var fornecedorCriado = await _fornecedorService.CriarFornecedorAsync(postFornecedorDTO); // Chamada a camada de serviço para criar
        return CreatedAtAction(nameof(ObterFornecedorPorId), new { fornecedorCriado.Id },
            fornecedorCriado); // Retorna o status 201 com a localização do novo recurso
    }

    // Atualização de um fornecedor existente
    // [Authorize(Roles = "put")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarFornecedor([Range(1, int.MaxValue)] int id, PutFornecedorDTO putFornecedorDTO)
    {
        await _fornecedorService.AtualizarFornecedorAsync(id, putFornecedorDTO); // Chamada a camada de serviço para atualizar
        return NoContent();
    }
    

    // Exclusão de um fornecedor existente
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarFornecedor([Range(1, int.MaxValue)] int id)
    {
        await _fornecedorService.DeletarFornecedorAsync(id); // Chamada a camada de serviço para deletar
        return NoContent();
    }
}