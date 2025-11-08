using System.ComponentModel.DataAnnotations;
using System.Data;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHortifruti.Controller;

[ApiController]
[Route("api/[controller]")]

public class FornecedorController : ControllerBase
{
    private readonly IFornecedorService _fornecedorService;
    private readonly IMapper _mapper;

    // Construtor com injeção de dependência do serviço e do mapper
    public FornecedorController(IFornecedorService fornecedorService, IMapper mapper)
    {
        _fornecedorService = fornecedorService;
        _mapper = mapper;
    }

    // Consulta de todos os fornecedores
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Fornecedor>>> ObterFornecedores()
    {
        var fornecedor = await _fornecedorService.ObterTodosFornecedoresAsync(); // Chamada a camada de serviço para obter todos
        return Ok(fornecedor);
    }

    // Consulta de um fornecedor por ID
    [Authorize(Roles = "get(id)")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Fornecedor>> ObterFornecedor([Range(1, int.MaxValue)] int id)
    {
        var fornecedor = await _fornecedorService.ObterFornecedorPorIdAsync(id); // Chamada a camada de serviço para obter por ID

        return Ok(fornecedor);
    }

    // Criação de um novo fornecedor
    [Authorize(Roles = "post")]
    [HttpPost]
    public async Task<ActionResult<Fornecedor>> CriarFornecedor(PostFornecedorDTO postFornecedorDTO)
    {
        var fornecedor = _mapper.Map<Fornecedor>(postFornecedorDTO); // Conversão de DTO para entidade

        var fornecedorCriado = await _fornecedorService.CriarFornecedorAsync(fornecedor); // Chamada a camada de serviço para criar
        return CreatedAtAction(nameof(ObterFornecedor), new { fornecedorCriado.Id },
            fornecedorCriado);
    }

    // Atualização de um fornecedor existente
    [Authorize(Roles = "put")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarFornecedor([Range(1, int.MaxValue)] int id, Fornecedor fornecedor)
    {
        await _fornecedorService.AtualizarFornecedorAsync(id, fornecedor); // Chamada a camada de serviço para atualizar
        return NoContent();
    }
    
    // Exclusão de um fornecedor existente
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeletarFornecedor([Range(1, int.MaxValue)] int id) 
    // { 
    //     await _fornecedorService.DeletarFornecedorAsync(id); // Chamada a camada de serviço para deletar
    //     return NoContent(); 
    // }
}