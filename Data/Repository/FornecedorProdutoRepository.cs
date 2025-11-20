using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApiHortifruti.Data.Repository;

public class FornecedorProdutoRepository : IFornecedorProdutoRepository
{
    private readonly AppDbContext _context;

    public FornecedorProdutoRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<FornecedorProduto>> ObterTodosAsync()
    {
        return await _context.FornecedorProduto.ToListAsync();
    }

    public async Task<FornecedorProduto?> ObterPorIdAsync(int fornecedorId, int produtoId)
    {
        return await _context.FornecedorProduto.SingleOrDefaultAsync(fp =>
            fp.FornecedorId == fornecedorId && fp.ProdutoId == produtoId);
    }

    public async Task<IEnumerable<FornecedorProduto>> ObterProdutosPorFornecedorIdAsync(int fornecedorId)
    {
        // return await _context.FornecedorProduto
        //     .Where(fp => fp.FornecedorId == fornecedorId)
        //     .Select(fp => fp.Produto) // <--- Aqui está a mágica
        //     .ToListAsync();
        throw new NotImplementedException();
    }

    public async Task<FornecedorProduto> AdicionarAsync(FornecedorProduto fornecedorProduto)
    {
        _context.FornecedorProduto.Add(fornecedorProduto);
        // await _context.SaveChangesAsync(); // Removido para ser controlado pelo UnitOfWork
        return fornecedorProduto;
    }

    public async Task<IEnumerable<FornecedorProduto>> AdicionarVariosAsync(IEnumerable<FornecedorProduto> fornecedorProdutos)
    {
        _context.FornecedorProduto.AddRange(fornecedorProdutos);
        return fornecedorProdutos;
    }

    public async Task AtualizarAsync(FornecedorProduto fornecedorProduto)
    {
        _context.Entry(fornecedorProduto).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(int fornecedorId, int produtoId)
    {
        var fornecedorProduto = await ObterPorIdAsync(fornecedorId, produtoId);

        if (fornecedorProduto != null)
        {
            _context.FornecedorProduto.Remove(fornecedorProduto);
            await _context.SaveChangesAsync();
        }
    }
}