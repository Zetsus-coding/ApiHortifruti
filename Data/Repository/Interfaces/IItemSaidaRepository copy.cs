using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface IItemSaidaRepository
{
    Task ObterTodosDeUmaSaidaAsync (int idSaida);
    Task AdicionarItensSaidaAsync(IEnumerable<ItemSaida> itens);
}