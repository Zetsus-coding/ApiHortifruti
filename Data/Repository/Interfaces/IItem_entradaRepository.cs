using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface IItem_entradaRepository
{
    Task<IEnumerable<Item_entrada>> ObterTodasAsync();
    Task<Item_entrada?> ObterPorIdAsync(int id);
    Task<Item_entrada> AdicionarAsync(Item_entrada item_entrada);
    Task AtualizarAsync(Item_entrada item_entrada);
    Task DeletarAsync(int id);
}