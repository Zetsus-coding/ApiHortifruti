using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class FornecedorProdutoService : IFornecedorProdutoService
{
    private readonly IUnityOfWork _uow;

    public FornecedorProdutoService(IUnityOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<FornecedorProduto>> ObterTodosFornecedorProdutosAsync()
    {
        try
        {
            return await _uow.FornecedorProduto.ObterTodosAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<FornecedorProduto?> ObterFornecedorProdutoPorIdAsync(int fornecedorId, int produtoId)
    {
        return await _uow.FornecedorProduto.ObterPorIdAsync(fornecedorId, produtoId);
    }

    public async Task<FornecedorProduto> CriarFornecedorProdutoAsync(FornecedorProduto fornecedorProduto)
    {
        var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(fornecedorProduto.FornecedorId); // Consulta o fornecedor por id
        var produto = await _uow.Produto.ObterPorIdAsync(fornecedorProduto.ProdutoId); // Consulta o produto por id
        var existente = await _uow.FornecedorProduto.ObterPorIdAsync(fornecedorProduto.FornecedorId, fornecedorProduto.ProdutoId); // Consulta a relação pelos id's

        if (fornecedor == null) // Valida se o fornecedor existe
            throw new KeyNotFoundException("Fornecedor não encontrado.");

        if (produto == null) // Valida se o produto existe
            throw new KeyNotFoundException("Produto não encontrado.");

        if (existente != null) // Valida se a relação já existe
            throw new KeyNotFoundException("A relação entre o fornecedor e o produto já existe.");

        await _uow.BeginTransactionAsync(); // Inicia a transação

        try
        {
            fornecedorProduto.Disponibilidade = true; // Define a disponibilidade como verdadeira por padrão
            fornecedorProduto.DataRegistro = DateOnly.FromDateTime(DateTime.Now); // Definição da data de registro como a data atual

            var novoFornecedorProduto = await _uow.FornecedorProduto.AdicionarAsync(fornecedorProduto); // Adiciona o novo fornecedorProduto

            await _uow.SaveChangesAsync(); // Salva as alterações
            await _uow.CommitAsync(); // Confirma a transação

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

        // Verifica associações duplicadas na lista recebida como parâmetro
        var duplicados = fornecedorProdutos
            .GroupBy(fp => new { fp.FornecedorId, fp.ProdutoId })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicados.Any()) // Se houver duplicatas ([fornecedorId, produtoId]), lança uma exceção
            throw new InvalidOperationException("Existem associações duplicadas na lista fornecida.");

        await _uow.BeginTransactionAsync(); // Inicia a transação

        try
        {
            foreach (var fornProd in fornecedorProdutos)
            {
                var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(fornProd.FornecedorId);
                var produto = await _uow.Produto.ObterPorIdAsync(fornProd.ProdutoId);
                var existente = await _uow.FornecedorProduto.ObterPorIdAsync(fornProd.FornecedorId, fornProd.ProdutoId);

                if (fornecedor == null)
                    throw new KeyNotFoundException($"Fornecedor com Id {fornProd.FornecedorId} não encontrado.");

                if (produto == null)
                    throw new KeyNotFoundException($"Produto com Id {fornProd.ProdutoId} não encontrado.");

                if (existente != null)
                    throw new InvalidOperationException($"A relação para o FornecedorId {fornProd.FornecedorId} e ProdutoId {fornProd.ProdutoId} já existe.");

                fornProd.Disponibilidade = true;
                fornProd.DataRegistro = DateOnly.FromDateTime(DateTime.Now);
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
            // Lançar erro/exceção | [new argumentexception]?
            return;
        }
        await _uow.FornecedorProduto.AtualizarAsync(fornecedorProduto);
    }

    public async Task DeletarFornecedorProdutoAsync(int fornecedorId, int produtoId)
    {
        await _uow.FornecedorProduto.DeletarAsync(fornecedorId, produtoId);
    }
}
