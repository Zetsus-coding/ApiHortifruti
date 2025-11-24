using System.ComponentModel.DataAnnotations;
using ApiHortifruti.DTO.Entrada;
using ApiHortifruti.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controller;

[ApiController]
[Route("api/[controller]")]

public class EntradaController : ControllerBase
{
    private readonly IEntradaService _entradaService;

    // CONSTRUTOR + INJEÇÃO DE DEPENDÊNCIA
    public EntradaController(IEntradaService entradaService)
    {
        _entradaService = entradaService;
    }

    // OPERAÇÕES
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetEntradaSimplesDTO>>> ObterTodasAsEntradas()
    {
        var entrada = await _entradaService.ObterTodasAsEntradasAsync();
        return Ok(entrada);
    }

    [HttpGet("{id}")]
    // [Authorize(Roles = "get(id)")]
    public async Task<ActionResult<GetEntradaDTO>> ObterEntradaPorId([Range(1, int.MaxValue)] int id)
    {
        var entrada = await _entradaService.ObterEntradaPorIdAsync(id);

        if (entrada == null) return NotFound();
        return Ok(entrada);
    }

    [HttpGet("entradas-recentes")]
    public async Task<ActionResult<IEnumerable<GetEntradaSimplesDTO>>> ObterEntradasRecentes()
    {
        try
        {
            var entradas = await _entradaService.ObterEntradasRecentesAsync();

            if (entradas == null || !entradas.Any())
            {
                return NoContent();
            }

            return Ok(entradas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao buscar entradas recentes: {ex.Message}");
        }
    }

    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<GetEntradaDTO>> CriarEntrada(PostEntradaDTO postEntradaDTO)
    {
        var entradaCriada = await _entradaService.CriarEntradaAsync(postEntradaDTO);

        return CreatedAtAction(nameof(ObterEntradaPorId), new { id = entradaCriada.Id },
            entradaCriada);
    }
}
