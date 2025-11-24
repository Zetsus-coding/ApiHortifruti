using ApiHortifruti.Domain;
using ApiHortifruti.DTO.Saida;

namespace ApiHortifruti.Service.Interfaces;

public interface ISaidaService
{
    Task<IEnumerable<GetSaidaSimplesDTO>> ObterTodasAsSaidasAsync();
    Task<GetSaidaDTO?> ObterSaidaPorIdAsync(int id);
    Task<GetSaidaDTO> CriarSaidaAsync(PostSaidaDTO postSaidaDTO);
}
