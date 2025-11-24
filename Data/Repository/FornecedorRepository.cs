using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApiHortifruti.Data.Repository;

public class FornecedorRepository : IFornecedorRepository
{
    private readonly AppDbContext _context;

    public FornecedorRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Fornecedor>> ObterTodosAsync()
    {
        return await _context.Fornecedor.ToListAsync();
    }

    public async Task<Fornecedor?> ObterPorIdAsync(int id)
    {
        return await _context.Fornecedor.FindAsync(id);
    }

    public async Task<Fornecedor> ObterFornecedorComListaDeProdutosAtravesDeFornecedorIdAsync(int fornecedorId)
    {
        var fornecedor = await _context.Fornecedor                              // 1) Inicia a consulta na tabela de 'Fornecedor'
                            .Include(f => f.FornecedorProduto)                  // 2) Inclui a tabela de associação
                                .ThenInclude(fp => fp.Produto)                  // 3) A partir da associação, inclui 'Produto'
                                    .ThenInclude(p => p.Categoria)              // 4) A partir do Produto, inclui a 'Categoria'
                            .Include(f => f.FornecedorProduto)                  // 5) Inicia um novo caminho de inclusão a partir do 'Fornecedor'
                                .ThenInclude(fp => fp.Produto)                  // 6) Inclui novamente a associação e o 'Produto'
                                    .ThenInclude(p => p.UnidadeMedida)          // 7) A partir do Produto, inclui a 'Unidade de Medida'
                            .FirstOrDefaultAsync(f => f.Id == fornecedorId);    // 8) Filtra pelo ID do fornecedor

        return fornecedor;
    }

    public async Task<Fornecedor> AdicionarAsync(Fornecedor fornecedor)
    {
        _context.Fornecedor.Add(fornecedor);
        return fornecedor;
    }

    public async Task AtualizarAsync(Fornecedor fornecedor)
    {
        _context.Entry(fornecedor).State = EntityState.Modified;
    }

    public async Task DeletarAsync(Fornecedor fornecedor)
    {
        _context.Fornecedor.Remove(fornecedor);
    }

    public async Task<Fornecedor?> ObterPorIdComProdutosAsync(int id)
    {
        return await _context.Fornecedor
            .Include(f => f.FornecedorProduto)
            .ThenInclude(fp => fp.Produto)
            .ThenInclude(p => p.Categoria)
            .Include(f => f.FornecedorProduto)
            .ThenInclude(fp => fp.Produto)
            .ThenInclude(p => p.UnidadeMedida)
            .FirstOrDefaultAsync(f => f.Id == id);
    }
}