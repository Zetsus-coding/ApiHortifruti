using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.DTO.Entrada;
using ApiHortifruti.DTO.ItemEntrada;
using ApiHortifruti.Domain;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;

namespace ApiHortifruti.Service;


public class EntradaService : IEntradaService
{
    private readonly IUnityOfWork _uow;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public EntradaService(IUnityOfWork uow, IDateTimeProvider dateTimeProvider, IMapper mapper)
    {
        _uow = uow;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetEntradaSimplesDTO>> ObterTodasAsEntradasAsync()
    {
        try
        {
            return _mapper.Map<IEnumerable<GetEntradaSimplesDTO>>(await _uow.Entrada.ObterTodosAsync());
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GetEntradaDTO?> ObterEntradaPorIdAsync(int id)
    {
        // Alterado para retornar GetEntradaDTO (detalhado)
        return _mapper.Map<GetEntradaDTO?>(await _uow.Entrada.ObterPorIdAsync(id));
    }

    public async Task<IEnumerable<GetEntradaSimplesDTO>> ObterEntradasRecentesAsync()
    {
        return _mapper.Map<IEnumerable<GetEntradaSimplesDTO>>(await _uow.Entrada.ObterRecentesAsync());
    }

    public async Task<GetEntradaDTO> CriarEntradaAsync(PostEntradaDTO postEntradaDTO)
    {
        if (postEntradaDTO.ItemEntrada == null || postEntradaDTO.ItemEntrada.Count == 0)
            throw new InvalidOperationException("É necessário adicionar no mínimo um item à entrada.");
        if (postEntradaDTO.ItemEntrada.Any(i => i.Quantidade <= 0))
            throw new InvalidOperationException("A quantidade de todos os itens deve ser maior que zero.");

        // Uso do AutoMapper para converter DTO -> Entity
        var entrada = _mapper.Map<Entrada>(postEntradaDTO);

        try
        {
            var fornecedor = await _uow.Fornecedor.ObterPorIdAsync(entrada.FornecedorId);
            var motivo = await _uow.MotivoMovimentacao.ObterPorIdAsync(entrada.MotivoMovimentacaoId);
            var nota = await _uow.Entrada.ObterPorNumeroNotaAsync(entrada.NumeroNota, entrada.FornecedorId);

            if (fornecedor == null)
                throw new KeyNotFoundException("Fornecedor não encontrado no sistema");

            if (motivo == null)
                throw new KeyNotFoundException("Motivo de movimentação não encontrado no sistema");

            if (entrada.DataCompra > _dateTimeProvider.Today)
                throw new InvalidOperationException("A data da compra não pode ser uma data futura");

            if (nota != null)
                throw new InvalidOperationException("Já existe um registro com esse número de nota fiscal para o fornecedor informado");

            // Adiciona a entrada ao context do EF
            await _uow.Entrada.AdicionarAsync(entrada);

            // Processa itens diretamente
            decimal precoTotal = 0m;
            foreach (var item in entrada.ItemEntrada)
            {
                var produto = await _uow.Produto.ObterPorIdAsync(item.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"O produto com ID {item.ProdutoId} não existe.");

                // Atualiza estoque
                produto.QuantidadeAtual += item.Quantidade;

                // Calcula valor total da entrada
                precoTotal += item.Quantidade * item.PrecoUnitario;
            }
            entrada.PrecoTotal = precoTotal;

            // Registra relacionamentos fornecedor-produto que não existem
            await InserirFornecedorProdutoDuranteCriarEntrada(entrada.FornecedorId, postEntradaDTO.ItemEntrada);

            await _uow.SaveChangesAsync();
            return _mapper.Map<GetEntradaDTO>(entrada);
        }
        catch
        {
            throw;
        }
    }

    public async Task InserirFornecedorProdutoDuranteCriarEntrada(int fornecedorId, IEnumerable<ItemEntradaDTO> itens)
    {
        var fpExistentes = await _uow.FornecedorProduto.ObterTodosAsync(); 
        var existentesSet = fpExistentes
            .Where(e => e.FornecedorId == fornecedorId)
            .Select(e => e.ProdutoId)
            .ToHashSet();

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
