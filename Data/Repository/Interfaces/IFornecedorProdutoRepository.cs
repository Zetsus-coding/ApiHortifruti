using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface IFornecedorProdutoRepository
{
    Task<IEnumerable<FornecedorProduto>> ObterTodosAsync();
    Task<FornecedorProduto?> ObterPorIdAsync(int fornecedorId, int produtoId);
    Task<IEnumerable<FornecedorComListaProdutosDTO>> ObterProdutosPorFornecedorIdAsync(int fornecedorId);
    Task<FornecedorProduto> AdicionarAsync(FornecedorProduto fornecedorProduto);
    Task<IEnumerable<FornecedorProduto>> AdicionarVariosAsync(IEnumerable<FornecedorProduto> fornecedorProdutos);
    Task AtualizarAsync(FornecedorProduto fornecedorProduto);
    Task DeletarAsync(int fornecedorId, int produtoId);
}