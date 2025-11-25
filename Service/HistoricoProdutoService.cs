using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;

namespace ApiHortifruti.Service;


public class HistoricoProdutoService : IHistoricoProdutoService
{
    private readonly IUnityOfWork _uow;
    private readonly IMapper _mapper;

    public HistoricoProdutoService(IUnityOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetHistoricoProdutoDTO>> ObterTodosOsHistoricosProdutosAsync()
    {
        try
        {
            var historicos = await _uow.HistoricoProduto.ObterTodosAsync();
            return _mapper.Map<IEnumerable<GetHistoricoProdutoDTO>>(historicos);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GetHistoricoProdutoDTO?> ObterHistoricoProdutoPorIdAsync(int id)
    {
        var historico = await _uow.HistoricoProduto.ObterPorIdAsync(id);
        return _mapper.Map<GetHistoricoProdutoDTO?>(historico);
    }

    public async Task<IEnumerable<GetHistoricoProdutoDTO>> ObterHistoricoProdutoPorProdutoIdAsync(int produtoId)
    {
        var historicos = await _uow.HistoricoProduto.ObterPorProdutoIdAsync(produtoId);
        return _mapper.Map<IEnumerable<GetHistoricoProdutoDTO>>(historicos);
    }

    public async Task CriarHistoricoProdutoAsync(HistoricoProduto historicoProduto)
    {
        // This method is used internally by other services (e.g. ProdutoService), so it takes Entity.
        // It does not need to return DTO as it's not exposed to Controller (or the return is ignored/void).
        // I changed return type to Task (void) in Interface to match usage if return value is not needed,
        // or I can return Entity if needed internally. Interface said Task.

        await _uow.HistoricoProduto.AdicionarAsync(historicoProduto);
        await _uow.SaveChangesAsync();
    }
}
