using System.ComponentModel.DataAnnotations;
using System.Data;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UnidadeMedidaController : ControllerBase
{
    private readonly IUnidadeMedidaService _unidadeMedidaService;
    private readonly IMapper _mapper;

    // Construtor com injeção de dependência do serviço e do mapper
    public UnidadeMedidaController(IUnidadeMedidaService unidadeMedidaService, IMapper mapper)
    {
        _unidadeMedidaService = unidadeMedidaService;
        _mapper = mapper;
    }

    // Consulta todas as unidades de medida
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UnidadeMedida>>> ObterTodosUnidadeMedida()
    {
        var getAllUnidadeMedida = await _unidadeMedidaService.ObterTodosUnidadeMedidaAsync(); // Chamada a camada de serviço para obter todos
        return Ok(getAllUnidadeMedida);
    }

    // Consulta uma unidade de medida pelo ID
    [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UnidadeMedida>> ObterUnidadeMedida([Range(1, int.MaxValue)] int id)
    {
        var getIdUnidadeMedida = await _unidadeMedidaService.ObterUnidadeMedidaPorIdAsync(id); // Chamada a camada de serviço para obter por ID
        return Ok(getIdUnidadeMedida);
    }

    // Cria uma nova unidade de medida
    [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<UnidadeMedida>> CriarUnidadeMedida(PostUnidadeMedidaDTO postUnidadeMedidaDTO)
    {
        var unidadeMedida = _mapper.Map<UnidadeMedida>(postUnidadeMedidaDTO); // Conversão de DTO para entidade

        var unidadeMedidaCriada = await _unidadeMedidaService.CriarUnidadeMedidaAsync(unidadeMedida); // Chamada a camada de serviço para criar
        return CreatedAtAction(nameof(ObterUnidadeMedida), new { unidadeMedidaCriada.Id },
            unidadeMedidaCriada);
    }

    // Atualiza uma unidade de medida existente
    [Authorize(Roles = "put")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarUnidadeMedida([Range(1, int.MaxValue)] int id, UnidadeMedida unidadeMedida)
    {
        await _unidadeMedidaService.AtualizarUnidadeMedidaAsync(id, unidadeMedida); // Chamada a camada de serviço para atualizar
        return NoContent();
    }
    
    // Exclusão de uma unidade de medida existente
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeletarUnidadeMedida([Range(1, int.MaxValue)]int id) 
    // { 
    //     await _unidadeMedidaService.DeletarUnidadeMedidaAsync(id); 
    //     return NoContent(); 
    // }
}