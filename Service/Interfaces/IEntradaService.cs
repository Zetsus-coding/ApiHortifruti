using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IEntradaService
{
    Task<IEnumerable<GetEntradaSimplesDTO>> ObterTodasAsEntradasAsync();
    Task<GetEntradaSimplesDTO?> ObterEntradaPorIdAsync(int id);
    Task<IEnumerable<GetEntradaSimplesDTO>> ObterEntradasRecentesAsync(int dias);
    Task<Entrada> CriarEntradaAsync(PostEntradaDTO postEntradaDTO);
    // Task AtualizarEntradaAsync(int id, Entrada entrada);
    // Task DeletarEntradaAsync(int id);
}