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

    [HttpGet("gastos-mensais")]
    public async Task<ActionResult<decimal>> ObterGastosMensais()
    {
        try
        {
            var gastos = await _financeiroService.CalcularGastosDoMesAsync();
            return Ok(gastos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao calcular gastos mensais: {ex.Message}");
        }
    }

    [HttpGet("vendas-diarias")]
    public async Task<ActionResult<decimal>> ObterVendasDiarias()
    {
        try
        {
            var vendas = await _financeiroService.CalcularVendasDoDiaAsync();
            return Ok(vendas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao calcular vendas diárias: {ex.Message}");
        }
    }
}