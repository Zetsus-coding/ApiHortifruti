using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IHistoricoProdutoService
{
    Task<IEnumerable<GetHistoricoProdutoDTO>> ObterTodosOsHistoricosProdutosAsync();
    Task<GetHistoricoProdutoDTO?> ObterHistoricoProdutoPorIdAsync(int id);
    Task<IEnumerable<GetHistoricoProdutoDTO>> ObterHistoricoProdutoPorProdutoIdAsync(int produtoId);
}
