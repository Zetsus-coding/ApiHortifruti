using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface IEntradaRepository
{
    Task<IEnumerable<Entrada>> ObterTodasAsync();
    Task<Entrada?> ObterPorIdAsync(int id);
    Task<Entrada> AdicionarAsync(Entrada entrada);
    Task AtualizarAsync(Entrada entrada);
    Task DeletarAsync(int id);
}