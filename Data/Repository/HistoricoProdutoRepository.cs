using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApiHortifruti.Data.Repository;

public class HistoricoProdutoRepository : IHistoricoProdutoRepository
{
    private readonly AppDbContext _context;

    public HistoricoProdutoRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<HistoricoProduto>> ObterTodosAsync()
    {
        return await _context.HistoricoProduto
            .Include(p => p.Produto)
            .ToListAsync();
    }

    public async Task<HistoricoProduto?> ObterPorIdAsync(int id)
    {
        return await _context.HistoricoProduto
            .Include(p => p.Produto)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<HistoricoProduto>> ObterPorProdutoIdAsync(int produtoId)
    {
        return await _context.HistoricoProduto
            .Include(p => p.Produto)
            .Where(hp => hp.ProdutoId == produtoId)
            .ToListAsync();
    }

    public async Task<HistoricoProduto> AdicionarAsync(HistoricoProduto historicoProduto)
    {
        _context.HistoricoProduto.Add(historicoProduto);
        return historicoProduto;
    }

    public async Task AtualizarAsync(HistoricoProduto historicoProduto)
    {
        _context.Entry(historicoProduto).State = EntityState.Modified;
    }

    public async Task DeletarAsync(int id)
    {
        var historicoProduto = await ObterPorIdAsync(id);
        
        if (historicoProduto != null)
        {
            _context.HistoricoProduto.Remove(historicoProduto);
        }
    }
}