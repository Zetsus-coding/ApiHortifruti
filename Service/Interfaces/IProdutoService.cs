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
    Task<ProdutoComListaDeFornecedoresDTO> ObterListaDeFornecedoresQueFornecemCertoProduto(int produtoId);
    Task<GetProdutoDTO> CriarProdutoAsync(PostProdutoDTO postProdutoDTO);
    Task AtualizarProdutoAsync(int id, PutProdutoDTO putProdutoDTO);
    Task DeletarProdutoAsync(int id);
}