using Hortifruti.Data.Repository.Interfaces;
using Hortifruti.Domain;
using Hortifruti.Service.Interfaces;

namespace Hortifruti.Service;


public class Historico_produtoService : IHistorico_produtoService
{
    private readonly IHistorico_produtoRepository _historico_produtoRepository;

    public Historico_produtoService(IHistorico_produtoRepository historico_produtoRepository)
    {
        _historico_produtoRepository = historico_produtoRepository;
    }

    public async Task<IEnumerable<Historico_produto>> ObterTodasHistorico_produtosAsync()
    {
        return await _historico_produtoRepository.ObterTodasAsync();
    }

    public async Task<Historico_produto?> ObterHistorico_produtoPorIdAsync(int id)
    {
        return await _historico_produtoRepository.ObterPorIdAsync(id);
    }

    public async Task<Historico_produto> CriarHistorico_produtoAsync(Historico_produto historico_produto)
    {
        return await _historico_produtoRepository.AdicionarAsync(historico_produto);
    }

    public async Task AtualizarHistorico_produtoAsync(int id, Historico_produto historico_produto)
    {
        if (id != historico_produto.Id)
        {
            // Lançar erro/exceção
            return;
        }
        await _historico_produtoRepository.AtualizarAsync(historico_produto);
    }

    public async Task DeletarHistorico_produtoAsync(int id)
    {
        await _historico_produtoRepository.DeletarAsync(id);
    }
}

