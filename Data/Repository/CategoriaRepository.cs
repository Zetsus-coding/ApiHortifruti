using Microsoft.EntityFrameworkCore;
using ApiHortifruti.Domain;
using ApiHortifruti.Data.Repository.Interfaces;

namespace ApiHortifruti.Data.Repository;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Categoria>> ObterTodosAsync()
    {
        return await _context.Categoria.ToListAsync();
    }

    public async Task<Categoria?> ObterPorIdAsync(int id)
    {
        return await _context.Categoria.FindAsync(id);
    }

    public async Task<Categoria> AdicionarAsync(Categoria categoria)
    {
        _context.Categoria.Add(categoria);
        return categoria;
    }

    public async Task AtualizarAsync(Categoria categoria)
    {
        _context.Entry(categoria).State = EntityState.Modified;
    }
    
    public async Task DeletarAsync(Categoria categoria)
    {
        _context.Categoria.Remove(categoria);
    }
}