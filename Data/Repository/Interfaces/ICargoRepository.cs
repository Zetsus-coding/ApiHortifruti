using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface ICargoRepository
{
    Task<IEnumerable<Cargo>> ObterTodosAsync();
    Task<Cargo?> ObterPorIdAsync(int id);
}