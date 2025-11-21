using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IHistoricoProdutoService
{
    Task<IEnumerable<HistoricoProduto>> ObterTodosOsHistoricosProdutosAsync();
    Task<HistoricoProduto?> ObterHistoricoProdutoPorIdAsync(int id);
    Task<HistoricoProduto> CriarHistoricoProdutoAsync(HistoricoProduto historicoProduto);
    Task<IEnumerable<HistoricoProduto>> ObterHistoricoProdutoPorProdutoIdAsync(int produtoId);
    // Task AtualizarHistoricoProdutoAsync(int id, HistoricoProduto historicoProduto);
    // Task DeletarHistoricoProdutoAsync(int id);
}