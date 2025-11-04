using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository; // Inj. dependência
    }

    public async Task<IEnumerable<Categoria>> ObterTodosCategoriasAsync()
    {
        return await _categoriaRepository.ObterTodosAsync();

        // É preciso exceção caso a lista esteja vazia?
        // if (!categoria.Any())
        //     throw new DBConcurrencyException("Nenhuma categoria criada.");
    }

    public async Task<Categoria?> ObterCategoriaPorIdAsync(int id)
    {
        return await _categoriaRepository.ObterPorIdAsync(id);

        // É preciso exceção caso o id não exista?
        // if (categoria == null) 
        //     throw new NotFoundExeption("Categoria não existe.");
    }

    public async Task<Categoria> CriarCategoriaAsync(Categoria categoria)
    {
        return await _categoriaRepository.AdicionarAsync(categoria);
    }

    public async Task AtualizarCategoriaAsync(int id, Categoria categoria)
    {
        if (id != categoria.Id)
        {
            throw new ArgumentException("O ID da categoria na URL não corresponde ao ID no corpo da requisição.");
        }
        await _categoriaRepository.AtualizarAsync(categoria);
    }

    // public async Task DeletarCategoriaAsync(int id)
    // {
    //     await _categoriaRepository.DeletarAsync(id);
    // }
}

