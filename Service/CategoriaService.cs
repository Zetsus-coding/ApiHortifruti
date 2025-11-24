using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;

namespace ApiHortifruti.Service;

public class CategoriaService : ICategoriaService
{
    private readonly IUnityOfWork _uow; // Responsável por gerenciar os repositórios e acessar os métodos desses repositories
    private readonly IMapper _mapper;

    // Construtor com injeção de dependência do Unit of Work
    public CategoriaService(IUnityOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    // Consulta de todas as categorias
    public async Task<IEnumerable<GetCategoriaDTO>> ObterTodasAsCategoriasAsync()
    {
        var categorias = await _uow.Categoria.ObterTodosAsync(); // Acessa a camada (através do Unit of Work) repository para fazer a consulta
        return _mapper.Map<IEnumerable<GetCategoriaDTO>>(categorias); // Mapeia de entidade para DTO e retorna
    }

    // Consulta de categoria por ID
    public async Task<GetCategoriaDTO?> ObterCategoriaPorIdAsync(int id)
    {
        var categoria = await _uow.Categoria.ObterPorIdAsync(id); // Acessa a camada (através do Unit of Work) repository para fazer a consulta
        return _mapper.Map<GetCategoriaDTO?>(categoria); // Mapeia de entidade para DTO e retorna
    }

    // Criação de uma nova categoria
    public async Task<GetCategoriaDTO> CriarCategoriaAsync(PostCategoriaDTO postCategoriaDTO)
    {
        var categoria = _mapper.Map<Categoria>(postCategoriaDTO); // Mapeia de DTO para entidade
        
        await _uow.Categoria.AdicionarAsync(categoria); // Acessa a camada (através do Unit of Work) repository para adicionar a nova categoria no context
        await _uow.SaveChangesAsync(); // Salva as alterações no banco de dados

        return _mapper.Map<GetCategoriaDTO>(categoria); // Mapeia de entidade para DTO e retorna
    }

    // Atualização de uma categoria existente
    public async Task AtualizarCategoriaAsync(int id, PutCategoriaDTO putCategoriaDTO) // TODO: PutCategoriaDTO
    {
        if (id != putCategoriaDTO.Id) throw new ArgumentException("O ID da categoria na URL não corresponde ao ID no corpo da requisição.");
        
        var categoria = _mapper.Map<Categoria>(putCategoriaDTO); // Mapeia de DTO para entidade
        await _uow.Categoria.AtualizarAsync(categoria); // Acessa a camada (através do Unit of Work) repository para atualizar a categoria no context
        await _uow.SaveChangesAsync(); // Salva as alterações no banco de dados
    }

    // Exclusão de uma categoria existente
    public async Task DeletarCategoriaAsync(int id)
    {
        var categoria = await _uow.Categoria.ObterPorIdAsync(id); // Acessa a camada (através do Unit of Work) repository para buscar a categoria no context
        if (categoria == null) throw new NotFoundException("A 'Categoria' informada na requisição não existe"); // Verifica se a categoria existe
        
        await _uow.Categoria.DeletarAsync(categoria);
        await _uow.SaveChangesAsync(); // Salva as alterações no banco de dados
    }
}

