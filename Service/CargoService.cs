using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;

public class CargoService : ICargoService
{
    private readonly IUnityOfWork _uow;

    public CargoService(IUnityOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Cargo>> ObterTodosCargosAsync()
    {
        return await _uow.Cargo.ObterTodosAsync();
    }

    public async Task<Cargo?> ObterCargoPorIdAsync(int id)
    {
        return await _uow.Cargo.ObterPorIdAsync(id);
    }
}