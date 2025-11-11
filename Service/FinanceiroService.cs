using ApiHortifruti.Data.Repository.Interfaces;
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
        var dataFim = DateOnly.FromDateTime(DateTime.Today).AddDays(-1); // Amanhã 00:00:00 (para pegar hoje completo)
        var dataInicio = DateOnly.FromDateTime(dataFim.ToDateTime(TimeOnly.MinValue)).AddDays(-7); // 7 dias atrás

        var totalEntradas = await _uow.Entrada.ObterTotalPorPeriodoAsync(dataInicio, dataFim);
        var totalSaidas = await _uow.Saida.ObterTotalPorPeriodoAsync(dataInicio, dataFim);

        return totalEntradas - totalSaidas;
    }
}