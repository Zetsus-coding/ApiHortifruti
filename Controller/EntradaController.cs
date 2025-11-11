using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controller;

[ApiController]
[Route("api/[controller]")]

public class EntradaController : ControllerBase
{
    private readonly IEntradaService _entradaService;
    private readonly IMapper _mapper;

    // CONSTRUTOR + INJEÇÃO DE DEPENDÊNCIA
    public EntradaController(IEntradaService entradaService, IMapper mapper)
    {
        _entradaService = entradaService;
        _mapper = mapper;
    }

    // OPERAÇÕES
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Entrada>>> ObterEntradas()
    {
        var entrada = await _entradaService.ObterTodosEntradasAsync();
        return Ok(entrada);
    }

    [HttpGet("{id}")]
    // [Authorize(Roles = "get(id)")]
    public async Task<ActionResult<Entrada>> ObterEntrada([Range(1, int.MaxValue)] int id)
    {
        var entrada = await _entradaService.ObterEntradaPorIdAsync(id);

        if (entrada == null) return NotFound();
        return Ok(entrada);
    }

    // get produtos associados a entrada (aqui [/entrada/identrada/produtos] ou em produtos [/produtos?entrada=x])?
    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<Entrada>> CriarEntrada(PostEntradaDTO postEntradaDTO)
    {
        var entrada = _mapper.Map<Entrada>(postEntradaDTO);

        var entradaCriada = await _entradaService.CriarEntradaAsync(entrada);
        return CreatedAtAction(nameof(ObterEntrada), new { id = entradaCriada.Id },
            entradaCriada);
    }
    // [Authorize(Roles = "put")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarEntrada([Range(1, int.MaxValue)] int id, Entrada entrada)
    {
        await _entradaService.AtualizarEntradaAsync(id, entrada);
        return NoContent();
    }

    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeletarEntrada([Range(1, int.MaxValue)] int id) 
    // { 
    //     await _entradaService.DeletarEntradaAsync(id); 
    //     return NoContent(); 
    // } 
}