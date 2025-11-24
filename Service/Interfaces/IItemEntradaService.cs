using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IItemEntradaService
{
    Task<IEnumerable<ItemEntrada>> ObterItensEntradaPorEntradaIdAsync(int entradaId); // Obtém os itens de entrada baseado no id da entrada
}

// Removido:
//Task AdicionarItensEntradaAsync(int entradaId, IEnumerable<ItemEntrada> itens); // Valida os itens de entrada (ex: existe o produto?, quantidade > 0?, preço >= 0?)