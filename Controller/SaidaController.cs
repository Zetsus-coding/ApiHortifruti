using System.ComponentModel.DataAnnotations;
using ApiHortifruti.DTO.Saida;
using ApiHortifruti.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controller;

[ApiController]
[Route("api/[controller]")]

public class SaidaController : ControllerBase
{
    private readonly ISaidaService _saidaService;

    // CONSTRUTOR + INJEÇÃO DE DEPENDÊNCIA
    public SaidaController(ISaidaService saidaService)
    {
        _saidaService = saidaService;
    }

    // OPERAÇÕES
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetSaidaSimplesDTO>>> ObterTodasAsSaidas()
    {
        var saida = await _saidaService.ObterTodasAsSaidasAsync();
        return Ok(saida); 
    }

    // [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetSaidaDTO>> ObterSaidaPorId([Range(1, int.MaxValue)] int id)
    {
        var saida = await _saidaService.ObterSaidaPorIdAsync(id);

        if (saida == null) return NotFound();
        return Ok(saida);
    }

    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<GetSaidaDTO>> CriarSaida(PostSaidaDTO postSaidaDTO)
    {
        var saidaCriada = await _saidaService.CriarSaidaAsync(postSaidaDTO);
        return CreatedAtAction(nameof(ObterSaidaPorId), new { id = saidaCriada.Id },
            saidaCriada);
    }
}
