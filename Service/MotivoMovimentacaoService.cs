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

    public async Task<IEnumerable<MotivoMovimentacao>> ObterTodosOsMotivosMovimentacaoAsync()
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
        motivoMovimentacao.Ativo = true;
        
        // 1. Adiciona ao contexto
        var criado = await _uow.MotivoMovimentacao.AdicionarAsync(motivoMovimentacao);
        
        // 2. Salva no banco de dados (Unit of Work)
        await _uow.SaveChangesAsync(); 

        return criado;
    }

    public async Task AtualizarMotivoMovimentacaoAsync(int id, MotivoMovimentacao motivoMovimentacao)
    {
        // Validação: Lança exceção em vez de retornar silenciosamente
        if (id != motivoMovimentacao.Id)
        {
            throw new ArgumentException("O ID informado na URL não corresponde ao ID do corpo da requisição.");
        }

        // 1. Atualiza no contexto
        await _uow.MotivoMovimentacao.AtualizarAsync(motivoMovimentacao);
        
        // 2. Salva no banco de dados
        await _uow.SaveChangesAsync();
    }

    public async Task DeletarMotivoMovimentacaoAsync(int id)
    {
        // 1. Remove do contexto
        await _uow.MotivoMovimentacao.DeletarAsync(id);
        
        // 2. Salva no banco de dados
        await _uow.SaveChangesAsync();
    }
}