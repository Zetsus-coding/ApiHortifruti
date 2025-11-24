using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IFuncionarioService
{
    Task<IEnumerable<GetFuncionarioDTO>> ObterTodosOsFuncionariosAsync();
    Task<GetFuncionarioDTO?> ObterFuncionarioPorIdAsync(int id);
    Task<GetFuncionarioDTO> CriarFuncionarioAsync(PostFuncionarioDTO postFuncionarioDTO);
    Task AtualizarFuncionarioAsync(int id, PutFuncionarioDTO putFuncionarioDTO);
    Task DeletarFuncionarioAsync(int id);
}
