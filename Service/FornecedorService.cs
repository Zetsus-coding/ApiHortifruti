using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;

public class FornecedorService : IFornecedorService
{
    private readonly IFornecedorRepository _fornecedorRepository;

    public FornecedorService(IFornecedorRepository fornecedorRepository)
    {
        _fornecedorRepository = fornecedorRepository;
    }

    public async Task<IEnumerable<Fornecedor>> ObterTodosFornecedoresAsync()
    {
        return await _fornecedorRepository.ObterTodosAsync();

        // Precisa lançar exceção se a lista estiver vazia?
        // if (!fornecedor.Any())
        //     throw new DBConcurrencyException("Nenhum fornecedor criado.");
    }

    public async Task<Fornecedor?> ObterFornecedorPorIdAsync(int id)
    {
        return await _fornecedorRepository.ObterPorIdAsync(id);

        // Precisa lançar exceção se o id não for encontrado?
        // if (fornecedor == null)
        //     throw new KeyNotFoundException("Fornecedor não encontrado.");
    }

    public async Task<Fornecedor> CriarFornecedorAsync(Fornecedor fornecedor)
    {
        fornecedor.DataRegistro = DateOnly.FromDateTime(DateTime.Now);
        return await _fornecedorRepository.AdicionarAsync(fornecedor);
    }

    public async Task AtualizarFornecedorAsync(int id, Fornecedor fornecedor)
    {
        if (id != fornecedor.Id)
        {
            throw new ArgumentException("O ID do fornecedor na URL não corresponde ao ID no corpo da requisição.");
        }
        await _fornecedorRepository.AtualizarAsync(fornecedor);
    }

    // public async Task DeletarFornecedorAsync(int id)
    // {
    //     await _fornecedorRepository.DeletarAsync(id);
    // }
}