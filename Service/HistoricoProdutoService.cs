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

    public async Task<IEnumerable<HistoricoProduto>> ObterTodosOsHistoricosProdutosAsync()
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

    public async Task<IEnumerable<HistoricoProduto>> ObterHistoricoProdutoPorProdutoIdAsync(int produtoId)
    {
        return await _uow.HistoricoProduto.ObterPorProdutoIdAsync(produtoId);
    }

    public async Task<HistoricoProduto> CriarHistoricoProdutoAsync(HistoricoProduto historicoProduto)
    {
        var criado = await _uow.HistoricoProduto.AdicionarAsync(historicoProduto);

        await _uow.SaveChangesAsync();
        return criado;
    }

    // public async Task AtualizarHistoricoProdutoAsync(int id, HistoricoProduto historicoProduto)
    // {
    //     if (id != historicoProduto.Id)
    //     {
    //         throw new ArgumentException("O ID informado não é o mesmo que está sendo editado");
    //     }
    //     await _uow.HistoricoProduto.AtualizarAsync(historicoProduto);
    //     await _uow.SaveChangesAsync();
    // }

    // public async Task DeletarHistoricoProdutoAsync(int id)
    // {
    //     await _uow.HistoricoProduto.DeletarAsync(id);
    //     await _uow.SaveChangesAsync();
    // }
}

