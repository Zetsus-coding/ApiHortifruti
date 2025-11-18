using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

// Interface para o serviço de Categoria
public interface ICategoriaService
{
    Task<IEnumerable<Categoria>> ObterTodasAsCategoriasAsync();
    Task<Categoria?> ObterCategoriaPorIdAsync(int id);
    Task<Categoria> CriarCategoriaAsync(Categoria categoria);
    Task AtualizarCategoriaAsync(int id, Categoria categoria);

    // Task DeletarCategoriaAsync(int id);
}