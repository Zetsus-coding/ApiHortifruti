using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;

public class FuncionarioService : IFuncionarioService
{
    private readonly IUnityOfWork _uow;

    public FuncionarioService(IUnityOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Funcionario>> ObterTodosOsFuncionariosAsync()
    {
        try
        {
            return await _uow.Funcionario.ObterTodosAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Funcionario?> ObterFuncionarioPorIdAsync(int id)
    {
        return await _uow.Funcionario.ObterPorIdAsync(id);
    }

    public async Task<Funcionario> CriarFuncionarioAsync(Funcionario funcionario)
    {

        await _uow.BeginTransactionAsync();
        try
        {
            await _uow.Funcionario.AdicionarAsync(funcionario);
            
            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();
            return funcionario;
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }

    public async Task AtualizarFuncionarioAsync(int id, Funcionario funcionario)
    {
        if (id != funcionario.Id)
        {
            throw new ArgumentException("O ID informado não é o mesmo que está sendo editado");
        }
        
        await _uow.Funcionario.AtualizarAsync(funcionario);
        await _uow.SaveChangesAsync();
    }

    public async Task DeletarFuncionarioAsync(int id)
    {
        var funcionario = await _uow.Funcionario.ObterPorIdAsync(id);
        if (funcionario == null) throw new NotFoundExeption("O 'Funcionario' informado na requisição não existe");

        await _uow.Funcionario.DeletarAsync(funcionario);
        await _uow.SaveChangesAsync();
    }
}