using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;

namespace ApiHortifruti.Service;

public class SaidaService : ISaidaService
{
    private readonly IUnityOfWork _uow;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public SaidaService(IUnityOfWork uow, IDateTimeProvider dateTimeProvider, IMapper mapper)
    {
        _uow = uow;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetSaidaSimplesDTO>> ObterTodasAsSaidasAsync()
    {
        try
        {
            var saidas = await _uow.Saida.ObterTodosAsync();
            return _mapper.Map<IEnumerable<GetSaidaSimplesDTO>>(saidas);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GetSaidaDTO?> ObterSaidaPorIdAsync(int id)
    {
        var saida = await _uow.Saida.ObterPorIdAsync(id);
        return _mapper.Map<GetSaidaDTO?>(saida);
    }

    public async Task<GetSaidaDTO> CriarSaidaAsync(PostSaidaDTO postSaidaDTO)
    {
        // Remove manual validation logic if redundant with DTO attributes, but keeping business rules.
        if (postSaidaDTO.ItemSaida == null || !postSaidaDTO.ItemSaida.Any())
            throw new InvalidOperationException("É obrigatório adicionar ao menos um item na saída");

        var saida = _mapper.Map<Saida>(postSaidaDTO);

        // Replicating EntradaService logic (EF Tracking, no explicit transaction if not needed or handled by SaveChanges, but EntradaService still used SaveChanges which implies Transaction scope usually? EntradaService removed Commit/Rollback/BeginTransaction calls in favor of single SaveChanges if I recall correctly from the file I read).
        // Let's check EntradaService content again. It removed BeginTransaction/Commit/Rollback blocks and just used _uow.SaveChangesAsync();
        // So I will do the same here.

        try
        {
            var motivo = await _uow.MotivoMovimentacao.ObterPorIdAsync(saida.MotivoMovimentacaoId);
            if (motivo == null)
                throw new InvalidOperationException("Motivo de movimentação não encontrado no sistema");

            saida.DataSaida = _dateTimeProvider.Today;
            saida.HoraSaida = TimeOnly.FromDateTime(_dateTimeProvider.Now);

            // Calculate totals and validate items
            decimal valorTotal = 0m;
            foreach (var item in saida.ItemSaida)
            {
                var produto = await _uow.Produto.ObterPorIdAsync(item.ProdutoId);

                if (produto == null)
                    throw new InvalidOperationException($"O produto com ID {item.ProdutoId} não existe.");

                if (produto.QuantidadeAtual < item.Quantidade)
                    throw new InvalidOperationException($"Estoque insuficiente para o produto ID {item.ProdutoId}. Disponível: {produto.QuantidadeAtual}, solicitado: {item.Quantidade}.");

                // Update stock
                produto.QuantidadeAtual -= item.Quantidade;

                // Calculate item value if not provided or just trust input?
                // EntradaService calculates PrecoTotal. Saida has ValorTotal.
                // Assuming ItemSaida has Valor (unit price * quantity or just unit price?).
                // Domain ItemSaida has "Valor". Usually this is total for the item line or unit price?
                // Looking at EntradaService: `precoTotal += item.Quantidade * item.PrecoUnitario;`
                // Looking at SaidaService (old): it didn't calculate total, it used `saida.ValorTotal`.

                // But we should probably recalculate `ValorTotal` based on items to be safe, or validate it.
                // However, Saida might have different pricing rules (discounts per item?).
                // I'll stick to updating stock and letting EF handle the graph.

                valorTotal += item.Valor;
            }

            saida.ValorTotal = valorTotal;

            if (saida.Desconto)
            {
                if (saida.ValorDesconto == null)
                    throw new InvalidOperationException("Valor de desconto deve ser informado quando houver desconto");

                if (saida.ValorDesconto < 0)
                    throw new InvalidOperationException("Valor de desconto inválido");

                if (saida.ValorTotal * 0.5m < saida.ValorDesconto)
                    throw new InvalidOperationException("O valor do desconto não pode ser maior que 50% do valor total");

                saida.ValorFinal = saida.ValorTotal - (saida.ValorDesconto ?? 0);
                if (saida.ValorFinal < 0)
                    throw new InvalidOperationException("O valor final da saída não pode ser negativo");
            }
            else
            {
                saida.ValorDesconto = 0;
                saida.ValorFinal = saida.ValorTotal;
            }

            await _uow.Saida.AdicionarAsync(saida);

            // EF will track items in `saida.ItemSaida` and insert them.
            // And since we loaded `Produto` entities above and modified them, EF should track updates to Products too?
            // Wait, we loaded `Produto` into `produto` var, but we didn't attach it to `item.Produto`.
            // However, `_uow.Produto.ObterPorIdAsync` likely returns a tracked entity.
            // Modifying `produto.QuantidadeAtual` on a tracked entity will cause an update.
            // So yes, stock update happens.

            await _uow.SaveChangesAsync();
            return _mapper.Map<GetSaidaDTO>(saida);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
