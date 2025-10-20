using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IItem_saidaService
{
    Task<IEnumerable<Item_saida>> ObterTodasItem_saidasAsync();
    Task<Item_saida?> ObterItem_saidaPorIdAsync(int id);
    Task<Item_saida> CriarItem_saidaAsync(Item_saida item_saida);
    Task AtualizarItem_saidaAsync(int id, Item_saida item_saida); // Provavelmente não vai existir
    Task DeletarItem_saidaAsync(int id); // Provavelmente não vai existir
}