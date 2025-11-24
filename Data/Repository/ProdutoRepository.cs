using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApiHortifruti.Data.Repository;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }
    
    // SELECT * FROM Produto
    public async Task<IEnumerable<Produto>> ObterTodosAsync()
    {
        return await _context.Produto
            .Include(p => p.Categoria)
            .Include(p => p.UnidadeMedida)
            .ToListAsync();
    }

    // SELECT * FROM Produto WHERE Id = {id}
    public async Task<Produto?> ObterPorIdAsync(int id)
    {
        return await _context.Produto
            .Include(p => p.Categoria)
            .Include(p => p.UnidadeMedida)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    // SELECT * FROM Produto WHERE Codigo = {codigo}
    public async Task<Produto?> ObterProdutoPorCodigoAsync(string codigo)
    {
        return await _context.Produto.FirstOrDefaultAsync(p => p.Codigo == codigo);
    }

    // SELECT * FROM Produto WHERE QuantidadeAtual <= QuantidadeMinima
    public async Task<IEnumerable<Produto>> ObterEstoqueCriticoAsync()
    {
        return await _context.Produto
            .Where(p => p.QuantidadeAtual <= p.QuantidadeMinima)
            .ToListAsync();
    }

    public async Task<Produto> ObterProdutoComListaDeFornecedoresAtravesDeProdutoIdAsync(int produtoId)
    {
        var produto = await _context.Produto
                                .Include(p => p.FornecedorProduto)                  // 1) Inclui a lista de associações (FornecedorProduto)
                                    .ThenInclude(fp => fp.Fornecedor)               // 2) Para cada associação, inclui o Fornecedor correspondente
                                .Include(p => p.Categoria)                            // 4) Inclui a Categoria do Produto
                                .Include(p => p.UnidadeMedida)                       // 5) Inclui a Unidade de Medida do Produto
                                .FirstOrDefaultAsync(p => p.Id == produtoId);       // 3) Filtra pelo ID do produto
        return produto;
    }

    // INSERT INTO Produto (...)
    public async Task<Produto> AdicionarAsync(Produto produto)
    {
        _context.Produto.Add(produto);
        return produto;
    }

    // UPDATE Produto SET ... WHERE Id = {produto.Id}
    public async Task AtualizarAsync(Produto produto)
    {
        var existing = _context.Produto.Local.FirstOrDefault(p => p.Id == produto.Id);
        if (existing != null)
        {
            _context.Entry(existing).CurrentValues.SetValues(produto);
        }
        else
        {
            _context.Entry(produto).State = EntityState.Modified;
        }
    }

    // DELETE FROM Produto WHERE Id = {produto.Id}
    public async Task DeletarAsync(Produto produto)
    {   
        if (produto != null)
        {
            _context.Produto.Remove(produto);
        }
    }
}