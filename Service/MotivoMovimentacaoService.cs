using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class MotivoMovimentacaoService : IMotivoMovimentacaoService
{
    private readonly IUnityOfWork _uow;

    public MotivoMovimentacaoService(IUnityOfWork uow)
    {
        
        _uow = uow;
    }

    public async Task<IEnumerable<MotivoMovimentacao>> ObterTodosMotivoMovimentacaoAsync()
    {
        try
        {
            return await _uow.MotivoMovimentacao.ObterTodosAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<MotivoMovimentacao?> ObterMotivoMovimentacaoPorIdAsync(int id)
    {
        return await _uow.MotivoMovimentacao.ObterPorIdAsync(id);
        
    }

    public async Task<MotivoMovimentacao> CriarMotivoMovimentacaoAsync(MotivoMovimentacao motivoMovimentacao)
    {
        return await _uow.MotivoMovimentacao.AdicionarAsync(motivoMovimentacao);
    }

    public async Task AtualizarMotivoMovimentacaoAsync(int id, MotivoMovimentacao motivoMovimentacao)
    {
        if (id != motivoMovimentacao.Id)
        {
            // Lançar erro/exceção
            return;
        }
        await _uow.MotivoMovimentacao.AtualizarAsync(motivoMovimentacao);
    }

    public async Task DeletarMotivoMovimentacaoAsync(int id)
    {
        await _uow.MotivoMovimentacao.DeletarAsync(id);
    }
}

