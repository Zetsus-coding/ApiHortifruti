using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
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

    public async Task<IEnumerable<Produto>> ObterTodosProdutoAsync()
    {
        return await _uow.Produto.ObterTodosAsync(); // Chamada a camada de repositório (através do Unit of Work) para obter todos

        // É preciso exceção caso a lista esteja vazia?
        // if (!produto.Any())
        //     throw new DBConcurrencyException("Nenhum produto criado.");
    }

    public async Task<Produto?> ObterProdutoPorIdAsync(int id)
    {
        return await _uow.Produto.ObterPorIdAsync(id); // Chamada a camada de repositório (através do Unit of Work) para obter por ID

        // É preciso exceção caso o id não exista?
        // if (produto == null)
        //     throw new NotFoundExeption("Produto não existe.");        
    }

    public async Task<Produto?> ObterPorCodigoAsync(string codigo)
    {
        return await _uow.Produto.ObterPorCodigoAsync(codigo); // Chamada a camada de repositório (através do Unit of Work) para obter por código

        // É preciso exceção caso o código não exista?
        // if (codigo == null)
    }

    public async Task<Produto> CriarProdutoAsync(Produto produto)
    {   
        var categoria = await _uow.Categoria.ObterPorIdAsync(produto.CategoriaId); // Consulta de categoria por id
        if (categoria is null) throw new KeyNotFoundException("A categoria informada não existe."); // Valida se a categoria existe
        
        var unidadeMedida = await _uow.UnidadeMedida.ObterPorIdAsync(produto.UnidadeMedidaId); // Consulta de unidade de medida por id
        if (unidadeMedida is null) throw new KeyNotFoundException("A unidade de medida informada não existe."); // Valida se a unidade de medida existe
        
        var codigoExistente = await _uow.Produto.ObterPorCodigoAsync(produto.Codigo); // Consulta de produto por código
        if (codigoExistente is not null) throw new ArgumentException("Esse código de produto já existe."); // Valida se o código do produto já existe

        await _uow.BeginTransactionAsync();
        try
        {
            await _uow.Produto.AdicionarAsync(produto); // Chamada a camada de repositório (através do Unit of Work) para adicionar o produto

            // Cria um registro em historicoProduto com o preço inicial
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

    public async Task AtualizarProdutoAsync(int id, Produto produto)
    {
        await _uow.BeginTransactionAsync();

        try
        {
            if (id != produto.Id)
            {
                throw new ArgumentException("O ID do produto na URL não corresponde ao ID no corpo da requisição.");
            }
    
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

    // public async Task DeletarProdutoAsync(int id)
    // {
    //     await _produtoRepository.DeletarAsync(id);
    // }
}
