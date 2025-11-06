using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class CategoriaService : ICategoriaService
{
    private readonly IUnityOfWork _uow;

    // Construtor com injeção de dependência do Unit of Work
    public CategoriaService(IUnityOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Categoria>> ObterTodosCategoriasAsync()
    {
        return await _uow.Categoria.ObterTodosAsync(); // Chamada a camada de repositório (através do Unit of Work) para obter todos

        // É preciso exceção caso a lista esteja vazia?
        // if (!categoria.Any())
        //     throw new DBConcurrencyException("Nenhuma categoria criada.");
    }

    public async Task<Categoria?> ObterCategoriaPorIdAsync(int id)
    {
        return await _uow.Categoria.ObterPorIdAsync(id); // Chamada a camada de repositório (através do Unit of Work) para obter por ID

        // É preciso exceção caso o id não exista?
        // if (categoria == null) 
        //     throw new NotFoundExeption("Categoria não existe.");
    }

    public async Task<Categoria> CriarCategoriaAsync(Categoria categoria)
    {  
        await _uow.Categoria.AdicionarAsync(categoria); // Chamada a camada de repositório (através do Unit of Work) para adicionar
        await _uow.SaveChangesAsync();

        return categoria;
    }

    public async Task AtualizarCategoriaAsync(int id, Categoria categoria)
    {
        if (id != categoria.Id)
        {
            throw new ArgumentException("O ID da categoria na URL não corresponde ao ID no corpo da requisição.");
        }
        await _uow.Categoria.AtualizarAsync(categoria); // Chamada a camada de repositório (através do Unit of Work) para atualizar
        await _uow.SaveChangesAsync();
    }

    // public async Task DeletarCategoriaAsync(int id)
    // {
    //     await _uow.Categoria.DeletarAsync(id);
    // }
}

