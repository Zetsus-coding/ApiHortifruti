using Microsoft.EntityFrameworkCore;
using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository;

public class EntradaRepository : IEntradaRepository
{
    private readonly AppDbContext _context;

    public EntradaRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Entrada>> ObterTodosAsync()
    {
        return await _context.Entrada.ToListAsync();
    }

    public async Task<Entrada?> ObterPorIdAsync(int id)
    {
        return await _context.Entrada.FindAsync(id);
    }

    public async Task<decimal> ObterValorTotalPorPeriodoAsync(DateOnly dataInicio, DateOnly dataFim)
    {
        return await _context.Entrada
            .Where(e => e.DataCompra >= dataInicio && e.DataCompra < dataFim)
            .SumAsync(e => e.PrecoTotal);
    }
    public async Task<IEnumerable<Entrada>> ObterRecentesAsync()
    {
        return await _context.Entrada
            .OrderByDescending(e => e.DataCompra)
            .ToListAsync();
    }

    public async Task<Entrada?> ObterPorNumeroNotaAsync(string numeroNota, int fornecedorId)
    {
        return await _context.Entrada
            .FirstOrDefaultAsync(e => e.NumeroNota == numeroNota && e.FornecedorId == fornecedorId);
    }

    public async Task AdicionarAsync(Entrada entrada)
    {
        _context.Entrada.Add(entrada);
    }

    // public async Task AtualizarAsync(Entrada entrada)
    // {
    //     _context.Entry(entrada).State = EntityState.Modified;
    // }

    // public async Task DeletarAsync(Entrada entrada)
    // {
    //     _context.Entrada.Remove(entrada);
    // }
}