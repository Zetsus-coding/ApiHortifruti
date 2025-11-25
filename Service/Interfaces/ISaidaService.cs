using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface ISaidaService
{
    Task<IEnumerable<GetSaidaSimplesDTO>> ObterTodasAsSaidasAsync();
    Task<GetSaidaDTO?> ObterSaidaPorIdAsync(int id);
    Task<GetSaidaDTO> CriarSaidaAsync(PostSaidaDTO postSaidaDTO);
}
