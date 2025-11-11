using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> ObterTodosAsync();
    Task<Produto?> ObterPorIdAsync(int id);
    Task<Produto?> ObterProdutoPorCodigoAsync(string produto);
    Task<IEnumerable<Produto>> ObterEstoqueCriticoAsync();
    Task<Produto> AdicionarAsync(Produto produto);
    Task AtualizarAsync(Produto produto);
   
    // Task DeletarAsync(int id);
}