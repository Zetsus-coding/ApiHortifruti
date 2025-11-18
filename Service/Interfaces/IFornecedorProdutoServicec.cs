using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IFornecedorProdutoService
{
    Task<IEnumerable<FornecedorProduto>> ObterTodosOsFornecedorProdutoAsync();
    Task<FornecedorProduto?> ObterFornecedorProdutoPorIdAsync(int fornecedorId, int produtoId);
    Task<FornecedorProduto> CriarFornecedorProdutoAsync(FornecedorProduto fornecedorProduto);
    Task<IEnumerable<FornecedorProduto>> CriarVariosFornecedorProdutosAsync(IEnumerable<FornecedorProduto> fornecedorProdutos);
    Task AtualizarFornecedorProdutoAsync(int fornecedorId, int produtoId, FornecedorProduto fornecedorProduto);
    Task DeletarFornecedorProdutoAsync(int fornecedorId, int produtoId);
}