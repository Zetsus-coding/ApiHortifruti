using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using SuaApi.Services;

namespace ApiHortifruti.Services;

public class FinanceiroService : IFinanceiroService
{
    private readonly IUnityOfWork _uow;
    

    public FinanceiroService(IUnityOfWork uow)
    {
        _uow = uow;
    }

    public async Task<decimal> CalcularLucroSemanalAsync()
    {
        var dataFim = DateOnly.FromDateTime(DateTime.Today); // Amanhã 00:00:00 (para pegar hoje completo)
        var dataInicio = DateOnly.FromDateTime(dataFim.ToDateTime(TimeOnly.MinValue)).AddDays(-7); // 7 dias atrás

        var totalEntradas = await _uow.Entrada.ObterTotalPorPeriodoAsync(dataInicio, dataFim);
        var totalSaidas = await _uow.Saida.ObterTotalPorPeriodoAsync(dataInicio, dataFim);

        return totalSaidas - totalEntradas;
    }
    public async Task<decimal> CalcularGastosDoMesAsync()
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var primeiroDia = new DateOnly(hoje.Year, hoje.Month, 1);
        var ultimoDia = primeiroDia.AddMonths(1).AddDays(-1); // último dia do mês

        var totalEntradas = await _uow.Entrada.ObterTotalPorPeriodoAsync(primeiroDia, ultimoDia);
        return totalEntradas;
    }
    public async Task<decimal> CalcularVendasDoDiaAsync()
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var totalSaidas = await _uow.Saida.ObterTotalPorPeriodoAsync(hoje, hoje);
        return totalSaidas;
    }
    public async Task<IEnumerable<Entrada>> ObterEntradasRecentesAsync()
    {
        return await _uow.Entrada.ObterRecentesAsync();
    }
}