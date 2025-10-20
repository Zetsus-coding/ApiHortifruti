using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface ISaidaRepository
{
    Task<IEnumerable<Saida>> ObterTodasAsync();
    Task<Saida?> ObterPorIdAsync(int id);
    Task<Saida> AdicionarAsync(Saida saida);
    Task AtualizarAsync(Saida saida);
    Task DeletarAsync(int id);
}