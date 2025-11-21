using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
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
        return await _uow.Fornecedor.ObterTodosAsync();
    }

    public async Task<Fornecedor?> ObterFornecedorPorIdAsync(int id)
    {
        return await _uow.Fornecedor.ObterPorIdAsync(id);
    }

    // Consulta de todos os produtos que um fornecedor fornece
    public async Task<IEnumerable<FornecedorComListaProdutosDTO>> ObterProdutosPorFornecedorIdAsync(int fornecedorId)
    {
        var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(fornecedorId);
        if (fornecedor is null) throw new NotFoundException("O 'Fornecedor' informado na requisição não existe");
        
        return await _uow.FornecedorProduto.ObterProdutosPorFornecedorIdAsync(fornecedorId);
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
        await _uow.SaveChangesAsync();
    }

    public Task<IEnumerable<FornecedorProduto>> ObterFornecedoresPorProdutoIdAsync(int produtoId)
    {
        throw new NotImplementedException();
    }

    public async Task<Fornecedor> ObterFornecedorComProdutosAsync(int id)
    {
        var fornecedor = await _uow.Fornecedor.ObterPorIdComProdutosAsync(id);
        if (fornecedor == null) throw new NotFoundException("O 'Fornecedor' informado na requisição não existe");

        return fornecedor;
    }

    public async Task DeletarFornecedorAsync(int id)
    {
        var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(id);
        if (fornecedor == null) throw new NotFoundException("O 'Fornecedor' informado na requisição não existe");

        await _uow.Fornecedor.DeletarAsync(fornecedor);
        await _uow.SaveChangesAsync();
    }
}