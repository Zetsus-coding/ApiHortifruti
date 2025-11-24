using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
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

    public async Task AtualizarFuncionarioAsync(int id, Funcionario funcionarioDadosNovos)
    {
        if (id != funcionarioDadosNovos.Id)
        {
            throw new ArgumentException("O ID informado não é o mesmo que está sendo editado");
        }
        
        // 1. Busca o funcionário ORIGINAL no banco (com CPF e RG preenchidos)
        var funcionarioExistente = await _uow.Funcionario.ObterPorIdAsync(id);
        
        if (funcionarioExistente == null)
        {
            throw new NotFoundException("Funcionário não encontrado.");
        }

        // 2. Atualiza APENAS os campos permitidos
        // O Entity Framework vai rastrear essas mudanças automaticamente
        funcionarioExistente.Nome = funcionarioDadosNovos.Nome;
        funcionarioExistente.Telefone = funcionarioDadosNovos.Telefone;
        funcionarioExistente.TelefoneExtra = funcionarioDadosNovos.TelefoneExtra;
        funcionarioExistente.Email = funcionarioDadosNovos.Email;
        funcionarioExistente.ContaBancaria = funcionarioDadosNovos.ContaBancaria;
        funcionarioExistente.AgenciaBancaria = funcionarioDadosNovos.AgenciaBancaria;
        funcionarioExistente.Ativo = funcionarioDadosNovos.Ativo;

        // IMPORTANTE: NÃO mexemos no funcionarioExistente.Cpf nem no Rg.
        // Eles continuam com os valores antigos do banco.

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