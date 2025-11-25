using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IEntradaService
{
    Task<IEnumerable<GetEntradaSimplesDTO>> ObterTodasAsEntradasAsync();
    Task<GetEntradaDTO?> ObterEntradaPorIdAsync(int id);
    Task<IEnumerable<GetEntradaDTO>> ObterEntradasRecentesAsync();
    Task<GetEntradaDTO> CriarEntradaAsync(PostEntradaDTO postEntradaDTO);
}
