using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> ObterTodasUsuariosAsync();
    Task<Usuario?> ObterUsuarioPorIdAsync(int id);
    Task<Usuario> CriarUsuarioAsync(Usuario usuario);
    Task AtualizarUsuarioAsync(int id, Usuario usuario);
    Task DeletarUsuarioAsync(int id);
}
