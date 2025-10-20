using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class Item_saidaService : IItem_saidaService
{
    private readonly IItem_saidaRepository _item_saidaRepository;

    public Item_saidaService(IItem_saidaRepository item_saidaRepository)
    {
        _item_saidaRepository = item_saidaRepository; // Inj. dependência
    }

    public async Task<IEnumerable<Item_saida>> ObterTodasItem_saidasAsync()
    {
        return await _item_saidaRepository.ObterTodasAsync();
    }

    public async Task<Item_saida?> ObterItem_saidaPorIdAsync(int id)
    {
        return await _item_saidaRepository.ObterPorIdAsync(id);
        
    }

    public async Task<Item_saida> CriarItem_saidaAsync(Item_saida item_saida)
    {
        return await _item_saidaRepository.AdicionarAsync(item_saida);
    }

    public async Task AtualizarItem_saidaAsync(int id, Item_saida item_saida)
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

