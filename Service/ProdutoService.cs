using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;

public class ProdutoService : IProdutoService
{
    private readonly IUnityOfWork _uow;

    // Construtor com injeção de dependência do repositório
    public ProdutoService(IUnityOfWork uow)
    {
        _uow = uow;
    }

    // Consulta de todos os produtos
    public async Task<IEnumerable<Produto>> ObterTodosOsProdutosAsync()
    {
        return await _uow.Produto.ObterTodosAsync();
    }

    // Consulta de produto por ID
    public async Task<Produto?> ObterProdutoPorIdAsync(int id)
    {
        return await _uow.Produto.ObterPorIdAsync(id); // Chamada a camada de repositório (através do Unit of Work) para consultar por ID    
    }

    // Consulta de produto por código
    public async Task<Produto?> ObterProdutoPorCodigoAsync(string codigo)
    {
        return await _uow.Produto.ObterProdutoPorCodigoAsync(codigo); // Chamada a camada de repositório (através do Unit of Work) para consultar por código
    }

    // Consulta de produtos com estoque crítico
    public async Task<IEnumerable<Produto>> ObterProdutosEstoqueCriticoAsync()
    {
        return await _uow.Produto.ObterEstoqueCriticoAsync();
    }

    // Consulta de todos os produtos que um fornecedor fornece
    public async Task<IEnumerable<FornecedorProduto>> ObterProdutosPorFornecedorIdAsync(int fornecedorId)
    {
        var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(fornecedorId);
        if (fornecedor is null) throw new NotFoundExeption("O 'Fornecedor' informado na requisição não existe");

        return await _uow.FornecedorProduto.ObterProdutosPorFornecedorIdAsync(fornecedorId);
    }

    // Inserção de um novo produto
    public async Task<Produto> CriarProdutoAsync(Produto produto)
    {
        try
        {
            var categoria = await _uow.Categoria.ObterPorIdAsync(produto.CategoriaId); // Consulta de categoria por id
            if (categoria is null) throw new KeyNotFoundException("A categoria informada não existe."); // Valida se a categoria existe

            var unidadeMedida = await _uow.UnidadeMedida.ObterPorIdAsync(produto.UnidadeMedidaId); // Consulta de unidade de medida por id
            if (unidadeMedida is null) throw new KeyNotFoundException("A unidade de medida informada não existe."); // Valida se a unidade de medida existe

            var codigoExistente = await _uow.Produto.ObterProdutoPorCodigoAsync(produto.Codigo); // Consulta de produto por código
            if (codigoExistente is not null) throw new ArgumentException("Esse código de produto já existe."); // Valida se o código do produto já existe

            await _uow.BeginTransactionAsync();
            await _uow.Produto.AdicionarAsync(produto); // Chamada a camada de repositório (através do Unit of Work) para adicionar o produto

            // Cria um registro em historicoProduto com o preço inicial (informado no momento da criação do novo produto)
            await _uow.HistoricoProduto.AdicionarAsync(new HistoricoProduto
            {
                ProdutoId = produto.Id,
                PrecoProduto = produto.Preco,
                DataAlteracao = DateOnly.FromDateTime(DateTime.Now),
            });

            await _uow.SaveChangesAsync(); // Salva as alterações
            await _uow.CommitAsync(); // Confirma a transação
            return produto;
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }

    // Atualização de um produto existente
    public async Task AtualizarProdutoAsync(int id, Produto produto)
    {
        await _uow.BeginTransactionAsync();

        try
        {
            if (id != produto.Id) throw new ArgumentException("O ID do produto na URL não corresponde ao ID no corpo da requisição.");

            var produtoExistente = await _uow.Produto.ObterPorIdAsync(id);
            if (produtoExistente is null) throw new KeyNotFoundException("Produto não encontrado."); // Valida se o produto existe

            await _uow.Produto.AtualizarAsync(produto);

            // Cria um registro em historicoProduto com novo preço, se o preço foi alterado
            if (produtoExistente.Preco != produto.Preco)
            {
                await _uow.HistoricoProduto.AdicionarAsync(new HistoricoProduto
                {
                    ProdutoId = produto.Id,
                    PrecoProduto = produto.Preco,
                    DataAlteracao = DateOnly.FromDateTime(DateTime.Now),
                });
            }

            await _uow.SaveChangesAsync(); // Salva as alterações
            await _uow.CommitAsync(); // Confirma a transação
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }

    public async Task DeletarProdutoAsync(int id)
    {
        var produto = await _uow.Produto.ObterPorIdAsync(id);
        if (produto == null) throw new NotFoundExeption("O 'Produto' informado na requisição não existe");

        await _uow.Produto.DeletarAsync(produto);
        await _uow.SaveChangesAsync();
    }
}
