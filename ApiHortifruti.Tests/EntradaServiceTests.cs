using ApiHortifruti.Domain;
using ApiHortifruti.Service;
using ApiHortifruti.Domain.DTO.ItemEntrada;
using Moq;
using Xunit;
using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Service.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using AutoMapper;

namespace ApiHortifruti.Tests;

public class EntradaServiceTests
{
    private readonly Mock<IUnityOfWork> _mockUow;
    private readonly Mock<IItemEntradaService> _mockItemEntradaService;
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
    private readonly Mock<IMapper> _mockMapper;

    private readonly EntradaService _service;

    // Mocks dos Repositórios
    private readonly Mock<IEntradaRepository> _mockEntradaRepo;
    private readonly Mock<IFornecedorRepository> _mockFornecedorRepo;
    private readonly Mock<IMotivoMovimentacaoRepository> _mockMotivoRepo;
    private readonly Mock<IFornecedorProdutoRepository> _mockFornecedorProdutoRepo;

    // Dados de teste
    private readonly Fornecedor _fornecedorFake = new Fornecedor { Id = 1, NomeFantasia = "Fazenda Bom Fruto" };
    private readonly MotivoMovimentacao _motivoFake = new MotivoMovimentacao { Id = 1, Motivo = "Compra" };
    private readonly Entrada _entradaValida;
    private readonly PostEntradaDTO _postEntradaValida;

    public EntradaServiceTests()
    {
        // Inicializar Mocks Principais
        _mockUow = new Mock<IUnityOfWork>();
        _mockItemEntradaService = new Mock<IItemEntradaService>();
        _mockDateTimeProvider = new Mock<IDateTimeProvider>();
        _mockMapper = new Mock<IMapper>();

        // Inicializar Mocks dos Repositórios
        _mockEntradaRepo = new Mock<IEntradaRepository>();
        _mockFornecedorRepo = new Mock<IFornecedorRepository>();
        _mockMotivoRepo = new Mock<IMotivoMovimentacaoRepository>();
        _mockFornecedorProdutoRepo = new Mock<IFornecedorProdutoRepository>();

        // Configurar comportamento dos Repositórios
        _mockFornecedorRepo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(_fornecedorFake);
        _mockMotivoRepo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(_motivoFake);

        // Simular não encontrado para IDs diferentes de 1
        _mockFornecedorRepo.Setup(r => r.ObterPorIdAsync(It.Is<int>(id => id != 1))).ReturnsAsync((Fornecedor?)null);
        _mockMotivoRepo.Setup(r => r.ObterPorIdAsync(It.Is<int>(id => id != 1))).ReturnsAsync((MotivoMovimentacao?)null);

        // Configuração do Repositório de Entrada
        _mockEntradaRepo.Setup(r => r.ObterPorNumeroNotaAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((Entrada?)null);
        _mockEntradaRepo.Setup(r => r.AdicionarAsync(It.IsAny<Entrada>())).Returns(Task.CompletedTask);

        // Configuração do Repositório de FornecedorProduto
        _mockFornecedorProdutoRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((FornecedorProduto?)null);
        
        // --- CORREÇÃO AQUI ---
        // Usamos Task.FromResult para satisfazer o retorno assíncrono
        _mockFornecedorProdutoRepo.Setup(r => r.AdicionarAsync(It.IsAny<FornecedorProduto>()))
            .Returns((FornecedorProduto f) => Task.FromResult(f)); 

        // Configurar UoW para retornar os Repositórios mockados
        _mockUow.Setup(uow => uow.Entrada).Returns(_mockEntradaRepo.Object);
        _mockUow.Setup(uow => uow.Fornecedor).Returns(_mockFornecedorRepo.Object);
        _mockUow.Setup(uow => uow.MotivoMovimentacao).Returns(_mockMotivoRepo.Object);
        _mockUow.Setup(uow => uow.FornecedorProduto).Returns(_mockFornecedorProdutoRepo.Object);

        // Setup das Transações
        _mockUow.Setup(uow => uow.BeginTransactionAsync()).ReturnsAsync(Mock.Of<IDbContextTransaction>());
        _mockUow.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);
        _mockUow.Setup(uow => uow.RollbackAsync()).Returns(Task.CompletedTask);
        _mockUow.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

        // Configurar DateProvider
        _mockDateTimeProvider.Setup(d => d.Today).Returns(DateOnly.FromDateTime(DateTime.Now));

        // Preparar dados da Entrada Válida
        _entradaValida = new Entrada
        {
            Id = 0,
            FornecedorId = 1,
            MotivoMovimentacaoId = 1,
            PrecoTotal = 150.00m,
            DataCompra = DateOnly.FromDateTime(DateTime.Now),
            NumeroNota = "NF-12345",
            ItemEntrada = new List<ItemEntrada> { new ItemEntrada { ProdutoId = 1, Quantidade = 1m, PrecoUnitario = 150.00m } }
        };

        _postEntradaValida = new PostEntradaDTO
        {
            FornecedorId = _entradaValida.FornecedorId,
            MotivoMovimentacaoId = _entradaValida.MotivoMovimentacaoId,
            PrecoTotal = _entradaValida.PrecoTotal,
            DataCompra = _entradaValida.DataCompra,
            NumeroNota = _entradaValida.NumeroNota,
            ItemEntrada = new List<ItemEntradaDTO>
            {
                new ItemEntradaDTO { ProdutoId = 1, Quantidade = 1m, PrecoUnitario = 150.00m, CodigoFornecedor = "CF-1" }
            }
        };

        _mockEntradaRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(_entradaValida);

        // Instanciar o serviço
        _service = new EntradaService(
            _mockUow.Object,
            _mockItemEntradaService.Object,
            _mockDateTimeProvider.Object,
            _mockMapper.Object
        );
    }

    // -------------------------------------------------------
    // TESTES DE SUCESSO
    // -------------------------------------------------------

    [Fact]
    public async Task CriarEntradaAsync_DeveCriarEntrada_QuandoDadosValidos()
    {
        // Arrange
        // (Preparação já feita no Construtor)

        // Act
        var resultado = await _service.CriarEntradaAsync(_postEntradaValida);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(_postEntradaValida.FornecedorId, resultado.FornecedorId);
        Assert.Equal(_postEntradaValida.PrecoTotal, resultado.PrecoTotal);

        _mockUow.Verify(u => u.CommitAsync(), Times.Once);
        _mockItemEntradaService.Verify(s => s.AdicionarItensEntradaAsync(It.IsAny<int>(), It.IsAny<IEnumerable<ItemEntrada>>()), Times.Once);
    }

    // -------------------------------------------------------
    // TESTES DE FALHA (VALIDAÇÕES)
    // -------------------------------------------------------

    [Fact]
    public async Task CriarEntradaAsync_DeveLancarExcecao_QuandoFornecedorNaoExiste()
    {
        // Arrange
        var dtoInvalido = _postEntradaValida;
        dtoInvalido.FornecedorId = 999; 

        // Act & Assert
        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _service.CriarEntradaAsync(dtoInvalido));

        Assert.Equal("Fornecedor não encontrado no sistema", ex.Message);
        _mockUow.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task CriarEntradaAsync_DeveLancarExcecao_QuandoMotivoNaoExiste()
    {
        // Arrange
        var dtoInvalido = _postEntradaValida;
        dtoInvalido.MotivoMovimentacaoId = 999;

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _service.CriarEntradaAsync(dtoInvalido));
    }

    [Fact]
    public async Task CriarEntradaAsync_DeveLancarExcecao_QuandoDataFutura()
    {
        // Arrange
        var dtoDataFutura = _postEntradaValida;
        dtoDataFutura.DataCompra = DateOnly.FromDateTime(DateTime.Now.AddDays(1)); 

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.CriarEntradaAsync(dtoDataFutura));

        Assert.Equal("A data da compra não pode ser uma data futura", ex.Message);
    }
}
