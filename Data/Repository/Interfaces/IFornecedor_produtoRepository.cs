using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface IFornecedor_produtoRepository
{
    Task<IEnumerable<Fornecedor_produto>> ObterTodasAsync();
    Task<Fornecedor_produto?> ObterPorIdAsync(int fornecedorId, int produtoId);
    Task<Fornecedor_produto> AdicionarAsync(Fornecedor_produto fornecedor_produto);
    Task AtualizarAsync(Fornecedor_produto fornecedor_produto);
    Task DeletarAsync(int fornecedorId, int produtoId);
}