using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controller;

[ApiController]
[Route("api/[controller]")]

public class FuncionarioController : ControllerBase
{
    private readonly IFuncionarioService _funcionarioService;

    public FuncionarioController(IFuncionarioService funcionarioService)
    {
        _funcionarioService = funcionarioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetFuncionarioDTO>>> ObterTodosOsFuncionarios()
    {
        var funcionario = await _funcionarioService.ObterTodosOsFuncionariosAsync();
        return Ok(funcionario);
    }

    // [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetFuncionarioDTO>> ObterFuncionarioPorId([Range(1, int.MaxValue)]int id)
    {
        var funcionario = await _funcionarioService.ObterFuncionarioPorIdAsync(id);

        if (funcionario == null) return NotFound();
        return Ok(funcionario);
    }
    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<GetFuncionarioDTO>> CriarFuncionario(PostFuncionarioDTO postFuncionarioDTO)
    {
        var funcionarioCriado = await _funcionarioService.CriarFuncionarioAsync(postFuncionarioDTO);
        return CreatedAtAction(nameof(ObterFuncionarioPorId), new { id = funcionarioCriado.Id },
            funcionarioCriado);
    }

    // [Authorize(Roles = "put")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarFuncionario([Range(1, int.MaxValue)] int id, PutFuncionarioDTO putFuncionarioDTO)
    {
        putFuncionarioDTO.Id = id;
        await _funcionarioService.AtualizarFuncionarioAsync(id, putFuncionarioDTO);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarFuncionario([Range(1, int.MaxValue)]int id)
    {
        await _funcionarioService.DeletarFuncionarioAsync(id);
        return NoContent();
    }
}
