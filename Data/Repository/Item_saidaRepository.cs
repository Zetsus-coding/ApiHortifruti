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
    
    public async Task<IEnumerable<Item_saida>> ObterTodasAsync()
    {
        return await _context.Item_saida.ToListAsync();
    }

    public async Task<Item_saida?> ObterPorIdAsync(int id)
    {
        return await _context.Item_saida.FindAsync(id);
    }

    public async Task<Item_saida> AdicionarAsync(Item_saida item_saida)
    {
        _context.Item_saida.Add(item_saida);
        await _context.SaveChangesAsync();
        return item_saida;
    }

    public async Task AtualizarAsync(Item_saida item_saida)
    {
        _context.Entry(item_saida).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(int id)
    {
        var item_saida = await ObterPorIdAsync(id);
        
        if (item_saida != null)
        {
            _context.Item_saida.Remove(item_saida);
            await _context.SaveChangesAsync();
        }
    }
}