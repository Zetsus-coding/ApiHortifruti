using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;

namespace ApiHortifruti.Service;

public class SaidaService : ISaidaService
{
    private readonly IUnityOfWork _uow;
    private readonly IItemSaidaService _itemSaidaService;

    public SaidaService(IUnityOfWork uow, IItemSaidaService itemSaidaService)
    {
        _uow = uow;
        _itemSaidaService = itemSaidaService;
    }

    public async Task<IEnumerable<Saida>> ObterTodosSaidasAsync()
    {
        return await _uow.Saida.ObterTodosAsync();
    }

    public async Task<Saida?> ObterSaidaPorIdAsync(int id)
    {
        return await _uow.Saida.ObterPorIdAsync(id);
    }

    public async Task<Saida> CriarSaidaAsync(Saida saida)
    {
        await using var transaction = await _uow.BeginTransactionAsync();

        try
        {
            //var funcionario = await _uow.Funcionario.ObterPorIdAsync(saida.FuncionarioId);
            //if (funcionario == null) // Validação de existência do funcionário
            //    throw new InvalidOperationException("Funcionário não encontrado no sistema");

            var motivo = await _uow.MotivoMovimentacao.ObterPorIdAsync(saida.MotivoMovimentacaoId);
            if (motivo == null) // Validação de existência do motivo de movimentação
                throw new InvalidOperationException("Motivo de movimentação não encontrado no sistema");

            // if (saida.DataSaida > DateOnly.FromDateTime(DateTime.Now)) // Validação de data futura
            //     throw new InvalidOperationException("A data da saída não pode ser uma data futura");

            if (saida.ItemSaida == null || !saida.ItemSaida.Any()) // Validação de itens na saída
                throw new InvalidOperationException("É obrigatório adicionar ao menos um item na saída");

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

            saida.DataSaida = DateOnly.FromDateTime(DateTime.Now);
            saida.HoraSaida = TimeOnly.FromDateTime(DateTime.Now);

            await _uow.Saida.AdicionarAsync(saida);
            await _itemSaidaService.AdicionarItensSaidaAsync(saida.Id, saida.ItemSaida);

            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();

            return saida;
        }
        catch (Exception exc)
        {
            await transaction.RollbackAsync();
            throw new Exception("Erro ao criar a saída e/ou seus registros subsequentes. MENSAGEM: " + exc.Message);
        }
    }

    // public async Task AtualizarSaidaAsync(int id, Saida saida)
    // {
    //     if (id != saida.Id)
    //     {
    //         throw new ArgumentException("O ID da saída na URL não corresponde ao ID no corpo da requisição.");
    //     }
    //     await _uow.Saida.AtualizarAsync(saida);
    //     await _uow.SaveChangesAsync();
    // }

    // public async Task DeletarSaidaAsync(int id)
    // {
    //     await _uow.Saida.DeletarAsync(id);
    //     await _uow.SaveChangesAsync();
    // }
}
