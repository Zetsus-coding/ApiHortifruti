using ApiHortifruti.Domain;
using ApiHortifruti.DTO.HistoricoProduto;

namespace ApiHortifruti.Service.Interfaces;

public interface IHistoricoProdutoService
{
    Task<IEnumerable<GetHistoricoProdutoDTO>> ObterTodosOsHistoricosProdutosAsync();
    Task<GetHistoricoProdutoDTO?> ObterHistoricoProdutoPorIdAsync(int id);
    Task<IEnumerable<GetHistoricoProdutoDTO>> ObterHistoricoProdutoPorProdutoIdAsync(int produtoId);
}
