using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;

public class FornecedorProdutoService : IFornecedorProdutoService
{
    private readonly IUnityOfWork _uow;
    private readonly IDateTimeProvider _dateTimeProvider;

    public FornecedorProdutoService(IUnityOfWork uow, IDateTimeProvider dateTimeProvider)
    {
        _uow = uow;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<IEnumerable<FornecedorProduto>> ObterTodosOsFornecedorProdutoAsync()
    {
            return await _uow.FornecedorProduto.ObterTodosAsync();
    }

    public async Task<FornecedorProduto?> ObterFornecedorProdutoPorIdAsync(int fornecedorId, int produtoId)
    {
        return await _uow.FornecedorProduto.ObterPorIdAsync(fornecedorId, produtoId);
    }

    public async Task<Fornecedor> ObterFornecedorComProdutosAsync(int id)
    {
        // Este método usa o repositório que faz os .Include()
        var fornecedor = await _uow.Fornecedor.ObterPorIdComProdutosAsync(id);
        
        if (fornecedor == null) throw new NotFoundException("O 'Fornecedor' informado na requisição não existe");

        return fornecedor;
    }

    public async Task<FornecedorProduto> CriarFornecedorProdutoAsync(FornecedorProduto fornecedorProduto)
    {
        await _uow.BeginTransactionAsync();

        try
        {
            var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(fornecedorProduto.FornecedorId);
            if (fornecedor == null) throw new KeyNotFoundException("Fornecedor não encontrado.");
            
            var produto = await _uow.Produto.ObterPorIdAsync(fornecedorProduto.ProdutoId);
            if (produto == null) throw new KeyNotFoundException("Produto não encontrado.");
            
            var existente = await _uow.FornecedorProduto.ObterPorIdAsync(fornecedorProduto.FornecedorId, fornecedorProduto.ProdutoId);
            if (existente != null) throw new InvalidOperationException("A relação entre o fornecedor e o produto já existe."); // Mudado para InvalidOperationException (mais semântico para conflito)

            fornecedorProduto.Disponibilidade = true;
            fornecedorProduto.DataRegistro = _dateTimeProvider.Today; // Usando provider

            var novoFornecedorProduto = await _uow.FornecedorProduto.AdicionarAsync(fornecedorProduto);

            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();

            return novoFornecedorProduto;
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<FornecedorProduto>> CriarVariosFornecedorProdutosAsync(IEnumerable<FornecedorProduto> fornecedorProdutos)
    {
        if (fornecedorProdutos == null || !fornecedorProdutos.Any())
            throw new ArgumentException("A lista de associações não pode ser nula ou vazia.");

        var duplicados = fornecedorProdutos
            .GroupBy(fp => new { fp.FornecedorId, fp.ProdutoId })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicados.Any())
            throw new InvalidOperationException("Existem associações duplicadas na lista fornecida.");

        await _uow.BeginTransactionAsync();

        try
        {
            foreach (var fornProd in fornecedorProdutos)
            {
                // Nota: Fazer consultas dentro de loop pode ser lento, mas para manter a lógica original:
                var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(fornProd.FornecedorId);
                var produto = await _uow.Produto.ObterPorIdAsync(fornProd.ProdutoId);
                var existente = await _uow.FornecedorProduto.ObterPorIdAsync(fornProd.FornecedorId, fornProd.ProdutoId);

                if (fornecedor == null) throw new KeyNotFoundException($"Fornecedor não encontrado.");
                if (produto == null) throw new KeyNotFoundException($"Produto não encontrado.");
                if (existente != null) throw new InvalidOperationException($"A relação para o fornecedorId {fornProd.FornecedorId} e produtoId {fornProd.ProdutoId} já existe.");

                fornProd.Disponibilidade = true;
                fornProd.DataRegistro = _dateTimeProvider.Today; // Usando provider
            }

            await _uow.FornecedorProduto.AdicionarVariosAsync(fornecedorProdutos);
            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();
            return fornecedorProdutos;
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }

    public async Task AtualizarFornecedorProdutoAsync(int fornecedorId, int produtoId, FornecedorProduto fornecedorProduto)
    {
        if (fornecedorId != fornecedorProduto.FornecedorId || produtoId != fornecedorProduto.ProdutoId)
        {
            throw new ArgumentException("Os IDs da relação informados na URL não correspondem ao corpo da requisição.");
        }
        
        await _uow.FornecedorProduto.AtualizarAsync(fornecedorProduto);
        await _uow.SaveChangesAsync(); // Adicionado
    }

    public async Task DeletarFornecedorProdutoAsync(int fornecedorId, int produtoId)
    {
        await _uow.FornecedorProduto.DeletarAsync(fornecedorId, produtoId);
        await _uow.SaveChangesAsync(); // Adicionado
    }
}