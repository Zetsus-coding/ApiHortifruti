using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class EntradaService : IEntradaService
{
    private readonly IEntradaRepository _entradaRepository;

    public EntradaService(IEntradaRepository entradaRepository)
    {
        _entradaRepository = entradaRepository; // Inj. dependência
    }

    public async Task<IEnumerable<Entrada>> ObterTodasEntradasAsync()
    {
        return await _entradaRepository.ObterTodasAsync();
    }

    public async Task<Entrada?> ObterEntradaPorIdAsync(int id)
    {
        return await _entradaRepository.ObterPorIdAsync(id);
        
    }

    public async Task<Entrada> CriarEntradaAsync(Entrada entrada)
    {
        return await _entradaRepository.AdicionarAsync(entrada);
    }

    public async Task AtualizarEntradaAsync(int id, Entrada entrada)
    {
        if (id != entrada.Id)
        {
            // Lançar erro/exceção
            return;
        }
        await _entradaRepository.AtualizarAsync(entrada);
    }

    public async Task DeletarEntradaAsync(int id)
    {
        await _entradaRepository.DeletarAsync(id);
    }
}

