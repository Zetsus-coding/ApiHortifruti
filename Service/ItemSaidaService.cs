using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class ItemSaidaService : IItemSaidaService
{
    private readonly IUnityOfWork _uow;

    public ItemSaidaService( IUnityOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<ItemSaida>> ObterTodosItemSaidasAsync()
    {
        try
        {
            return await _uow.ItensSaida.ObterTodosAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ItemSaida?> ObterItemSaidaPorIdAsync(int id)
    {
        return await _uow.ItensSaida.ObterPorIdAsync(id);
        
    }

    public async Task<ItemSaida> CriarItemSaidaAsync(ItemSaida itemSaida)
    {
        return await _uow.ItensSaida.AdicionarAsync(itemSaida);
    }

    public async Task AtualizarItemSaidaAsync(int id, ItemSaida itemSaida)
    {
        if (id != itemSaida.Id)
        {
            // Lançar erro/exceção
            return;
        }
        await _uow.ItensSaida.AtualizarAsync(itemSaida);
    }

    public async Task DeletarItemSaidaAsync(int id)
    {
        await _uow.ItensSaida.DeletarAsync(id);
    }
}

