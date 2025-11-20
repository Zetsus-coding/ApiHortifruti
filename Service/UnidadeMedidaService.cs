using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;

public class UnidadeMedidaService : IUnidadeMedidaService
{
    private readonly IUnityOfWork _uow;

    // Construtor com injeção de dependência do Unit of Work
    public UnidadeMedidaService(IUnityOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<UnidadeMedida>> ObterTodasAsUnidadesMedidaAsync()
    {
        try
        {
            return await _uow.UnidadeMedida.ObterTodosAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<UnidadeMedida?> ObterUnidadeMedidaPorIdAsync([Range(1, int.MaxValue)] int id)
    {
        return await _uow.UnidadeMedida.ObterPorIdAsync(id); // Chamada a camada de repositório (através do Unit of Work) para obter por ID
    }

    public async Task<UnidadeMedida> CriarUnidadeMedidaAsync(UnidadeMedida unidadeMedida)
    {
        await _uow.UnidadeMedida.AdicionarAsync(unidadeMedida); // Chamada a camada de repositório (através do Unit of Work) para criar
        await _uow.SaveChangesAsync();

        return unidadeMedida;
    }

    public async Task AtualizarUnidadeMedidaAsync([Range(1, int.MaxValue)] int id, UnidadeMedida unidadeMedida)
    {
        if (id != unidadeMedida.Id)
        {
            throw new ArgumentException("O ID da unidade de medida na URL não corresponde ao ID no corpo da requisição.");
        }

        await _uow.UnidadeMedida.AtualizarAsync(unidadeMedida); // Chamada a camada de repositório (através do Unit of Work) para atualizar
        await _uow.SaveChangesAsync();
    }

    public async Task DeletarUnidadeMedidaAsync(int id)
    {
        var unidadeMedida = await _uow.UnidadeMedida.ObterPorIdAsync(id);
        if (unidadeMedida == null) throw new NotFoundExeption("A 'Unidade de Medida' informada na requisição não existe");

        await _uow.UnidadeMedida.DeletarAsync(unidadeMedida);
        await _uow.SaveChangesAsync();
    }
}