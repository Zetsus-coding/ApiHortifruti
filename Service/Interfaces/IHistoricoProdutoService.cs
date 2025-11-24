using ApiHortifruti.Domain;
using ApiHortifruti.DTO.HistoricoProduto;

namespace ApiHortifruti.Service.Interfaces;

public interface IHistoricoProdutoService
{
    Task<IEnumerable<GetHistoricoProdutoDTO>> ObterTodosOsHistoricosProdutosAsync();
    Task<GetHistoricoProdutoDTO?> ObterHistoricoProdutoPorIdAsync(int id);
    Task<IEnumerable<GetHistoricoProdutoDTO>> ObterHistoricoProdutoPorProdutoIdAsync(int produtoId);
    Task CriarHistoricoProdutoAsync(HistoricoProduto historicoProduto); // Called internally by ProdutoService? Keep Entity if internal?
    // User said: "O post ocorre apenas em service mas ele é chamado na criação e alteração de um produto então não é necessário dto para isso."
    // If it is called by other services, it might expect Entity.
    // However, interface methods are usually public.
    // If it is NOT exposed in controller, we can keep it as Entity or make it internal.
    // Given the instruction "implementar os dtos de get", and "post ocorre apenas em service", I will keep CriarHistoricoProdutoAsync taking Entity (or remove it from interface if it's only internal, but it's likely used by ProdutoService).
}
