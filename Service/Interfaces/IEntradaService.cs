using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IEntradaService
{
    Task<IEnumerable<Entrada>> ObterTodasAsEntradasAsync();
    Task<IEnumerable<Entrada>> ObterEntradasRecentesAsync();
    Task<Entrada?> ObterEntradaPorIdAsync(int id);
    Task<Entrada> CriarEntradaAsync(Entrada entrada);
    Task AtualizarEntradaAsync(int id, Entrada entrada);
    
    // Task DeletarEntradaAsync(int id);
}