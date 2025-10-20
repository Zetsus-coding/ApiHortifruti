using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class Item_entradaService : IItem_entradaService
{
    private readonly IItem_entradaRepository _item_entradaRepository;

    public Item_entradaService(IItem_entradaRepository item_entradaRepository)
    {
        _item_entradaRepository = item_entradaRepository; // Inj. dependência
    }

    public async Task<IEnumerable<Item_entrada>> ObterTodasItem_entradasAsync()
    {
        return await _item_entradaRepository.ObterTodasAsync();
    }

    public async Task<Item_entrada?> ObterItem_entradaPorIdAsync(int id)
    {
        return await _item_entradaRepository.ObterPorIdAsync(id);
        
    }

    public async Task<Item_entrada> CriarItem_entradaAsync(Item_entrada item_entrada)
    {
        return await _item_entradaRepository.AdicionarAsync(item_entrada);
    }

    public async Task AtualizarItem_entradaAsync(int id, Item_entrada item_entrada)
    {
        if (id != item_entrada.Id)
        {
            // Lançar erro/exceção
            return;
        }
        await _item_entradaRepository.AtualizarAsync(item_entrada);
    }

    public async Task DeletarItem_entradaAsync(int id)
    {
        await _item_entradaRepository.DeletarAsync(id);
    }
}

