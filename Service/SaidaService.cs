using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class SaidaService : ISaidaService
{
    private readonly ISaidaRepository _entradaRepository;

    public SaidaService(ISaidaRepository entradaRepository)
    {
        _entradaRepository = entradaRepository; // Inj. dependência
    }

    public async Task<IEnumerable<Saida>> ObterTodasSaidasAsync()
    {
        return await _entradaRepository.ObterTodasAsync();
    }

    public async Task<Saida?> ObterSaidaPorIdAsync(int id)
    {
        return await _entradaRepository.ObterPorIdAsync(id);
        
    }

    public async Task<Saida> CriarSaidaAsync(Saida entrada)
    {
        return await _entradaRepository.AdicionarAsync(entrada);
    }

    public async Task AtualizarSaidaAsync(int id, Saida entrada)
    {
        if (id != entrada.Id)
        {
            // Lançar erro/exceção
            return;
        }
        await _entradaRepository.AtualizarAsync(entrada);
    }

    public async Task DeletarSaidaAsync(int id)
    {
        await _entradaRepository.DeletarAsync(id);
    }
}

