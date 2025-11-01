using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface IFornecedorProdutoRepository
{
    Task<IEnumerable<FornecedorProduto>> ObterTodasAsync();
    Task<FornecedorProduto?> ObterPorIdAsync(int fornecedorId, int produtoId);
    Task<FornecedorProduto> AdicionarAsync(FornecedorProduto fornecedorProduto);
    Task AtualizarAsync(FornecedorProduto fornecedorProduto);
    Task DeletarAsync(int fornecedorId, int produtoId);
}