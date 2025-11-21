using ApiHortifruti.Data.Repository;
using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Domain.DTO.ItemEntrada;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;

namespace ApiHortifruti.Service;


public class EntradaService : IEntradaService
{
    private readonly IUnityOfWork _uow;
    private readonly IItemEntradaService _itemEntradaService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public EntradaService(IUnityOfWork uow, IItemEntradaService itemEntradaService, IDateTimeProvider dateTimeProvider, IMapper mapper)
    {
        _uow = uow; // Inj. dependência
        _itemEntradaService = itemEntradaService;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetEntradaSimplesDTO>> ObterTodasAsEntradasAsync()
    {
        try
        {
            return _mapper.Map<IEnumerable<GetEntradaSimplesDTO>>(await _uow.Entrada.ObterTodosAsync()); // Mapeia a lista de entradas para DTOs e retorna
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GetEntradaSimplesDTO?> ObterEntradaPorIdAsync(int id)
    {
        return _mapper.Map<GetEntradaSimplesDTO?>(await _uow.Entrada.ObterPorIdAsync(id)); // Mapeia a entrada para DTO e retorna
    }

    public async Task<IEnumerable<GetEntradaSimplesDTO>> ObterEntradasRecentesAsync()
    {
        return _mapper.Map<IEnumerable<GetEntradaSimplesDTO>>(await _uow.Entrada.ObterRecentesAsync());
    }

    public async Task<Entrada> CriarEntradaAsync(PostEntradaDTO postEntradaDTO)
    {
        // Conversão manual de DTO para entidade
        var entrada = new Entrada
        {
            FornecedorId = postEntradaDTO.FornecedorId,
            MotivoMovimentacaoId = postEntradaDTO.MotivoMovimentacaoId,
            NumeroNota = postEntradaDTO.NumeroNota,
            DataCompra = postEntradaDTO.DataCompra,
            ItemEntrada = postEntradaDTO.ItemEntrada.Select(item => new ItemEntrada
            {
                ProdutoId = item.ProdutoId,
                Quantidade = item.Quantidade,
                PrecoUnitario = item.PrecoUnitario,
            }).ToList()
        };

        await _uow.BeginTransactionAsync();
        try
        {
            var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(entrada.FornecedorId);
            var motivo = await _uow.MotivoMovimentacao.ObterPorIdAsync(entrada.MotivoMovimentacaoId);
            var nota = await _uow.Entrada.ObterPorNumeroNotaAsync(entrada.NumeroNota, entrada.FornecedorId);

            if (fornecedor == null) // Verifica se o fornecedor existe
                throw new KeyNotFoundException("Fornecedor não encontrado no sistema");

            if (motivo == null) // Verifica se o motivo de movimentação existe
                throw new KeyNotFoundException("Motivo de movimentação não encontrado no sistema");

            if (entrada.DataCompra > _dateTimeProvider.Today)
                throw new InvalidOperationException("A data da compra não pode ser uma data futura");

            if (nota != null)
                throw new InvalidOperationException("Já existe um registro com esse número de nota fiscal para o fornecedor informado");

            await _uow.Entrada.AdicionarAsync(entrada);
            await _itemEntradaService.AdicionarItensEntradaAsync(entrada.Id, entrada.ItemEntrada);
            await InserirFornecedorProdutoDuranteCriarEntrada(entrada.FornecedorId, postEntradaDTO.ItemEntrada);

            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();

            return entrada;
        }
        catch
        {
            await _uow.RollbackAsync();
            throw; // Relança a exceção para ser tratada pelo middleware
        }
    }

    public async Task InserirFornecedorProdutoDuranteCriarEntrada(int fornecedorId, IEnumerable<ItemEntradaDTO> itens)
    {
        foreach (var item in itens)
        {
            var registroExiste = await _uow.FornecedorProduto.ObterPorIdAsync(fornecedorId, item.ProdutoId);
            if (registroExiste == null)
            {
                var fornecedorProduto = new FornecedorProduto
                {
                    FornecedorId = fornecedorId,
                    ProdutoId = item.ProdutoId,
                    CodigoFornecedor = item.CodigoFornecedor,
                    Disponibilidade = true,
                    DataRegistro = _dateTimeProvider.Today // Usando provider
                };
                await _uow.FornecedorProduto.AdicionarAsync(fornecedorProduto);
            }
        }
    }
}

// public async Task AtualizarEntradaAsync(int id, Entrada entrada)
// {
//     if (id != entrada.Id) throw new ArgumentException("O ID da entrada na URL não corresponde ao ID no corpo da requisição.");

//     await _uow.Entrada.AtualizarAsync(entrada);
//     await _uow.SaveChangesAsync();
// }



// public async Task DeletarEntradaAsync(int id)
// {
//     var entrada = await _uow.Entrada.ObterPorIdAsync(id);
//     if (entrada == null) throw new NotFoundException("A 'Entrada' informada na requisição não existe");

//     await _uow.Entrada.DeletarAsync(entrada);
//     await _uow.SaveChangesAsync();
// }