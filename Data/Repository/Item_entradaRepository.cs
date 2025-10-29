using Microsoft.EntityFrameworkCore;
using ApiHortifruti.Domain;
using ApiHortifruti.Data.Repository.Interfaces;

namespace ApiHortifruti.Data.Repository;

public class Item_entradaRepository : IItem_entradaRepository
{
    private readonly AppDbContext _context; // Inj. dep.

    // Construtor
    public Item_entradaRepository(AppDbContext context)
    {
        _context = context;
    }

    // Métodos da interface
    public async Task AdicionarListaItensEntradaAsync(IEnumerable<Item_entrada> itens)
    {
       await _context.Item_entrada.AddRangeAsync(itens); // Adiciona no context a "lista" de itens (ou item se só tiver 1 item na list)
    }
}