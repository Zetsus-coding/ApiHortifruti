using Microsoft.EntityFrameworkCore;
using ApiHortifruti.Domain;
using ApiHortifruti.Data.Repository.Interfaces;

namespace ApiHortifruti.Data.Repository;

public class Item_entradaRepository : IItem_entradaRepository
{
    private readonly AppDbContext _context;

    public Item_entradaRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Item_entrada>> ObterTodasAsync()
    {
        return await _context.Item_entrada.ToListAsync();
    }

    public async Task<Item_entrada?> ObterPorIdAsync(int id)
    {
        return await _context.Item_entrada.FindAsync(id);
    }

    public async Task<Item_entrada> AdicionarAsync(Item_entrada item_entrada)
    {
        _context.Item_entrada.Add(item_entrada);
        await _context.SaveChangesAsync();
        return item_entrada;
    }

    public async Task AtualizarAsync(Item_entrada item_entrada)
    {
        _context.Entry(item_entrada).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(int id)
    {
        var item_entrada = await ObterPorIdAsync(id);
        
        if (item_entrada != null)
        {
            _context.Item_entrada.Remove(item_entrada);
            await _context.SaveChangesAsync();
        }
    }
}