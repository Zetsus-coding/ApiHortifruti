using Microsoft.EntityFrameworkCore;
using ApiHortifruti.Domain;
using ApiHortifruti.Data.Repository.Interfaces;

namespace ApiHortifruti.Data.Repository;

public class Item_saidaRepository : IItem_saidaRepository
{
    private readonly AppDbContext _context;

    public Item_saidaRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ItemSaida>> ObterTodasAsync()
    {
        return await _context.ItemSaida.ToListAsync();
    }

    public async Task<ItemSaida?> ObterPorIdAsync(int id)
    {
        return await _context.ItemSaida.FindAsync(id);
    }

    public async Task<ItemSaida> AdicionarAsync(ItemSaida item_saida)
    {
        _context.ItemSaida.Add(item_saida);
        await _context.SaveChangesAsync();
        return item_saida;
    }

    public async Task AtualizarAsync(ItemSaida item_saida)
    {
        _context.Entry(item_saida).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(int id)
    {
        var item_saida = await ObterPorIdAsync(id);
        
        if (item_saida != null)
        {
            _context.ItemSaida.Remove(item_saida);
            await _context.SaveChangesAsync();
        }
    }
}