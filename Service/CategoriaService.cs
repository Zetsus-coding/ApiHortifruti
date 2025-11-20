using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class CategoriaService : ICategoriaService
{
    private readonly IUnityOfWork _uow; // Responsável por gerenciar os repositórios e acessar os métodos desses repositories

    // Construtor com injeção de dependência do Unit of Work
    public CategoriaService(IUnityOfWork uow)
    {
        _uow = uow;
    }

    // Consulta de todas as categorias
    public async Task<IEnumerable<Categoria>> ObterTodasAsCategoriasAsync()
    {
        return await _uow.Categoria.ObterTodosAsync();
    }

    // Consulta de categoria por ID
    public async Task<Categoria?> ObterCategoriaPorIdAsync([Range(1, int.MaxValue)] int id)
    {
        return await _uow.Categoria.ObterPorIdAsync(id);
    }

    // Criação de uma nova categoria
    public async Task<Categoria> CriarCategoriaAsync(Categoria categoria)
    {
        await _uow.Categoria.AdicionarAsync(categoria);
        await _uow.SaveChangesAsync();

        return categoria;
    }

    // Atualização de uma categoria existente
    public async Task AtualizarCategoriaAsync([Range(1, int.MaxValue)] int id, Categoria categoria) // TODO: PutCategoriaDTO
    {
        if (id != categoria.Id) throw new ArgumentException("O ID da categoria na URL não corresponde ao ID no corpo da requisição.");
        
        await _uow.Categoria.AtualizarAsync(categoria);
        await _uow.SaveChangesAsync();
    }

    // Exclusão de uma categoria existente
    public async Task DeletarCategoriaAsync(int id)
    {
        var categoria = await _uow.Categoria.ObterPorIdAsync(id);
        if (categoria == null) throw new NotFoundExeption("A 'Categoria' informada na requisição não existe");
        
        await _uow.Categoria.DeletarAsync(categoria);
        await _uow.SaveChangesAsync();
    }
}

