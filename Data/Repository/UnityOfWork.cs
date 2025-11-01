using ApiHortifruti.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace ApiHortifruti.Data.Repository;

public class UnityOfWork : IUnityOfWork
{
    private readonly AppDbContext _context; // Context
    private IDbContextTransaction _transaction; // Transação

    // Propriedades dos repositórios
    private IEntradaRepository _entradaRepository;
    private IItemEntradaRepository _itemEntradaRepository;
    private IProdutoRepository _produtoRepository;
    private IFornecedorRepository _fornecedorRepository;
    private IMotivoMovimentacaoRepository _motivoMovimentacaoRepository;


    // Inicializador das propriedades ('lazy')
    public IEntradaRepository Entrada { get => _entradaRepository ??= new EntradaRepository(_context); }
    public IItemEntradaRepository ItensEntrada { get => _itemEntradaRepository ??= new ItemEntradaRepository(_context); }
    public IProdutoRepository Produto { get => _produtoRepository ??= new ProdutoRepository(_context); }
    public IFornecedorRepository Fornecedor { get => _fornecedorRepository ??= new FornecedorRepository(_context); }
    public IMotivoMovimentacaoRepository MotivoMovimentacao { get => _motivoMovimentacaoRepository ??= new MotivoMovimentacaoRepository(_context); }

    // Construtor

    public UnityOfWork(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context), "O contexto não pode ser nulo");
    }

    public async Task<int> SaveChangesAsync() // Salva as mudanças no context
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose() // O que fazer aqui (implementação)?
    {
        _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }

}
