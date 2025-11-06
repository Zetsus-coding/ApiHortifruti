using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface ICargoService
{
    Task<IEnumerable<Cargo>> ObterTodosCargosAsync();
    Task<Cargo?> ObterCargoPorIdAsync(int id);
}