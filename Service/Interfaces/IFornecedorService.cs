using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

// Interface para o serviço de Fornecedor
public interface IFornecedorService
{
    Task<IEnumerable<Fornecedor>> ObterTodosOsFornecedoresAsync();
    Task<Fornecedor?> ObterFornecedorPorIdAsync(int id);
    Task<IEnumerable<FornecedorComListaProdutosDTO>> ObterProdutosPorFornecedorIdAsync(int fornecedorId);
    Task<Fornecedor> ObterFornecedorComProdutosAsync(int id);
    Task<Fornecedor> CriarFornecedorAsync(Fornecedor fornecedor);
    Task AtualizarFornecedorAsync(int id, Fornecedor fornecedor);
    Task DeletarFornecedorAsync(int id);
}