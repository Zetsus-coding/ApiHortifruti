using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;

namespace ApiHortifruti.Service;

public class UnidadeMedidaService : IUnidadeMedidaService
{
    private readonly IUnityOfWork _uow;
    private readonly IMapper _mapper;

    // Construtor com injeção de dependência do Unit of Work
    public UnidadeMedidaService(IUnityOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetUnidadeMedidaDTO>> ObterTodasAsUnidadesMedidaAsync()
    {
        var unidadesMedida = await _uow.UnidadeMedida.ObterTodosAsync(); // Chamada a camada de repositório (através do Unit of Work) para obter todos
        return _mapper.Map<IEnumerable<GetUnidadeMedidaDTO>>(unidadesMedida); // Mapeia de entidade para DTO e retorna
    }

    public async Task<GetUnidadeMedidaDTO?> ObterUnidadeMedidaPorIdAsync(int id)
    {
        var unidadeMedida = await _uow.UnidadeMedida.ObterPorIdAsync(id); // Chamada a camada de repositório (através do Unit of Work) para obter por ID
        return _mapper.Map<GetUnidadeMedidaDTO?>(unidadeMedida); // Mapeia de entidade para DTO e retorna
    }

    public async Task<GetUnidadeMedidaDTO> CriarUnidadeMedidaAsync(PostUnidadeMedidaDTO postUnidadeMedidaDTO)
    {
        var unidadeMedida = _mapper.Map<UnidadeMedida>(postUnidadeMedidaDTO); // Conversão de DTO para entidade

        await _uow.UnidadeMedida.AdicionarAsync(unidadeMedida); // Chamada a camada de repositório (através do Unit of Work) para criar
        await _uow.SaveChangesAsync(); // Salva as alterações no banco de dados

        return _mapper.Map<GetUnidadeMedidaDTO>(unidadeMedida); // Mapeia de entidade para DTO e retorna
    }

    public async Task AtualizarUnidadeMedidaAsync(int id, PutUnidadeMedidaDTO putUnidadeMedidaDTO)
    {
        var unidadeMedida = _mapper.Map<UnidadeMedida>(putUnidadeMedidaDTO); // Conversão de DTO para entidade

        if (id != unidadeMedida.Id) throw new ArgumentException("O ID da unidade de medida na URL não corresponde ao ID no corpo da requisição.");

        await _uow.UnidadeMedida.AtualizarAsync(unidadeMedida); // Chamada a camada de repositório (através do Unit of Work) para atualizar
        await _uow.SaveChangesAsync(); // Salva as alterações no banco de dados
    }

    public async Task DeletarUnidadeMedidaAsync(int id)
    {
        var unidadeMedida = await _uow.UnidadeMedida.ObterPorIdAsync(id);
        if (unidadeMedida == null) throw new NotFoundException("A 'Unidade de Medida' informada na requisição não existe");

        await _uow.UnidadeMedida.DeletarAsync(unidadeMedida);
        await _uow.SaveChangesAsync();
    }
}