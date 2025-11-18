using System.ComponentModel.DataAnnotations;
using System.Data;
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

    public async Task<IEnumerable<Categoria>> ObterTodasAsCategoriasAsync()
    {
        try
        {
            return await _uow.Categoria.ObterTodosAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Categoria?> ObterCategoriaPorIdAsync([Range(1, int.MaxValue)] int id)
    {
        return await _uow.Categoria.ObterPorIdAsync(id); // Chamada a camada de repositório (através do Unit of Work) para obter por ID
    }

    public async Task<Categoria> CriarCategoriaAsync(Categoria categoria)
    {
        await _uow.Categoria.AdicionarAsync(categoria); // Chamada a camada de repositório (através do Unit of Work) para adicionar
        await _uow.SaveChangesAsync();

        return categoria;
    }

    public async Task AtualizarCategoriaAsync([Range(1, int.MaxValue)] int id, Categoria categoria) // TODO: PutCategoriaDTO
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

