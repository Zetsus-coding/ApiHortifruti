using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;

public class FornecedorService : IFornecedorService
{
    private readonly IUnityOfWork _uow;

    public FornecedorService( IUnityOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Fornecedor>> ObterTodosOsFornecedoresAsync()
    {
        try
        {
            return await _uow.Fornecedor.ObterTodosAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Fornecedor?> ObterFornecedorPorIdAsync(int id)
    {
        return await _uow.Fornecedor.ObterPorIdAsync(id);

        // Precisa lançar exceção se o id não for encontrado?
        // if (fornecedor == null)
        //     throw new KeyNotFoundException("Fornecedor não encontrado.");
    }

    public async Task<Fornecedor> CriarFornecedorAsync(Fornecedor fornecedor)
    {
        fornecedor.DataRegistro = DateOnly.FromDateTime(DateTime.Now);
        return await _uow.Fornecedor.AdicionarAsync(fornecedor);
    }

    public async Task AtualizarFornecedorAsync(int id, Fornecedor fornecedor)
    {
        if (id != fornecedor.Id)
        {
            throw new ArgumentException("O ID do fornecedor na URL não corresponde ao ID no corpo da requisição.");
        }
        await _uow.Fornecedor.AtualizarAsync(fornecedor);
    }

    public Task<IEnumerable<FornecedorProduto>> ObterFornecedoresPorProdutoIdAsync(int produtoId)
    {
        throw new NotImplementedException();
    }

    // public async Task DeletarFornecedorAsync(int id)
    // {
    //     await _uow.Fornecedor.DeletarAsync(id);
    // }
}