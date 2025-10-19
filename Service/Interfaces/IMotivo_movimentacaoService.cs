using ApiHortifruti.Domain;

namespace ApiHortifruti.Service.Interfaces;

public interface IMotivo_movimentacaoService
{
    Task<IEnumerable<Motivo_movimentacao>> ObterTodosMotivo_movimentacaoAsync();
    Task<Motivo_movimentacao?> ObterMotivo_movimentacaoPorIdAsync(int id);
    Task<Motivo_movimentacao> CriarMotivo_movimentacaoAsync(Motivo_movimentacao motivo_movimentacao);
    Task AtualizarMotivo_movimentacaoAsync(int id, Motivo_movimentacao motivo_movimentacao);
    Task DeletarMotivo_movimentacaoAsync(int id);
}