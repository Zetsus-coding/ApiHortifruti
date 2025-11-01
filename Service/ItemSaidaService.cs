using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class ItemSaidaService : IItemSaidaService
{
    private readonly IItem_saidaRepository _item_saidaRepository;

    public ItemSaidaService(IItem_saidaRepository item_saidaRepository)
    {
        _item_saidaRepository = item_saidaRepository; // Inj. dependência
    }

    public async Task<IEnumerable<ItemSaida>> ObterTodasItem_saidasAsync()
    {
        return await _item_saidaRepository.ObterTodasAsync();
    }

    public async Task<ItemSaida?> ObterItem_saidaPorIdAsync(int id)
    {
        return await _item_saidaRepository.ObterPorIdAsync(id);
        
    }

    public async Task<ItemSaida> CriarItem_saidaAsync(ItemSaida item_saida)
    {
        return await _item_saidaRepository.AdicionarAsync(item_saida);
    }

    public async Task AtualizarItem_saidaAsync(int id, ItemSaida item_saida)
    {
        if (id != item_saida.Id)
        {
            // Lançar erro/exceção
            return;
        }
        await _item_saidaRepository.AtualizarAsync(item_saida);
    }

    public async Task DeletarItem_saidaAsync(int id)
    {
        await _item_saidaRepository.DeletarAsync(id);
    }
}

