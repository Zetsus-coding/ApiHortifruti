using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IFornecedorProdutoService
{
    Task<IEnumerable<GetFornecedorProdutoDTO>> ObterTodosOsFornecedorProdutoAsync();
    Task<GetFornecedorProdutoDTO?> ObterFornecedorProdutoPorIdAsync(int fornecedorId, int produtoId);
    Task<GetFornecedorProdutoDTO> CriarFornecedorProdutoAsync(PostFornecedorProdutoDTO postFornecedorProdutoDTO);
    Task<IEnumerable<GetFornecedorProdutoDTO>> CriarVariosFornecedorProdutosAsync(IEnumerable<PostFornecedorProdutoDTO> postFornecedorProdutoDTOs);
    Task AtualizarFornecedorProdutoAsync(int fornecedorId, int produtoId, PutFornecedorProdutoDTO putFornecedorProdutoDTO);
    Task DeletarFornecedorProdutoAsync(int fornecedorId, int produtoId);
}
