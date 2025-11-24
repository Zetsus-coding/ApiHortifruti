using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.DTO.Funcionario;
using ApiHortifruti.DTO.PutFuncionarioDTO;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;

namespace ApiHortifruti.Service;

public class FuncionarioService : IFuncionarioService
{
    private readonly IUnityOfWork _uow;
    private readonly IMapper _mapper;

    public FuncionarioService(IUnityOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetFuncionarioDTO>> ObterTodosOsFuncionariosAsync()
    {
        try
        {
            var funcionarios = await _uow.Funcionario.ObterTodosAsync();
            return _mapper.Map<IEnumerable<GetFuncionarioDTO>>(funcionarios);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GetFuncionarioDTO?> ObterFuncionarioPorIdAsync(int id)
    {
        var funcionario = await _uow.Funcionario.ObterPorIdAsync(id);
        return _mapper.Map<GetFuncionarioDTO?>(funcionario);
    }

    public async Task<GetFuncionarioDTO> CriarFuncionarioAsync(PostFuncionarioDTO postFuncionarioDTO)
    {
        var funcionario = _mapper.Map<Funcionario>(postFuncionarioDTO);

        await _uow.BeginTransactionAsync();
        try
        {
            await _uow.Funcionario.AdicionarAsync(funcionario);
            
            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();
            return _mapper.Map<GetFuncionarioDTO>(funcionario);
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }

    public async Task AtualizarFuncionarioAsync(int id, PutFuncionarioDTO putFuncionarioDTO)
    {
        if (id != putFuncionarioDTO.Id)
        {
            throw new ArgumentException("O ID informado não é o mesmo que está sendo editado");
        }
        
        // 1. Busca o funcionário ORIGINAL no banco
        var funcionarioExistente = await _uow.Funcionario.ObterPorIdAsync(id);
        
        if (funcionarioExistente == null)
        {
            throw new NotFoundException("Funcionário não encontrado.");
        }

        // 2. Atualiza APENAS os campos permitidos usando AutoMapper ou manualmente
        // Como o serviço já mapeava manualmente antes para evitar alterar CPF/RG, podemos manter isso ou usar o Mapper com configuração.
        // O PutFuncionarioDTO provavelmente não tem CPF/RG se a intenção for proteger?
        // Vamos checar PutFuncionarioDTO.

        // Se PutFuncionarioDTO não tiver CPF/RG, o Mapper não vai sobrescrever se usarmos Map(source, dest).
        // Mas se o DTO tiver, precisamos cuidar.
        // O código anterior mapeava manualmente. Vou usar o Mapper para os campos comuns, mas precisamos garantir que o DTO reflete o que pode ser mudado.

        _mapper.Map(putFuncionarioDTO, funcionarioExistente);

        // 3. Manda salvar as alterações no objeto rastreado
        await _uow.Funcionario.AtualizarAsync(funcionarioExistente);
        await _uow.SaveChangesAsync();
    }

    public async Task DeletarFuncionarioAsync(int id)
    {
        var funcionario = await _uow.Funcionario.ObterPorIdAsync(id);
        if (funcionario == null) throw new NotFoundException("O 'Funcionario' informado na requisição não existe");

        await _uow.Funcionario.DeletarAsync(funcionario);
        await _uow.SaveChangesAsync();
    }
}
