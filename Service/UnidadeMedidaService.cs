using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;

public class UnidadeMedidaService : IUnidadeMedidaService
{
    private readonly IUnidadeMedidaRepository _unidadeMedidaRepository;

    public UnidadeMedidaService(IUnidadeMedidaRepository unidadeMedidaRepository)
    {
        _unidadeMedidaRepository = unidadeMedidaRepository;
    }

    public async Task<IEnumerable<UnidadeMedida>> ObterTodosUnidadeMedidaAsync()
    {
        return await _unidadeMedidaRepository.ObterTodosAsync();

        // É preciso exceção caso a lista esteja vazia?
        // if (!getAllUnidadeMedida.Any())
        //  throw new DBConcurrencyException("Nenhuma unidade de medida criada.");
    }

    public async Task<UnidadeMedida?> ObterUnidadeMedidaPorIdAsync(int id)
    {
        return await _unidadeMedidaRepository.ObterPorIdAsync(id);

        // É preciso exceção caso o id não exista?
        // if (getIdUnidadeMedida == null)
        // throw new NotFoundExeption("Unidade de medida não existe.");
    }

    public async Task<UnidadeMedida> CriarUnidadeMedidaAsync(UnidadeMedida unidadeMedida)
    {
        return await _unidadeMedidaRepository.AdicionarAsync(unidadeMedida);
    }

    public async Task AtualizarUnidadeMedidaAsync(int id, UnidadeMedida unidadeMedida)
    {
        if (id != unidadeMedida.Id)
        {
            throw new ArgumentException("O ID da unidade de medida na URL não corresponde ao ID no corpo da requisição.");
        }
        await _unidadeMedidaRepository.AtualizarAsync(unidadeMedida);
    }

    // public async Task DeletarUnidadeMedidaAsync(int id)
    // {
    //     await _unidadeMedidaRepository.DeletarAsync(id);
    // }
}