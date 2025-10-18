using Hortifruti.Data.Repository;
using Hortifruti.Domain;
using Hortifruti.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hortifruti.Controller;

[ApiController]
[Route("api/[controller]")]

public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    // CONSTRUTOR + INJEÇÃO DE DEPENDÊNCIA
    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    // OPERAÇÕES
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> ObterCategorias()
    {
        var categoria = await _categoriaService.ObterTodasCategoriasAsync();
        return Ok(categoria);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> ObterCategoria(int id)
    {
        var categoria = await _categoriaService.ObterCategoriaPorIdAsync(id);

        if (categoria == null) return NotFound();
        return Ok(categoria);
    }

    // get produtos associados a categoria (aqui [/categoria/idcategoria/produtos] ou em produtos [/produtos?categoria=x])?

    [HttpPost]
    public async Task<ActionResult<Categoria>> CriarCategoria(Categoria categoria)
    {
        var categoriaCriada = await _categoriaService.CriarCategoriaAsync(categoria);
        return CreatedAtAction(nameof(ObterCategoria), new { id = categoriaCriada.Id },
            categoriaCriada);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarCategoria(int id, Categoria categoria)
    {
        if (id != categoria.Id) return BadRequest();
        await _categoriaService.AtualizarCategoriaAsync(id, categoria);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarCategoria(int id) 
    { 
        await _categoriaService.DeletarCategoriaAsync(id); 
        return NoContent(); 
    } 
}