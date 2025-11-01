using Microsoft.EntityFrameworkCore.Storage;

namespace ApiHortifruti.Data.Repository.Interfaces;

public interface IUnityOfWork : IDisposable
{
    IEntradaRepository Entrada { get; }
    IItemEntradaRepository ItensEntrada { get; }
    IProdutoRepository Produto { get; }
    IFornecedorRepository Fornecedor { get; }
    IMotivoMovimentacaoRepository MotivoMovimentacao { get; }

    public Task<int> SaveChangesAsync();
}
