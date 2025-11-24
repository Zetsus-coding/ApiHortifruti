using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

// Interface do serviço de produto
public interface IProdutoService
{
    Task<IEnumerable<GetProdutoDTO>> ObterTodosOsProdutosAsync();
    Task<GetProdutoDTO?> ObterProdutoPorIdAsync(int id);    
    Task<GetProdutoDTO?> ObterProdutoPorCodigoAsync(string codigo);
    Task<IEnumerable<GetProdutoEstoqueCriticoDTO>> ObterProdutosEstoqueCriticoAsync();
    // Task<IEnumerable<FornecedorProduto>> ObterFornecedoresPorProdutoIdAsync(int produtoId); // TODO: Avaliar depois de implementar o obterProdutosPorFornecedorIdAsync
    Task<GetProdutoDTO> CriarProdutoAsync(PostProdutoDTO postProdutoDTO);
    Task AtualizarProdutoAsync(int id, PutProdutoDTO putProdutoDTO);
    Task DeletarProdutoAsync(int id);
}