using ApiHortifruti.Domain;
using ApiHortifruti.DTO.Entrada;

namespace ApiHortifruti.Service.Interfaces;

public interface IEntradaService
{
    Task<IEnumerable<GetEntradaSimplesDTO>> ObterTodasAsEntradasAsync();
    Task<GetEntradaDTO?> ObterEntradaPorIdAsync(int id);
    Task<IEnumerable<GetEntradaSimplesDTO>> ObterEntradasRecentesAsync();
    Task<GetEntradaDTO> CriarEntradaAsync(PostEntradaDTO postEntradaDTO);
}
