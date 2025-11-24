using ApiHortifruti.Data.Repository;
using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.DTO.ItemEntrada;
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
        // Validações dos itens entrada antes de criar a entidade Entrada
        if (postEntradaDTO.ItemEntrada == null || postEntradaDTO.ItemEntrada.Count == 0)
            throw new InvalidOperationException("É necessário adicionar no mínimo um item à entrada.");
        if (postEntradaDTO.ItemEntrada.Any(i => i.Quantidade <= 0))
            throw new InvalidOperationException("A quantidade de todos os itens deve ser maior que zero.");

        // Criação da entidade Entrada com os itens a partir do DTO
        var entrada = new Entrada
        {
            FornecedorId = postEntradaDTO.FornecedorId,
            MotivoMovimentacaoId = postEntradaDTO.MotivoMovimentacaoId,
            NumeroNota = postEntradaDTO.NumeroNota,
            DataCompra = postEntradaDTO.DataCompra,
            PrecoTotal = postEntradaDTO.PrecoTotal,
            ItemEntrada = postEntradaDTO.ItemEntrada.Select(item => new ItemEntrada
            {
                ProdutoId = item.ProdutoId,
                Quantidade = item.Quantidade,
                PrecoUnitario = item.PrecoUnitario,
            }).ToList()
        };

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

            // Adiciona a entrada ao context do EF que também rastreia os itens da coleção (ex: item entrada)
            await _uow.Entrada.AdicionarAsync(entrada);

            // Processa itens diretamente
            decimal precoTotal = 0m;
            foreach (var item in entrada.ItemEntrada)
            {
                var produto = await _uow.Produto.ObterPorIdAsync(item.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"O produto com ID {item.ProdutoId} não existe.");

                // Atualiza estoque (o EF já consegue rastrear a mudança e mandar o UPDATE)
                produto.QuantidadeAtual += item.Quantidade;
                // Calcula valor total da entrada
                precoTotal += item.Quantidade * item.PrecoUnitario;
            }
            entrada.PrecoTotal = precoTotal;

            // Registra relacionamentos fornecedor-produto que não existem
            await InserirFornecedorProdutoDuranteCriarEntrada(entrada.FornecedorId, postEntradaDTO.ItemEntrada);

            await _uow.SaveChangesAsync(); // A princípio, garante a atomicidade
            return entrada;
        }
        catch
        {
            throw; // Middleware trata as exceções
        }
    }

    public async Task InserirFornecedorProdutoDuranteCriarEntrada(int fornecedorId, IEnumerable<ItemEntradaDTO> itens)
    {
        // TODO (Planos futuros: Talvez otimizar a consulta inicial e filtragem, buscando por fornecedorId logo de cara [ObterProdutosPorFornecedorIdSimplesAsync])

        // Carrega relacionamentos existentes e filtra apenas os do fornecedorId informado (armazena em um hashset)
        var fpExistentes = await _uow.FornecedorProduto.ObterTodosAsync(); 
        var existentesSet = fpExistentes
            .Where(e => e.FornecedorId == fornecedorId)
            .Select(e => e.ProdutoId)
            .ToHashSet();

        // Criação apenas de novos relacionamentos de FornecedorProduto (que não existem no hashset)
        var novos = itens
            .Where(i => !existentesSet.Contains(i.ProdutoId))
            .Select(i => new FornecedorProduto
            {
                FornecedorId = fornecedorId,
                ProdutoId = i.ProdutoId,
                CodigoFornecedor = i.CodigoFornecedor,
                Disponibilidade = true,
                DataRegistro = _dateTimeProvider?.Today ?? DateOnly.FromDateTime(DateTime.Today)
            })
            .ToList();

        if (novos.Count > 0)
        {
            await _uow.FornecedorProduto.AdicionarVariosAsync(novos);
        }
    }
}

// // Removido:
// // A princípio, não é possível atualizar ou deletar uma entrada
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
