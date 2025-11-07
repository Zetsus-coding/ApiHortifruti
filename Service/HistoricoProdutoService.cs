using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class HistoricoProdutoService : IHistoricoProdutoService
{
    private readonly IUnityOfWork _uow;

    public HistoricoProdutoService(IUnityOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<HistoricoProduto>> ObterTodosHistoricoProdutosAsync()
    {
        try
        {
            return await _uow.HistoricoProduto.ObterTodosAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<HistoricoProduto?> ObterHistoricoProdutoPorIdAsync(int id)
    {
        return await _uow.HistoricoProduto.ObterPorIdAsync(id);
    }

    public async Task<HistoricoProduto> CriarHistoricoProdutoAsync(HistoricoProduto historicoProduto)
    {
        return await _uow.HistoricoProduto.AdicionarAsync(historicoProduto);
    }

    public async Task AtualizarHistoricoProdutoAsync(int id, HistoricoProduto historicoProduto)
    {
        if (id != historicoProduto.Id)
        {
            // Lançar erro/exceção
            return;
        }
        await _uow.HistoricoProduto.AtualizarAsync(historicoProduto);
    }

    public async Task DeletarHistoricoProdutoAsync(int id)
    {
        await _uow.HistoricoProduto.DeletarAsync(id);
    }
}

