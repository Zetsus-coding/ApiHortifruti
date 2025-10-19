using Hortifruti.Data.Repository.Interfaces;
using Hortifruti.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hortifruti.Data.Repository;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Produto>> ObterTodasAsync()
    {
        return await _context.Produto.ToListAsync();
    }

    public async Task<Produto?> ObterPorIdAsync(int id)
    {
        return await _context.Produto.FindAsync(id);
    }

    public async Task<Produto> AdicionarAsync(Produto produto)
    {
        _context.Produto.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task AtualizarAsync(Produto produto)
    {
        _context.Entry(produto).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(int id)
    {
        var produto = await ObterPorIdAsync(id);
        
        if (produto != null)
        {
            _context.Produto.Remove(produto);
            await _context.SaveChangesAsync();
        }
    }
}