using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controller;

[ApiController]
[Route("api/[controller]")]

public class FuncionarioController : ControllerBase
{
    private readonly IFuncionarioService _funcionarioService;
    private readonly IMapper _mapper;

    public FuncionarioController(IFuncionarioService funcionarioService, IMapper mapper)
    {
        _funcionarioService = funcionarioService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Funcionario>>> ObterFuncionarios()
    {
        var funcionario = await _funcionarioService.ObterTodosFuncionarioAsync();
        return Ok(funcionario);
    }

    // [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Funcionario>> ObteFuncionario([Range(1, int.MaxValue)]int id)
    {
        var funcionario = await _funcionarioService.ObterFuncionarioPorIdAsync(id);

        if (funcionario == null) return NotFound();
        return Ok(funcionario);
    }
    // [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<Funcionario>> CriarFuncionario(PostFuncionarioDTO postFuncionarioDTO)
    {
        var funcionario = _mapper.Map<Funcionario>(postFuncionarioDTO); // Conversão de DTO para entidade
        
        var funcionarioCriado = await _funcionarioService.CriarFuncionarioAsync(funcionario);
        return CreatedAtAction(nameof(ObteFuncionario), new { funcionarioCriado.Id },
            funcionarioCriado);
    }
    // [Authorize(Roles = "put")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarFuncionario([Range(1, int.MaxValue)] int id, Funcionario funcionario)
    {
        await _funcionarioService.AtualizarFuncionarioAsync(id, funcionario);
        return NoContent();
    }
    
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeletarFuncionario([Range(1, int.MaxValue)]int id) 
    // { 
    //     await _funcionarioService.DeletarFuncionarioAsync(id); 
    //     return NoContent(); 
    // }
}