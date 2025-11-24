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
        //O FindAsync sozinho NÃO traz os dados do Fornecedor nem do Motivo.
        //Usa o Include para carregar as tabelas relacionadas.
        
        return await _context.Entrada
            .Include(e => e.Fornecedor)           // <--- Carrega o Fornecedor para o DTO pegar o NomeFantasia
            .Include(e => e.MotivoMovimentacao)   // <--- Carrega o Motivo para o DTO pegar a descrição
            .Include(e => e.ItemEntrada)          // <--- Opcional: Carrega os itens caso precise no futuro
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<decimal> ObterValorTotalPorPeriodoAsync(DateOnly dataInicio, DateOnly dataFim)
    {
        return await _context.Entrada
            .Where(e => e.DataCompra >= dataInicio && e.DataCompra <= dataFim)
            .SumAsync(e => e.PrecoTotal);
    }
    public async Task<IEnumerable<Entrada>> ObterRecentesAsync(DateOnly dataInicio)
    {
        return await _context.Entrada
        .AsNoTracking()
        .Include(e => e.Fornecedor)
        
        // AQUI ESTÁ A MUDANÇA: Filtra tudo que for MAIOR ou IGUAL a data de início
        .Where(e => e.DataCompra >= dataInicio) 
        .OrderByDescending(e => e.DataCompra)
        .ThenByDescending(e => e.Id)
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