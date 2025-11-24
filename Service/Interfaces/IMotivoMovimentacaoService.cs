using ApiHortifruti.Domain;
using ApiHortifruti.DTO.MotivoMovimentacao;

namespace ApiHortifruti.Service.Interfaces;

public interface IMotivoMovimentacaoService
{
    Task<IEnumerable<GetMotivoMovimentacaoDTO>> ObterTodosOsMotivosMovimentacaoAsync();
    Task<GetMotivoMovimentacaoDTO?> ObterMotivoMovimentacaoPorIdAsync(int id);
    Task<GetMotivoMovimentacaoDTO> CriarMotivoMovimentacaoAsync(PostMotivoMovimentacaoDTO postMotivoMovimentacaoDTO);
    Task AtualizarMotivoMovimentacaoAsync(int id, PutMotivoMovimentacaoDTO putMotivoMovimentacaoDTO);
    Task DeletarMotivoMovimentacaoAsync(int id);
}
