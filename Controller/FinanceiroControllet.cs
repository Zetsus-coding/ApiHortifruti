using Microsoft.AspNetCore.Mvc;
using SuaApi.Services;

namespace ApiHortifruti.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinanceiroController : ControllerBase
{
    private readonly IFinanceiroService _financeiroService;

    public FinanceiroController(IFinanceiroService financeiroService)
    {
        _financeiroService = financeiroService;
    }
    /// Retorna o lucro semanal (entradas - saídas) do dia atual até 7 dias atrás
    
    /// <returns>Valor do lucro semanal</returns>
    [HttpGet("lucro-semanal")]
    public async Task<ActionResult<decimal>> ObterLucroSemanal()
    {
        try
        {
            var lucro = await _financeiroService.CalcularLucroSemanalAsync();
            return Ok(lucro);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao calcular lucro semanal: {ex.Message}");
        }
    }
}