using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

// Interface para o serviço de Unidade de Medida
public interface IUnidadeMedidaService
{
    Task<IEnumerable<UnidadeMedida>> ObterTodasAsUnidadesMedidaAsync();
    Task<UnidadeMedida?> ObterUnidadeMedidaPorIdAsync(int id);
    Task<UnidadeMedida> CriarUnidadeMedidaAsync(UnidadeMedida unidadeMedida);
    Task AtualizarUnidadeMedidaAsync(int id, UnidadeMedida unidadeMedida);

    // Task DeletarUnidadeMedidaAsync(int id);
}