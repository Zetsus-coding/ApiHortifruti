using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

// Interface do serviço de produto
public interface IProdutoService
{
    Task<IEnumerable<Produto>> ObterTodosOsProdutosAsync();
    Task<Produto?> ObterProdutoPorIdAsync(int id);    
    Task<Produto?> ObterProdutoPorCodigoAsync(string codigo);
    Task<IEnumerable<Produto>> ObterProdutosEstoqueCriticoAsync();
    Task<Produto> CriarProdutoAsync(Produto produto);
    Task AtualizarProdutoAsync(int id, Produto produto);
    Task<IEnumerable<FornecedorProduto>> ObterProdutosPorFornecedorIdAsync(int fornecedorId);

    //Task DeletarProdutoAsync(int id);
}