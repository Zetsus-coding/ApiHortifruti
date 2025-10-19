using Hortifruti.Data.Repository.Interfaces;
using Hortifruti.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hortifruti.Data.Repository;

public class Historico_produtoRepository : IHistorico_produtoRepository
{
    private readonly AppDbContext _context;

    public Historico_produtoRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Historico_produto>> ObterTodasAsync()
    {
        return await _context.Historico_produto.ToListAsync();
    }

    public async Task<Historico_produto?> ObterPorIdAsync(int id)
    {
        return await _context.Historico_produto.FindAsync(id);
    }

    public async Task<Historico_produto> AdicionarAsync(Historico_produto historico_produto)
    {
        _context.Historico_produto.Add(historico_produto);
        await _context.SaveChangesAsync();
        return historico_produto;
    }

    public async Task AtualizarAsync(Historico_produto historico_produto)
    {
        _context.Entry(historico_produto).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(int id)
    {
        var historico_produto = await ObterPorIdAsync(id);
        
        if (historico_produto != null)
        {
            _context.Historico_produto.Remove(historico_produto);
            await _context.SaveChangesAsync();
        }
    }
}