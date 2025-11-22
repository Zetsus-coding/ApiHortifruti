using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApiHortifruti.Data.Repository;

public class UnidadeMedidaRepository : IUnidadeMedidaRepository
{
    private readonly AppDbContext _context;

    public UnidadeMedidaRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<UnidadeMedida>> ObterTodosAsync()
    {
        return await _context.UnidadeMedida.ToListAsync();
    }

    public async Task<UnidadeMedida?> ObterPorIdAsync(int id)
    {
        return await _context.UnidadeMedida.FindAsync(id);
    }

    public async Task<UnidadeMedida> AdicionarAsync(UnidadeMedida unidadeMedida)
    {
        await _context.UnidadeMedida.AddAsync(unidadeMedida);
        return unidadeMedida;
    }

    public async Task AtualizarAsync(UnidadeMedida unidadeMedida)
    {
        _context.Entry(unidadeMedida).State = EntityState.Modified;
    }

    public async Task DeletarAsync(UnidadeMedida unidadeMedida)
    {
        if (unidadeMedida == null) throw new KeyNotFoundException("Unidade de medida informada na requisição não existe.");

        if (unidadeMedida != null)
        {
            _context.UnidadeMedida.Remove(unidadeMedida);
        }
    }
}