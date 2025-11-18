using Microsoft.EntityFrameworkCore;
using ApiHortifruti.Domain;
using ApiHortifruti.Data.Repository.Interfaces;

namespace ApiHortifruti.Data.Repository;

public class ItemSaidaRepository : IItemSaidaRepository
{
    private readonly AppDbContext _context;

    public ItemSaidaRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ItemSaida>> ObterTodosAsync()
    {
        return await _context.ItemSaida.ToListAsync();
    }

    public async Task<ItemSaida?> ObterPorIdAsync(int id)
    {
        return await _context.ItemSaida.FindAsync(id);
    }

    public async Task<ItemSaida> AdicionarAsync(ItemSaida itemSaida)
    {
        _context.ItemSaida.Add(itemSaida);
        return itemSaida;
    }

    public async Task AdicionarItensSaidaAsync(IEnumerable<ItemSaida> itens)
    {
        await _context.ItemSaida.AddRangeAsync(itens);
    }

    public async Task AtualizarAsync(ItemSaida itemSaida)
    {
        _context.Entry(itemSaida).State = EntityState.Modified;
    }

    public async Task DeletarAsync(int id)
    {
        var itemSaida = await ObterPorIdAsync(id);
        
        if (itemSaida != null)
        {
            _context.ItemSaida.Remove(itemSaida);
        }
    }

    public Task ObterTodosDeUmaSaidaAsync(int idSaida)
    {
        throw new NotImplementedException();
    }
}