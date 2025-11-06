using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

// Interface para o serviço de Fornecedor
public interface IFornecedorService
{
    Task<IEnumerable<Fornecedor>> ObterTodosFornecedoresAsync();
    Task<Fornecedor?> ObterFornecedorPorIdAsync(int id);
    Task<Fornecedor> CriarFornecedorAsync(Fornecedor fornecedor);
    Task AtualizarFornecedorAsync(int id, Fornecedor fornecedor);

    // Task DeletarFornecedorAsync(int id);
}