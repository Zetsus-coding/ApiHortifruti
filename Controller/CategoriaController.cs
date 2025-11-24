using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controller;

[ApiController]
[Route("api/[controller]")]

public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    // Construtor com injeção de dependência do serviço e do mapper
    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    // Consulta de todas as categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCategoriaDTO>>> ObterTodasAsCategorias()
    {
        var categoria = await _categoriaService.ObterTodasAsCategoriasAsync(); // Chamada a camada de serviço para obter todos
        return Ok(categoria);
    }

    // Consulta de categoria por id
    // [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCategoriaDTO>> ObterCategoriaPorId([Range(1, int.MaxValue)] int id)
    {
        var categoria = await _categoriaService.ObterCategoriaPorIdAsync(id); // Chamada a camada de serviço para obter por ID
        return Ok(categoria);
    }

    // Criação de categoria
    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<GetCategoriaDTO>> CriarCategoria(PostCategoriaDTO postCategoriaDTO)
    {
        var categoriaCriada = await _categoriaService.CriarCategoriaAsync(postCategoriaDTO); // Chamada a camada de serviço para criar
        return CreatedAtAction(nameof(ObterCategoriaPorId), new { id = categoriaCriada.Id },
            categoriaCriada); // Retorna 201 com a nova categoria criada e sua localização (url)
    }

    // Atualização de uma categoria existente
    // [Authorize(Roles = "put")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarCategoria([Range(1, int.MaxValue)] int id, PutCategoriaDTO putCategoriaDTO)
    {   
        await _categoriaService.AtualizarCategoriaAsync(id, putCategoriaDTO); // Chamada a camada de serviço para atualizar
        return NoContent();
    }

    // Exclusão de uma categoria existente
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarCategoria([Range(1, int.MaxValue)] int id)
    { 
        await _categoriaService.DeletarCategoriaAsync(id); // Chamada a camada de serviço para deletar
        return NoContent(); 
    } 
}