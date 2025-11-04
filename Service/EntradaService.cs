using ApiHortifruti.Data.Repository;
using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;


public class EntradaService : IEntradaService
{
    private readonly IUnityOfWork _uow;
    private readonly IItemEntradaService _itemEntradaService;

    public EntradaService(IUnityOfWork uow, IItemEntradaService itemEntradaService)
    {
        _uow = uow; // Inj. dependência
        _itemEntradaService = itemEntradaService;
    }

    public async Task<IEnumerable<Entrada>> ObterTodosEntradasAsync()
    {
        return await _uow.Entrada.ObterTodosAsync();
    }

    public async Task<Entrada?> ObterEntradaPorIdAsync(int id)
    {
        return await _uow.Entrada.ObterPorIdAsync(id);
        
    }

    public async Task<Entrada> CriarEntradaAsync(Entrada entrada)
{
    var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(entrada.FornecedorId);
    var motivo = await _uow.MotivoMovimentacao.ObterPorIdAsync(entrada.MotivoMovimentacaoId);
    var nota = await _uow.Entrada.ObterPorNumeroNotaAsync(entrada.NumeroNota, entrada.FornecedorId);

    if (fornecedor == null)
        throw new InvalidOperationException("Fornecedor não encontrado no sistema");

    if (motivo == null)
        throw new InvalidOperationException("Motivo de movimentação não encontrado no sistema");

    if (entrada.DataCompra > DateOnly.FromDateTime(DateTime.Now))
        throw new InvalidOperationException("A data da compra não pode ser uma data futura");

    if (nota != null)
        throw new InvalidOperationException("Já existe um registro com esse número de nota fiscal para o fornecedor informado");

    await _uow.Entrada.AdicionarAsync(entrada);
    await _itemEntradaService.AdicionarItensEntradaAsync(entrada.Id, entrada.ItemEntrada);

    await _uow.SaveChangesAsync();

    return entrada;
}


    public async Task AtualizarEntradaAsync(int id, Entrada entrada)
    {
        if (id != entrada.Id)
        {
            // Lançar erro/exceção
            return;
        }
        // await _entradaRepository.AtualizarAsync(entrada);
    }

    public async Task DeletarEntradaAsync(int id)
    {
        // await _entradaRepository.DeletarAsync(id);
    }
}

