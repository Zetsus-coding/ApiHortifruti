using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controller;

[ApiController]
[Route("api/[controller]")]

public class SaidaController : ControllerBase
{
    private readonly ISaidaService _saidaService;
    private readonly IMapper _mapper;

    // CONSTRUTOR + INJEÇÃO DE DEPENDÊNCIA
    public SaidaController(ISaidaService saidaService, IMapper mapper)
    {
        _saidaService = saidaService;
        _mapper = mapper;
    }

    // OPERAÇÕES
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Saida>>> ObterSaidas() // get all
    {
        var saida = await _saidaService.ObterTodosSaidasAsync();
        return Ok(saida);
    }

    // [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Saida>> ObterSaida([Range(1, int.MaxValue)] int id) // get por id
    {
        var saida = await _saidaService.ObterSaidaPorIdAsync(id);

        if (saida == null) return NotFound();
        return Ok(saida);
    }

    // get produtos associados a saida (aqui [/saida/idsaida/produtos] ou em produtos [/produtos?saida=x])?
    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<Saida>> CriarSaida(PostSaidaDTO postSaidaDTO)
    {
        var saida = _mapper.Map<Saida>(postSaidaDTO);

        var saidaCriada = await _saidaService.CriarSaidaAsync(saida);
        return CreatedAtAction(nameof(ObterSaida), new { id = saidaCriada.Id },
            saidaCriada);
    }
    
    // [Authorize(Roles = "put")]
    // [HttpPut("{id}")]
    // public async Task<IActionResult> AtualizarSaida([Range(1, int.MaxValue)] int id, Saida saida)
    // {
    //     if (id != saida.Id) return BadRequest();
    //     await _saidaService.AtualizarSaidaAsync(id, saida);
    //     return NoContent();
    // }

    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeletarSaida([Range(1, int.MaxValue)] int id) 
    // { 
    //     await _saidaService.DeletarSaidaAsync(id); 
    //     return NoContent(); 
    // }
}