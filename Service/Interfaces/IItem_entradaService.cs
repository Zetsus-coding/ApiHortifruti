using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IItem_entradaService
{
    Task<IEnumerable<Item_entrada>> ObterTodasItem_entradasAsync();
    Task<Item_entrada?> ObterItem_entradaPorIdAsync(int id);
    Task<Item_entrada> CriarItem_entradaAsync(Item_entrada item_entrada);
    Task AtualizarItem_entradaAsync(int id, Item_entrada item_entrada); // Provavelmente não vai existir
    Task DeletarItem_entradaAsync(int id); // Provavelmente não vai existir
}