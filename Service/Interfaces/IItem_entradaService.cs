using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IItem_entradaService
{
    Task ValidarItensEntradaAsync(int entradaId, IEnumerable<Item_entrada> itens); // Valida os itens de entrada (ex: existe o produto?, quantidade > 0?, preço >= 0?)
}