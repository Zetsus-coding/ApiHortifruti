using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IItemSaidaService
{
    Task AdicionarItensSaidaAsync(int saidaId, IEnumerable<ItemSaida> itens);
}