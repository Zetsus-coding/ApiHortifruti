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

    public async Task<GetLucroSemanalDTO> CalcularLucroSemanalAsync()
    {
        var dataFim = DateOnly.FromDateTime(DateTime.Today);
        var dataInicio = DateOnly.FromDateTime(dataFim.ToDateTime(TimeOnly.MinValue)).AddDays(-7);

        var totalEntradas = await _uow.Entrada.ObterValorTotalPorPeriodoAsync(dataInicio, dataFim);
        var totalSaidas = await _uow.Saida.ObterValorTotalPorPeriodoAsync(dataInicio, dataFim);

        return new GetLucroSemanalDTO { Valor = totalSaidas - totalEntradas };
    }

    public async Task<GetGastosMensaisDTO> CalcularGastosDoMesAsync()
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var primeiroDia = new DateOnly(hoje.Year, hoje.Month, 1);
        var ultimoDia = primeiroDia.AddMonths(1).AddDays(-1);

        var totalEntradas = await _uow.Entrada.ObterValorTotalPorPeriodoAsync(primeiroDia, ultimoDia);
        return new GetGastosMensaisDTO { Valor = totalEntradas };
    }

    public async Task<GetVendasDiariasDTO> CalcularVendasDoDiaAsync()
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var totalSaidas = await _uow.Saida.ObterValorTotalPorPeriodoAsync(hoje, hoje);
        return new GetVendasDiariasDTO { Valor = totalSaidas };
    }
}
