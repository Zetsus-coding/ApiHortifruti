using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

// Interface do serviço de produto
public interface IProdutoService
{
    Task<IEnumerable<Produto>> ObterTodosOsProdutosAsync();
    Task<Produto?> ObterProdutoPorIdAsync(int id);    
    Task<Produto?> ObterProdutoPorCodigoAsync(string codigo);
    Task<IEnumerable<Produto>> ObterProdutosEstoqueCriticoAsync();
    // Task<IEnumerable<FornecedorProduto>> ObterFornecedoresPorProdutoIdAsync(int produtoId); // TODO: Avaliar depois de implementar o obterProdutosPorFornecedorIdAsync
    Task<Produto> CriarProdutoAsync(PostProdutoDTO postProdutoDTO);
    Task AtualizarProdutoAsync(int id, Produto produto);
    Task DeletarProdutoAsync(int id);
}