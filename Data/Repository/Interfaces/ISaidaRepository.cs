using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface ISaidaRepository
{
    Task<IEnumerable<Saida>> ObterTodosAsync();
    Task<Saida?> ObterPorIdAsync(int id);
    Task <decimal> ObterValorTotalPorPeriodoAsync(DateOnly dataInicio, DateOnly dataFim);
    Task<Saida> AdicionarAsync(Saida saida);
    Task AtualizarAsync(Saida saida);
    Task DeletarAsync(int id);
}