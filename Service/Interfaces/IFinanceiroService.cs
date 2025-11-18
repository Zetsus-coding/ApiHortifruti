using ApiHortifruti.Domain;

namespace SuaApi.Services;

public interface IFinanceiroService
{
    Task<decimal> CalcularLucroSemanalAsync();
    Task<decimal> CalcularGastosDoMesAsync();
    Task<decimal> CalcularVendasDoDiaAsync();
}