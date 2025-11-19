using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore.Storage;

namespace ApiHortifruti.Tests;

public class ProdutoServiceTests
{
    private readonly Mock<IUnityOfWork> _mockUow;
    private readonly Mock<IProdutoRepository> _mockProdutoRepo;
    private readonly Mock<ICategoriaRepository> _mockCategoriaRepo;
    private readonly Mock<IUnidadeMedidaRepository> _mockUnidadeMedidaRepo;
    private readonly Mock<IHistoricoProdutoRepository> _mockHistoricoProdutoRepo;
    private readonly Mock<Interfaces.IProdutoServiceTests.IDateTimeProvider> _mockDateTimeProvider;
    private readonly ProdutoService _service;
    
    // Data Fixa para testes determinísticos
    private readonly DateOnly _hojeFixo = new DateOnly(2025, 11, 18); 
    
    // Dados Fakes
    private readonly Categoria _categoriaFake = new Categoria { Id = 1, Nome = "Frutas" };
    private readonly UnidadeMedida _unidadeMedidaFake = new UnidadeMedida { Id = 1, Nome = "Kg" };
    private readonly List<Produto> _produtosFake = new List<Produto>
    {
        // Produto 1: Não está em estoque crítico
        new Produto { Id = 1, Nome = "Maçã Fuji", Preco = 5.00m, QuantidadeAtual = 100, QuantidadeMinima = 50, CategoriaId = 1, UnidadeMedidaId = 1, Ativo = true, Codigo = "111" },
        // Produto 2: Está em estoque crítico (10 <= 20)
        new Produto { Id = 2, Nome = "Alface Crespa", Preco = 2.50m, QuantidadeAtual = 10, QuantidadeMinima = 20, CategoriaId = 2, UnidadeMedidaId = 2, Ativo = true, Codigo = "222" }
    };

    public ProdutoServiceTests()
    {
        _mockUow = new Mock<IUnityOfWork>();
        _mockProdutoRepo = new Mock<IProdutoRepository>();
        _mockCategoriaRepo = new Mock<ICategoriaRepository>();
        _mockUnidadeMedidaRepo = new Mock<IUnidadeMedidaRepository>();
        _mockHistoricoProdutoRepo = new Mock<IHistoricoProdutoRepository>();
        _mockDateTimeProvider = new Mock<Interfaces.IProdutoServiceTests.IDateTimeProvider>(); // Ajuste o namespace conforme sua interface

        // Configurar o UoW para retornar os Repositórios
        _mockUow.Setup(uow => uow.Produto).Returns(_mockProdutoRepo.Object);
        _mockUow.Setup(uow => uow.Categoria).Returns(_mockCategoriaRepo.Object);
        _mockUow.Setup(uow => uow.UnidadeMedida).Returns(_mockUnidadeMedidaRepo.Object);
        _mockUow.Setup(uow => uow.HistoricoProduto).Returns(_mockHistoricoProdutoRepo.Object);

        // 🚨 CORREÇÃO IMPORTANTE: Configurar Transações para evitar NullReferenceException
        _mockUow.Setup(uow => uow.BeginTransactionAsync()).ReturnsAsync(Mock.Of<IDbContextTransaction>());        
        _mockUow.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);
        _mockUow.Setup(uow => uow.RollbackAsync()).Returns(Task.CompletedTask); // O Rollback precisa estar configurado para ser rastreado
        _mockUow.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);
        
        // Configurar IDateTimeProvider
        _mockDateTimeProvider.Setup(p => p.Today).Returns(_hojeFixo);

        // Configurações Comuns de Consulta
        _mockProdutoRepo.Setup(r => r.ObterTodosAsync()).ReturnsAsync(_produtosFake);
        _mockProdutoRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                        .ReturnsAsync((int id) => _produtosFake.FirstOrDefault(p => p.Id == id));
        _mockProdutoRepo.Setup(r => r.ObterEstoqueCriticoAsync())
                        .ReturnsAsync(_produtosFake.Where(p => p.QuantidadeAtual <= p.QuantidadeMinima).ToList());

        // Validações de dependência
        _mockCategoriaRepo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(_categoriaFake);
        _mockCategoriaRepo.Setup(r => r.ObterPorIdAsync(It.IsNotIn(1))).ReturnsAsync((Categoria)null!); 

        _mockUnidadeMedidaRepo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(_unidadeMedidaFake);
        _mockUnidadeMedidaRepo.Setup(r => r.ObterPorIdAsync(It.IsNotIn(1))).ReturnsAsync((UnidadeMedida)null!); 
        
        // Instanciar o serviço
        // Se você ainda não atualizou o construtor do ProdutoService para receber IDateTimeProvider, remova o segundo argumento abaixo.
        _service = new ProdutoService(_mockUow.Object, _mockDateTimeProvider.Object); 
        // _service = new ProdutoService(_mockUow.Object, _mockDateTimeProvider.Object); // Use esta linha se já tiver refatorado o serviço
    }

    // ---------------------------------------------------------------------
    // Testes de Consulta (GET)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "ObterTodosProdutos deve retornar a lista completa")]
    public async Task ObterTodosProdutosAsync_DeveRetornarTodos()
    {
        // Act
        var resultado = await _service.ObterTodosOsProdutosAsync();

        // Assert (Xunit.Assert)
        Assert.NotNull(resultado);
        Assert.Equal(_produtosFake.Count, resultado.Count());
        _mockProdutoRepo.Verify(r => r.ObterTodosAsync(), Times.Once);
    }

    [Fact(DisplayName = "ObterProdutosEstoqueCritico deve retornar apenas produtos com estoque baixo")]
    public async Task ObterProdutosEstoqueCriticoAsync_DeveRetornarProdutosCriticos()
    {
        // Act
        var resultado = await _service.ObterProdutosEstoqueCriticoAsync();

        // Assert (Xunit.Assert)
        Assert.NotNull(resultado);
        Assert.Single(resultado); // Apenas o "Alface Crespa" (ID=2)
        Assert.Equal("Alface Crespa", resultado.First().Nome);
        _mockProdutoRepo.Verify(r => r.ObterEstoqueCriticoAsync(), Times.Once);
    }
    
    // ---------------------------------------------------------------------
    // Testes de Criação (POST)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "CriarProduto deve Adicionar, SaveChanges e Commitar a transação quando válido")]
    public async Task CriarProdutoAsync_Valido_DeveAdicionarESalvarECommittar()
    {
        // Arrange
        var novoProduto = new Produto 
        { 
            Id = 0, Nome = "Pêra", Preco = 8.00m, QuantidadeAtual = 20, 
            CategoriaId = _categoriaFake.Id, 
            UnidadeMedidaId = _unidadeMedidaFake.Id,
            Codigo = "333"
        };
        _mockProdutoRepo.Setup(r => r.AdicionarAsync(It.IsAny<Produto>())).ReturnsAsync(novoProduto);
        
        // Act
        var resultado = await _service.CriarProdutoAsync(novoProduto);

        // Assert (Xunit.Assert)
        Assert.NotNull(resultado);
        
        // Verifica se os métodos corretos de persistência foram chamados
        _mockProdutoRepo.Verify(r => r.AdicionarAsync(novoProduto), Times.Once);
        _mockUow.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mockUow.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact(DisplayName = "CriarProduto deve chamar Rollback e lançar exceção se CategoriaId for inválida")]
    public async Task CriarProdutoAsync_CategoriaInvalida_DeveLancarExcecaoEChamarRollback()
    {
        // Arrange
        var novoProduto = new Produto { Id = 0, Nome = "Invalido", CategoriaId = 99, UnidadeMedidaId = _unidadeMedidaFake.Id, Codigo = "333" };
        
        // Act & Assert (Xunit.Assert)
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CriarProdutoAsync(novoProduto));

        // Verifica que o método de adição e commit NÃO foram chamados, mas o Rollback foi.
        _mockUow.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
        _mockProdutoRepo.Verify(r => r.AdicionarAsync(It.IsAny<Produto>()), Times.Never);
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        _mockUow.Verify(uow => uow.CommitAsync(), Times.Never);
        _mockUow.Verify(uow => uow.RollbackAsync(), Times.Once);
    }

    // ---------------------------------------------------------------------
    // Testes de Atualização (PUT)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "AtualizarProduto deve chamar Atualizar, SaveChanges e Commitar se o preço não mudar")]
    public async Task AtualizarProdutoAsync_PrecoInalterado_DeveAtualizarSemHistorico()
    {
        // Arrange
        int idAtualizar = 1;
        var produtoOriginal = _produtosFake.First(p => p.Id == idAtualizar);
        var produtoAtualizado = new Produto 
        { 
            Id = idAtualizar, 
            Nome = "Maçã Gala", // Nome alterado
            Preco = produtoOriginal.Preco, // Preço igual
            CategoriaId = produtoOriginal.CategoriaId, 
            UnidadeMedidaId = produtoOriginal.UnidadeMedidaId 
        };
        
        // Simula que o produto EXISTE
        _mockProdutoRepo.Setup(r => r.ObterPorIdAsync(idAtualizar)).ReturnsAsync(produtoOriginal);

        // Act
        await _service.AtualizarProdutoAsync(idAtualizar, produtoAtualizado);

        // Assert (Xunit.Assert)
        _mockProdutoRepo.Verify(r => r.AtualizarAsync(produtoAtualizado), Times.Once);
        _mockHistoricoProdutoRepo.Verify(r => r.AdicionarAsync(It.IsAny<HistoricoProduto>()), Times.Never); // NÃO deve criar histórico
        
        _mockUow.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mockUow.Verify(uow => uow.CommitAsync(), Times.Once);
    }
    
    [Fact(DisplayName = "AtualizarProduto deve Criar Histórico, SaveChanges e Commitar se o preço MUDAR")]
    public async Task AtualizarProdutoAsync_PrecoAlterado_DeveCriarHistorico()
    {
        // Arrange
        int idAtualizar = 1;
        var produtoOriginal = _produtosFake.First(p => p.Id == idAtualizar); // Preco = 5.00m
        var novoPreco = 7.50m;
        
        var produtoAtualizado = new Produto 
        { 
            Id = idAtualizar, 
            Nome = "Maçã Gala", 
            Preco = novoPreco, // Preço alterado
            CategoriaId = produtoOriginal.CategoriaId, 
            UnidadeMedidaId = produtoOriginal.UnidadeMedidaId 
        };
        
        // Simula que o produto EXISTE
        _mockProdutoRepo.Setup(r => r.ObterPorIdAsync(idAtualizar)).ReturnsAsync(produtoOriginal);

        // Act
        await _service.AtualizarProdutoAsync(idAtualizar, produtoAtualizado);

        // Assert (Xunit.Assert)
        
        // 1. Verifica se o Atualizar foi chamado
        _mockProdutoRepo.Verify(r => r.AtualizarAsync(produtoAtualizado), Times.Once);
        
        // 2. Verifica se o HistoricoProduto foi criado com o novo preço e a data mockada
        _mockHistoricoProdutoRepo.Verify(r => r.AdicionarAsync(It.Is<HistoricoProduto>(
            h => h.ProdutoId == idAtualizar && 
                 h.PrecoProduto == novoPreco && 
                 h.DataAlteracao == _hojeFixo
        )), Times.Once); 
        
        // 3. Verifica o fluxo da transação
        _mockUow.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mockUow.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact(DisplayName = "AtualizarProduto com IDs divergentes deve lançar ArgumentException")]
    public async Task AtualizarProdutoAsync_ComIdsDiferentes_DeveLancarExcecaoEChamarRollback()
    {
        // Arrange
        int idUrl = 10;
        var produtoComIdDiferente = new Produto { Id = 20, Nome = "Teste", CategoriaId = 1, UnidadeMedidaId = 1 };
        
        // Configura o mock para simular o início da transação (que deve falhar na primeira validação)
        
        // Act & Assert (Xunit.Assert)
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.AtualizarProdutoAsync(idUrl, produtoComIdDiferente));

        // Verifica que NENHUM método de persistência foi chamado e que a transação foi fechada (Rollback)
        _mockUow.Verify(uow => uow.BeginTransactionAsync(), Times.Once); // O Begin deve ser o primeiro
        _mockProdutoRepo.Verify(r => r.ObterPorIdAsync(It.IsAny<int>()), Times.Never);
        _mockProdutoRepo.Verify(r => r.AtualizarAsync(It.IsAny<Produto>()), Times.Never);
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        _mockUow.Verify(uow => uow.CommitAsync(), Times.Never);
        _mockUow.Verify(uow => uow.RollbackAsync(), Times.Once);
    }
}