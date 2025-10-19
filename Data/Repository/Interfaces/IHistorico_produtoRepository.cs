using Hortifruti.Domain;

namespace Hortifruti.Data.Repository.Interfaces;

public interface IHistorico_produtoRepository
{
    Task<IEnumerable<Historico_produto>> ObterTodasAsync();
    Task<Historico_produto?> ObterPorIdAsync(int id);
    Task<Historico_produto> AdicionarAsync(Historico_produto historico_produto);
    Task AtualizarAsync(Historico_produto historico_produto);
    Task DeletarAsync(int id);
}