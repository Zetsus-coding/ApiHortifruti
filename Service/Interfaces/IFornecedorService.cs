using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

// Interface para o serviço de Fornecedor
public interface IFornecedorService
{
    Task<IEnumerable<GetFornecedorDTO>> ObterTodosOsFornecedoresAsync(); // Retorna uma lista de todos os fornecedores como DTOs
    Task<GetFornecedorDTO?> ObterFornecedorPorIdAsync(int id); // Recebe id e retorna o fornecedor correspondente como DTO
    Task<FornecedorComListaProdutosDTO> ObterProdutosPorFornecedorIdAsync(int fornecedorId); // Recebe o id do fornecedor e retorna um DTO com a lista de produtos fornecidos por ele
    Task<GetFornecedorDTO> CriarFornecedorAsync(PostFornecedorDTO postFornecedorDTO); // Recebe um DTO e cria um novo fornecedor, retornando o DTO do fornecedor criado
    Task AtualizarFornecedorAsync(int id, PutFornecedorDTO putFornecedorDTO); // Atualiza um fornecedor existente
    Task DeletarFornecedorAsync(int id); // Deleta um fornecedor existente
}