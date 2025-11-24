using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.DTO.FornecedorProduto;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;

namespace ApiHortifruti.Service;

public class FornecedorProdutoService : IFornecedorProdutoService
{
    private readonly IUnityOfWork _uow;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public FornecedorProdutoService(IUnityOfWork uow, IDateTimeProvider dateTimeProvider, IMapper mapper)
    {
        _uow = uow;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetFornecedorProdutoDTO>> ObterTodosOsFornecedorProdutoAsync()
    {
        var result = await _uow.FornecedorProduto.ObterTodosAsync();
        return _mapper.Map<IEnumerable<GetFornecedorProdutoDTO>>(result);
    }

    public async Task<GetFornecedorProdutoDTO?> ObterFornecedorProdutoPorIdAsync(int fornecedorId, int produtoId)
    {
        var result = await _uow.FornecedorProduto.ObterPorIdAsync(fornecedorId, produtoId);
        return _mapper.Map<GetFornecedorProdutoDTO?>(result);
    }

    public async Task<GetFornecedorProdutoDTO> CriarFornecedorProdutoAsync(PostFornecedorProdutoDTO postFornecedorProdutoDTO)
    {
        var fornecedorProduto = _mapper.Map<FornecedorProduto>(postFornecedorProdutoDTO);

        await _uow.BeginTransactionAsync();

        try
        {
            var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(fornecedorProduto.FornecedorId);
            if (fornecedor == null) throw new KeyNotFoundException("Fornecedor não encontrado.");
            
            var produto = await _uow.Produto.ObterPorIdAsync(fornecedorProduto.ProdutoId);
            if (produto == null) throw new KeyNotFoundException("Produto não encontrado.");
            
            var existente = await _uow.FornecedorProduto.ObterPorIdAsync(fornecedorProduto.FornecedorId, fornecedorProduto.ProdutoId);
            if (existente != null) throw new InvalidOperationException("A relação entre o fornecedor e o produto já existe.");

            fornecedorProduto.Disponibilidade = true;
            fornecedorProduto.DataRegistro = _dateTimeProvider.Today;

            var novoFornecedorProduto = await _uow.FornecedorProduto.AdicionarAsync(fornecedorProduto);

            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();

            return _mapper.Map<GetFornecedorProdutoDTO>(novoFornecedorProduto);
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<GetFornecedorProdutoDTO>> CriarVariosFornecedorProdutosAsync(IEnumerable<PostFornecedorProdutoDTO> postFornecedorProdutoDTOs)
    {
        var fornecedorProdutos = _mapper.Map<IEnumerable<FornecedorProduto>>(postFornecedorProdutoDTOs).ToList();

        if (fornecedorProdutos == null || !fornecedorProdutos.Any())
            throw new ArgumentException("A lista de associações não pode ser nula ou vazia.");

        var duplicados = fornecedorProdutos
            .GroupBy(fp => new { fp.FornecedorId, fp.ProdutoId })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicados.Any())
            throw new InvalidOperationException("Existem associações duplicadas na lista fornecida.");

        await _uow.BeginTransactionAsync();

        try
        {
            foreach (var fornProd in fornecedorProdutos)
            {
                var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(fornProd.FornecedorId);
                var produto = await _uow.Produto.ObterPorIdAsync(fornProd.ProdutoId);
                var existente = await _uow.FornecedorProduto.ObterPorIdAsync(fornProd.FornecedorId, fornProd.ProdutoId);

                if (fornecedor == null) throw new KeyNotFoundException($"Fornecedor não encontrado.");
                if (produto == null) throw new KeyNotFoundException($"Produto não encontrado.");
                if (existente != null) throw new InvalidOperationException($"A relação para o fornecedorId {fornProd.FornecedorId} e produtoId {fornProd.ProdutoId} já existe.");

                fornProd.Disponibilidade = true;
                fornProd.DataRegistro = _dateTimeProvider.Today;
            }

            await _uow.FornecedorProduto.AdicionarVariosAsync(fornecedorProdutos);
            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();

            return _mapper.Map<IEnumerable<GetFornecedorProdutoDTO>>(fornecedorProdutos);
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }

    public async Task AtualizarFornecedorProdutoAsync(int fornecedorId, int produtoId, PutFornecedorProdutoDTO putFornecedorProdutoDTO)
    {
        if (fornecedorId != putFornecedorProdutoDTO.FornecedorId || produtoId != putFornecedorProdutoDTO.ProdutoId)
        {
            throw new ArgumentException("Os IDs da relação informados na URL não correspondem ao corpo da requisição.");
        }
        
        var fornecedorProduto = _mapper.Map<FornecedorProduto>(putFornecedorProdutoDTO);

        await _uow.FornecedorProduto.AtualizarAsync(fornecedorProduto);
        await _uow.SaveChangesAsync();
    }

    public async Task DeletarFornecedorProdutoAsync(int fornecedorId, int produtoId)
    {
        await _uow.FornecedorProduto.DeletarAsync(fornecedorId, produtoId);
        await _uow.SaveChangesAsync();
    }
}
