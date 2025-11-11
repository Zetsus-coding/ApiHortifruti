namespace SuaApi.Services;

public interface IFinanceiroService
{
    Task<decimal> CalcularLucroSemanalAsync();
}