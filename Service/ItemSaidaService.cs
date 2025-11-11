using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;

public class ItemSaidaService : IItemSaidaService
{
    private readonly IUnityOfWork _uow;

    public ItemSaidaService(IUnityOfWork uow)
    {
        _uow = uow; // Inj. dependência
    }

    public async Task AdicionarItensSaidaAsync(int saidaId, IEnumerable<ItemSaida> itens)
    {
        if (itens == null || !itens.Any())
            throw new InvalidOperationException("É necessário adicionar no mínimo um item à saída.");

        if (itens.Any(item => item.Quantidade <= 0))
            throw new InvalidOperationException("A quantidade de todos os itens deve ser maior que zero");

        await _uow.ItensSaida.AdicionarItensSaidaAsync(itens);

        foreach (var item in itens)
        {
            var produto = await _uow.Produto.ObterPorIdAsync(item.ProdutoId);

            if (produto == null)
                throw new InvalidOperationException($"O produto com ID {item.ProdutoId} não existe.");

            if (produto.QuantidadeAtual < item.Quantidade)
                throw new InvalidOperationException($"Estoque insuficiente para o produto ID {item.ProdutoId}. Disponível: {produto.QuantidadeAtual}, solicitado: {item.Quantidade}.");

            produto.QuantidadeAtual -= item.Quantidade;
            await _uow.Produto.AtualizarAsync(produto);
        }
    }
}

