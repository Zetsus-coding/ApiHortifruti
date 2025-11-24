using ApiHortifruti.Domain;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface IItemEntradaRepository
{
    Task<IEnumerable<ItemEntrada>> ObterTodosDeUmaEntradaAsync (int idEntrada);
    
    // RESERVADO PARA OUTROS MÉTODOS (GET?) #1(?)
    // RESERVADO PARA OUTROS MÉTODOS (GET?) #2(?)
    // RESERVADO PARA OUTROS MÉTODOS (GET?) #3(?)
    // PUT E DELETE = QUESTIONÁVEIS
}

// Removido:
//Task AdicionarItensEntradaAsync(IEnumerable<ItemEntrada> itens); // Adicionar no context a lista de itens (ou item se só tiver 1 item na list)