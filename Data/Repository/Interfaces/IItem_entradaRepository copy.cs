using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface IItem_saidaRepository
{
    Task<IEnumerable<Item_saida>> ObterTodasAsync();
    Task<Item_saida?> ObterPorIdAsync(int id);
    Task<Item_saida> AdicionarAsync(Item_saida item_saida);
    Task AtualizarAsync(Item_saida item_saida);
    Task DeletarAsync(int id);
}