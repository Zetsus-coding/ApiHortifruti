using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApiHortifruti.Data.Repository;

public class CargoRepository : ICargoRepository
{
    private readonly AppDbContext _context;

    public CargoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cargo>> ObterTodosAsync()
    {
        return await _context.Cargo.ToListAsync();
    }

    public async Task<Cargo?> ObterPorIdAsync(int id)
    {
        return await _context.Cargo.FindAsync(id);
    }
}
