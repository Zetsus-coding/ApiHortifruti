using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IHistorico_produtoService
{
    Task<IEnumerable<Historico_produto>> ObterTodasHistorico_produtosAsync();
    Task<Historico_produto?> ObterHistorico_produtoPorIdAsync(int id);
    Task<Historico_produto> CriarHistorico_produtoAsync(Historico_produto historico_produto);
    Task AtualizarHistorico_produtoAsync(int id, Historico_produto historico_produto);
    Task DeletarHistorico_produtoAsync(int id);
}