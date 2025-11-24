using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

// Interface para o serviço de Categoria
public interface ICategoriaService
{
    Task<IEnumerable<GetCategoriaDTO>> ObterTodasAsCategoriasAsync(); // Recebe GetCategoriaDTO e retorna lista de GetCategoriaDTO
    Task<GetCategoriaDTO?> ObterCategoriaPorIdAsync(int id); // Recebe id e retorna GetCategoriaDTO
    Task<GetCategoriaDTO> CriarCategoriaAsync(PostCategoriaDTO postCategoriaDTO); // Recebe PostCategoriaDTO e retorna GetCategoriaDTO
    Task AtualizarCategoriaAsync(int id, PutCategoriaDTO putCategoriaDTO); // Recebe id e PutCategoriaDTO para atualizar uma categoria existente
    Task DeletarCategoriaAsync(int id); // Recebe id para deletar uma categoria existente
}