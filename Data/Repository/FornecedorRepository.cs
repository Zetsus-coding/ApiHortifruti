using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApiHortifruti.Data.Repository;

public class FornecedorRepository : IFornecedorRepository
{
    private readonly AppDbContext _context;

    public FornecedorRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Fornecedor>> ObterTodosAsync()
    {
        return await _context.Fornecedor.ToListAsync();
    }

    public async Task<Fornecedor?> ObterPorIdAsync(int id)
    {
        return await _context.Fornecedor.FindAsync(id);
    }

    public async Task<Fornecedor> AdicionarAsync(Fornecedor fornecedor)
    {
        _context.Fornecedor.Add(fornecedor);
        return fornecedor;
    }

    public async Task AtualizarAsync(Fornecedor fornecedor)
    {
        _context.Entry(fornecedor).State = EntityState.Modified;
        
    }

    public async Task DeletarAsync(Fornecedor fornecedor)
    {
        if (fornecedor != null)
        {
            _context.Fornecedor.Remove(fornecedor);
            await _context.SaveChangesAsync();
        }
    }
}