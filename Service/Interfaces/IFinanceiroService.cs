using ApiHortifruti.DTO.Financeiro;

namespace SuaApi.Services;

public interface IFinanceiroService
{
    Task<GetLucroSemanalDTO> CalcularLucroSemanalAsync();
    Task<GetGastosMensaisDTO> CalcularGastosDoMesAsync();
    Task<GetVendasDiariasDTO> CalcularVendasDoDiaAsync();
}
