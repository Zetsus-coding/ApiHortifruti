using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IItemSaidaService
{
    Task<IEnumerable<ItemSaida>> ObterTodasItem_saidasAsync();
    Task<ItemSaida?> ObterItem_saidaPorIdAsync(int id);
    Task<ItemSaida> CriarItem_saidaAsync(ItemSaida item_saida);
    Task AtualizarItem_saidaAsync(int id, ItemSaida item_saida); // Provavelmente não vai existir
    Task DeletarItem_saidaAsync(int id); // Provavelmente não vai existir
}