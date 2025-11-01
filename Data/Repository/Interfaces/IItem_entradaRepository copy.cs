using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface IItem_saidaRepository
{
    Task<IEnumerable<ItemSaida>> ObterTodasAsync();
    Task<ItemSaida?> ObterPorIdAsync(int id);
    Task<ItemSaida> AdicionarAsync(ItemSaida item_saida);
    Task AtualizarAsync(ItemSaida item_saida);
    Task DeletarAsync(int id);
}