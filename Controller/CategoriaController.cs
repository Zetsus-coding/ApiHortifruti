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

public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;
    private readonly IMapper _mapper;

    // Construtor com injeção de dependência do serviço e do mapper
    public CategoriaController(ICategoriaService categoriaService, IMapper mapper)
    {
        _categoriaService = categoriaService;
        _mapper = mapper;
    }


    // Consulta de todas as categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> ObterCategorias()
    {
        var categoria = await _categoriaService.ObterTodosCategoriasAsync(); // Chamada a camada de serviço para obter todos
        return Ok(categoria);
    }

    // Consulta de categoria por id
    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> ObterCategoria([Range(1, int.MaxValue)]int id)
    {
        var categoria = await _categoriaService.ObterCategoriaPorIdAsync(id); // Chamada a camada de serviço para obter por ID
        return Ok(categoria);
    }

    // get produtos associados a categoria (aqui [/categoria/idcategoria/produtos] ou em produtos [/produtos?categoria=x])?

    // Criação de categoria
    [HttpPost]
    public async Task<ActionResult<Categoria>> CriarCategoria(PostCategoriaDTO postCategoriaDTO)
    {
        var categoria = _mapper.Map<Categoria>(postCategoriaDTO); // Conversão de DTO para entidade

        var categoriaCriada = await _categoriaService.CriarCategoriaAsync(categoria); // Chamada a camada de serviço para criar
        return CreatedAtAction(nameof(ObterCategoria), new { id = categoriaCriada.Id },
            categoriaCriada);
    }

    // Atualização de uma categoria existente
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarCategoria([Range(1, int.MaxValue)] int id, Categoria categoria)
    {
        await _categoriaService.AtualizarCategoriaAsync(id, categoria); // Chamada a camada de serviço para atualizar
        return NoContent();
    }

    // Exclusão de uma categoria existente
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeletarCategoria(int id)
    // { 
    //     await _categoriaService.DeletarCategoriaAsync(id); // Chamada a camada de serviço para deletar
    //     return NoContent(); 
    // } 
}