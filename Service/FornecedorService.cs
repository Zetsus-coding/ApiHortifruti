using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;

namespace ApiHortifruti.Service;

public class FornecedorService : IFornecedorService
{
    private readonly IUnityOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public FornecedorService( IUnityOfWork uow, IMapper mapper, IDateTimeProvider dateTimeProvider)
    {
        _uow = uow;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<IEnumerable<GetFornecedorDTO>> ObterTodosOsFornecedoresAsync()
    {
        var fornecedores = await _uow.Fornecedor.ObterTodosAsync(); // Acessa a camada (através do Unit of Work) repository para fazer a consulta
        return _mapper.Map<IEnumerable<GetFornecedorDTO>>(fornecedores); // Mapeia de entidade para DTO e retorna
    }

    public async Task<GetFornecedorDTO?> ObterFornecedorPorIdAsync(int id)
    {
        var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(id); // Acessa a camada (através do Unit of Work) repository para fazer a consulta
        return _mapper.Map<GetFornecedorDTO>(fornecedor); // Mapeia de entidade para DTO e retorna
    }

    // Consulta de todos os produtos que um fornecedor fornece
    public async Task<FornecedorComListaProdutosDTO> ObterProdutosPorFornecedorIdAsync(int fornecedorId)
    {
        var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(fornecedorId);
        if (fornecedor is null) throw new NotFoundException("O 'Fornecedor' informado na requisição não existe");
        
        var fornecedorComProdutos = await _uow.Fornecedor.ObterFornecedorComListaDeProdutosAtravesDeFornecedorIdAsync(fornecedorId);
        return _mapper.Map<FornecedorComListaProdutosDTO>(fornecedorComProdutos);
    }

    // Inserção de um novo fornecedor
    public async Task<GetFornecedorDTO> CriarFornecedorAsync(PostFornecedorDTO postFornecedorDTO)
    {
        var fornecedor = _mapper.Map<Fornecedor>(postFornecedorDTO); // Mapeia de DTO para entidade

        // Define valores padrão necessários
        fornecedor.DataRegistro = DateOnly.FromDateTime(DateTime.Now); // Define a data de registro usando o provedor de data ou a data atual
        fornecedor.Ativo = true;  // Define o fornecedor como ativo por padrão

        await _uow.Fornecedor.AdicionarAsync(fornecedor); // Adiciona o fornecedor ao repositório (context)
        await _uow.SaveChangesAsync(); // Salva as alterações no banco de dados

        return _mapper.Map<GetFornecedorDTO>(fornecedor); // Mapeia de entidade para DTO e retorna
    }

    // Atualização de um fornecedor existente
    public async Task AtualizarFornecedorAsync(int id, PutFornecedorDTO putFornecedorDTO)
    {
        if (id != putFornecedorDTO.Id) throw new ArgumentException("O ID do fornecedor na URL não corresponde ao ID no corpo da requisição.");

        var fornecedor = _mapper.Map<Fornecedor>(putFornecedorDTO);
        await _uow.Fornecedor.AtualizarAsync(fornecedor);
        await _uow.SaveChangesAsync();
    }

    // Exclusão de um fornecedor existente
    public async Task DeletarFornecedorAsync(int id)
    {
        var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(id);
        if (fornecedor == null) throw new NotFoundException("O 'Fornecedor' informado na requisição não existe");

        await _uow.Fornecedor.DeletarAsync(fornecedor);
        await _uow.SaveChangesAsync();
    }
}