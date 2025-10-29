using Microsoft.EntityFrameworkCore.Storage;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface IUnityOfWork : IDisposable
{
    IEntradaRepository Entrada { get; }
    IItem_entradaRepository ItensEntrada { get; }
    IProdutoRepository Produto { get; }
    IFornecedorRepository Fornecedor { get; }
    IMotivo_movimentacaoRepository MotivoMovimentacao { get; }

    public Task<IDbContextTransaction> BeginTransactionAsync();
    public Task<int> SaveChangesAsync();
    public Task CommitAsync(IDbContextTransaction transaction);
    public Task RollbackAsync(IDbContextTransaction transaction);
}
