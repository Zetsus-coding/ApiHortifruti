using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UnidadeMedidaController : ControllerBase
{
    private readonly IUnidadeMedidaService _unidadeMedidaService;

    // Construtor com injeção de dependência do serviço e do mapper
    public UnidadeMedidaController(IUnidadeMedidaService unidadeMedidaService)
    {
        _unidadeMedidaService = unidadeMedidaService;
    }

    // Consulta todas as unidades de medida
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetUnidadeMedidaDTO>>> ObterTodasAsUnidadesMedida()
    {
        var unidadeMedida = await _unidadeMedidaService.ObterTodasAsUnidadesMedidaAsync(); // Chamada a camada de serviço para obter todos
        return Ok(unidadeMedida);
    }

    // Consulta uma unidade de medida pelo ID
    // [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetUnidadeMedidaDTO>> ObterUnidadeMedidaPorId([Range(1, int.MaxValue)] int id)
    {
        var getIdUnidadeMedida = await _unidadeMedidaService.ObterUnidadeMedidaPorIdAsync(id); // Chamada a camada de serviço para obter por ID
        return Ok(getIdUnidadeMedida);
    }

    // Cria uma nova unidade de medida
    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<GetUnidadeMedidaDTO>> CriarUnidadeMedida(PostUnidadeMedidaDTO postUnidadeMedidaDTO)
    {
        var unidadeMedidaCriada = await _unidadeMedidaService.CriarUnidadeMedidaAsync(postUnidadeMedidaDTO); // Chamada a camada de serviço para criar
        return CreatedAtAction(nameof(ObterTodasAsUnidadesMedida), new { unidadeMedidaCriada.Id },
            unidadeMedidaCriada);
    }

    // Atualiza uma unidade de medida existente
    // [Authorize(Roles = "put")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarUnidadeMedida([Range(1, int.MaxValue)] int id, PutUnidadeMedidaDTO putUnidadeMedidaDTO)
    {
        await _unidadeMedidaService.AtualizarUnidadeMedidaAsync(id, putUnidadeMedidaDTO); // Chamada a camada de serviço para atualizar
        return NoContent();
    }
    
    // Exclusão de uma unidade de medida existente
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarUnidadeMedida([Range(1, int.MaxValue)]int id)
    {
        await _unidadeMedidaService.DeletarUnidadeMedidaAsync(id);
        return NoContent();
    }
}