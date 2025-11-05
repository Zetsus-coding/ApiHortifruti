using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class ProdutoService : IProdutoService
{
    private readonly IUnityOfWork _uow;

    public ProdutoService(IUnityOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Produto>> ObterTodosProdutoAsync()
    {
        return await _uow.Produto.ObterTodosAsync();

        // É preciso exceção caso a lista esteja vazia?
        // if (!produto.Any())
        //     throw new DBConcurrencyException("Nenhum produto criado.");
    }

    public async Task<Produto?> ObterProdutoPorIdAsync(int id)
    {
        return await _uow.Produto.ObterPorIdAsync(id);

        // É preciso exceção caso o id não exista?
        // if (produto == null)
        //     throw new NotFoundExeption("Produto não existe.");        
    }

    public async Task<Produto?> ObterPorCodigoAsync(string codigo)
    {
        return await _uow.Produto.ObterPorCodigoAsync(codigo);
    }

    public async Task<Produto> CriarProdutoAsync(Produto produto)
    {   
        await _uow.BeginTransactionAsync();
        try
        {
            var categoria = await _uow.Categoria.ObterPorIdAsync(produto.CategoriaId);
            var unidadeMedida = await _uow.UnidadeMedida.ObterPorIdAsync(produto.UnidadeMedidaId);
            var codigo = await _uow.Produto.ObterPorCodigoAsync(produto.Codigo);
    
            if (categoria is null)
                throw new KeyNotFoundException("A categoria informada não existe.");
    
            if (unidadeMedida is null)
                throw new KeyNotFoundException("A unidade de medida informada não existe.");
    
            if (codigo is not null)
                throw new ArgumentException("Esse código de produto já existe.");

            await _uow.Produto.AdicionarAsync(produto);

            // TODO: Preciso criar um registro em histórico de produtos com o preço inicial
            await _uow.HistoricoProduto.AdicionarAsync(new HistoricoProduto
            {
                ProdutoId = produto.Id,
                PrecoProduto = produto.Preco,
                DataAlteracao = DateOnly.FromDateTime(DateTime.Now),
            });

            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();
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
            
            if (produtoExistente is null)
                throw new KeyNotFoundException("Produto não encontrado.");
    
            await _uow.Produto.AtualizarAsync(produto);
    
            // TODO: Preciso criar um registro em histórico de produtos com o preço inicial
            if (produtoExistente.Preco != produto.Preco)
            {
                await _uow.HistoricoProduto.AdicionarAsync(new HistoricoProduto
                {
                    ProdutoId = produto.Id,
                    PrecoProduto = produto.Preco,
                    DataAlteracao = DateOnly.FromDateTime(DateTime.Now),
                });
            }
    
            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();
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
