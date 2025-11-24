using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;

namespace ApiHortifruti.Service;

public class ProdutoService : IProdutoService
{
    private readonly IUnityOfWork _uow;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;


    // Construtor com injeção de dependência do repositório
    public ProdutoService(IUnityOfWork uow, IDateTimeProvider dateTimeProvider, IMapper mapper)
    {
        _uow = uow;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    // Consulta de todos os produtos
    public async Task<IEnumerable<GetProdutoDTO>> ObterTodosOsProdutosAsync()
    {
        var produtos = await _uow.Produto.ObterTodosAsync(); // Acessa a camada (através do Unit of Work) repository para fazer a consulta
        return _mapper.Map<IEnumerable<GetProdutoDTO>>(produtos); // Mapeia de entidade para DTO e retorna
    }

    // Consulta de produto por ID
    public async Task<GetProdutoDTO?> ObterProdutoPorIdAsync(int id)
    {
        var produtoPorId = await _uow.Produto.ObterPorIdAsync(id); // Acessa a camada (através do Unit of Work) repository para fazer a consulta
        return _mapper.Map<GetProdutoDTO>(produtoPorId); // Mapeia de entidade para DTO e retorna
    }

    // Consulta de produto por código
    public async Task<GetProdutoDTO?> ObterProdutoPorCodigoAsync(string codigo)
    {
        var produtoPorCodigo = await _uow.Produto.ObterProdutoPorCodigoAsync(codigo); // Acessa a camada (através do Unit of Work) repository para fazer a consulta
        return _mapper.Map<GetProdutoDTO>(produtoPorCodigo); // Mapeia de entidade para DTO e retorna
    }

    // Consulta de produtos com estoque crítico
    public async Task<IEnumerable<GetProdutoEstoqueCriticoDTO>> ObterProdutosEstoqueCriticoAsync()
    {
        var produtosEstoqueCritico = await _uow.Produto.ObterEstoqueCriticoAsync(); // Acessa a camada (através do Unit of Work) repository para fazer a consulta
        return _mapper.Map<IEnumerable<GetProdutoEstoqueCriticoDTO>>(produtosEstoqueCritico); // Mapeia de entidade para DTO e retorna
    }

    public async Task<ProdutoComListaDeFornecedoresDTO> ObterListaDeFornecedoresQueFornecemCertoProduto(int produtoId)
    {
        var produto = await _uow.Produto.ObterProdutoComListaDeFornecedoresAtravesDeProdutoIdAsync(produtoId);
        if (produto is null) throw new NotFoundException("O 'Produto' informado na requisição não existe");

        return _mapper.Map<ProdutoComListaDeFornecedoresDTO>(produto);
    }

    // Inserção de um novo produto
    public async Task<GetProdutoDTO> CriarProdutoAsync(PostProdutoDTO postProdutoDTO)
    {
        // Conversão manual de DTO para entidade
        var produto = new Produto
        {
            Nome = postProdutoDTO.Nome,
            Codigo = postProdutoDTO.Codigo,
            CategoriaId = postProdutoDTO.CategoriaId,
            UnidadeMedidaId = postProdutoDTO.UnidadeMedidaId,
            Preco = postProdutoDTO.Preco,
            QuantidadeMinima = postProdutoDTO.QuantidadeMinima,
            QuantidadeAtual = 0, // Inicializa o estoque atual como 0
        };

        // Inicializa o histórico de preços do produto
        produto.HistoricoProduto = new List<HistoricoProduto>
        {
            new HistoricoProduto
            {
                PrecoProduto = produto.Preco,
                DataAlteracao = _dateTimeProvider?.Today ?? DateOnly.FromDateTime(DateTime.Today) // Usa a data atual fornecida pelo IDateTimeProvider ou a data de hoje
            }
        };

        var categoria = await _uow.Categoria.ObterPorIdAsync(produto.CategoriaId); // Consulta de categoria por id
        if (categoria is null) throw new KeyNotFoundException("A categoria informada não existe."); // Valida se a categoria existe

        var unidadeMedida = await _uow.UnidadeMedida.ObterPorIdAsync(produto.UnidadeMedidaId); // Consulta de unidade de medida por id
        if (unidadeMedida is null) throw new KeyNotFoundException("A unidade de medida informada não existe."); // Valida se a unidade de medida existe

        var codigoExistente = await _uow.Produto.ObterProdutoPorCodigoAsync(produto.Codigo); // Consulta de produto por código
        if (codigoExistente is not null) throw new ArgumentException("Esse código de produto já existe."); // Valida se o código do produto já existe

        var produtoCriado = await _uow.Produto.AdicionarAsync(produto);

        await _uow.SaveChangesAsync(); // Salva as alterações
        return _mapper.Map<GetProdutoDTO>(produtoCriado); // Mapeia de entidade para DTO e retorna
    }

    // Atualização de um produto existente
    public async Task AtualizarProdutoAsync(int id, PutProdutoDTO putProdutoDTO)
    {
        var produto = _mapper.Map<Produto>(putProdutoDTO); // Mapeia de DTO para entidade

        if (id != produto.Id) throw new ArgumentException("O ID do produto na URL não corresponde ao ID no corpo da requisição.");

        var produtoExistente = await _uow.Produto.ObterPorIdAsync(id); // Consulta do produto por id
        if (produtoExistente is null) throw new KeyNotFoundException("Produto não encontrado."); // Valida se o produto existe

        var precoAntigo = produtoExistente.Preco;

        await _uow.Produto.AtualizarAsync(produto); // Atualiza o produto (atráves da Unit of Work para acessar o repository)

        // Cria um registro em historicoProduto com novo preço, se o preço foi alterado
        if (precoAntigo != produto.Preco)
        {
            var historico = new HistoricoProduto
            {
                ProdutoId = id,
                PrecoProduto = produto.Preco,
                DataAlteracao = _dateTimeProvider?.Today ?? DateOnly.FromDateTime(DateTime.Today)
            };
            await _uow.HistoricoProduto.AdicionarAsync(historico); // Necessário chamar o AdicioanarAsync pois o EF não rastreia automaticamente aqui
        }

        await _uow.SaveChangesAsync(); // Salva as alterações
    }

    public async Task DeletarProdutoAsync(int id)
    {
        var produto = await _uow.Produto.ObterPorIdAsync(id);
        if (produto == null) throw new NotFoundException("O 'Produto' informado na requisição não existe");

        await _uow.Produto.DeletarAsync(produto);
        await _uow.SaveChangesAsync();
    }
}
