using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

// Interface para o serviço de Unidade de Medida
public interface IUnidadeMedidaService
{
    Task<IEnumerable<GetUnidadeMedidaDTO>> ObterTodasAsUnidadesMedidaAsync();
    Task<GetUnidadeMedidaDTO?> ObterUnidadeMedidaPorIdAsync(int id);
    Task<GetUnidadeMedidaDTO> CriarUnidadeMedidaAsync(PostUnidadeMedidaDTO postUnidadeMedidaDTO);
    Task AtualizarUnidadeMedidaAsync(int id, PutUnidadeMedidaDTO putUnidadeMedidaDTO);
    Task DeletarUnidadeMedidaAsync(int id);
}