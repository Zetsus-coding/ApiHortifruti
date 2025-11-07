using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class SaidaService : ISaidaService
{
    private readonly ISaidaRepository _saidaRepository;

    private readonly IUnityOfWork _uow;

    public SaidaService(ISaidaRepository saidaRepository, IUnityOfWork uow)
    {
        _saidaRepository = saidaRepository; // Inj. dependência
        _uow = uow;
    }

    public async Task<IEnumerable<Saida>> ObterTodosSaidasAsync()
    {
        try
        {
            return await _uow.Saida.ObterTodosAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Saida?> ObterSaidaPorIdAsync(int id)
    {
        return await _uow.Saida.ObterPorIdAsync(id);
        
    }

    public async Task<Saida> CriarSaidaAsync(Saida saida)
    {
        return await _uow.Saida.AdicionarAsync(saida);
    }

    public async Task AtualizarSaidaAsync(int id, Saida saida)
    {
        if (id != saida.Id)
        {
            // Lançar erro/exceção
            return;
        }
        await _uow.Saida.AtualizarAsync(saida);
    }

    public async Task DeletarSaidaAsync(int id)
    {
        await _uow.Saida.DeletarAsync(id);
    }
}

