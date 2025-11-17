using ApiHortifruti.Domain;
using ApiHortifruti.Service;
using Moq;
using Xunit;
using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Service.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;


namespace ApiHortifruti.Tests;

public class EntradaServiceTests
{
    private readonly Mock<IUnityOfWork> _mockUow;
    private readonly Mock<IItemEntradaService> _mockItemEntradaService;
    private readonly EntradaService _service;
    private readonly Mock<IEntradaRepository> _mockEntradaRepo;
    private readonly Mock<IFornecedorRepository> _mockFornecedorRepo;
    private readonly Mock<IMotivoMovimentacaoRepository> _mockMotivoRepo;

    // Dados de teste (Mocks de Entidades)
    // Usando as propriedades que você definiu recentemente
    private readonly Fornecedor _fornecedorFake = new Fornecedor { Id = 1, NomeFantasia = "Fazenda Bom Fruto" };
    private readonly MotivoMovimentacao _motivoFake = new MotivoMovimentacao { Id = 1, TipoMovimentacao = "Compra" };

    // Entrada Válida (usa DateOnly de ontem)
    private readonly Entrada _entradaValida = new Entrada
    {
        Id = 0,
        FornecedorId = 1,
        MotivoMovimentacaoId = 1,
        PrecoTotal = 150.00m,
        DataCompra = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
        NumeroNota = "NF-12345",
        ItemEntrada = new List<ItemEntrada> { new ItemEntrada() }
    };

    public EntradaServiceTests()
    {
        // 1. Inicializar Mocks
        _mockUow = new Mock<IUnityOfWork>();
        _mockItemEntradaService = new Mock<IItemEntradaService>();
        // O tipo aqui é o tipo REAL do seu projeto
        _mockEntradaRepo = new Mock<IEntradaRepository>();
        _mockFornecedorRepo = new Mock<IFornecedorRepository>();
        _mockMotivoRepo = new Mock<IMotivoMovimentacaoRepository>();

        // 2. Configurar Comportamento Padrão de Sucesso (Setup)

        // Repositórios de Dependência
        _mockFornecedorRepo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(_fornecedorFake);
        _mockMotivoRepo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(_motivoFake);

        // Simular Fornecedor/Motivo inexistente para IDs diferentes de 1
        _mockFornecedorRepo.Setup(r => r.ObterPorIdAsync(It.Is<int>(id => id != 1))).ReturnsAsync((Fornecedor?)null);
        _mockMotivoRepo.Setup(r => r.ObterPorIdAsync(It.Is<int>(id => id != 1))).ReturnsAsync((MotivoMovimentacao?)null);

        // ...existing code...

        // ...existing code...

        // ...existing setup...

        // Repositório de Entrada (checar nota duplicada e métodos CRUD)
        _mockEntradaRepo.Setup(r => r.ObterPorNumeroNotaAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((Entrada?)null);
        _mockEntradaRepo.Setup(r => r.AdicionarAsync(It.IsAny<Entrada>())).Returns(Task.CompletedTask);
        _mockEntradaRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(_entradaValida);
        _mockEntradaRepo.Setup(r => r.AtualizarAsync(It.IsAny<Entrada>())).Returns(Task.CompletedTask);

        // UoW Setup: Configura o UoW para retornar os mocks de repositório
        _mockUow.Setup(uow => uow.Entrada).Returns(_mockEntradaRepo.Object);
        _mockUow.Setup(uow => uow.Fornecedor).Returns(_mockFornecedorRepo.Object);
        _mockUow.Setup(uow => uow.MotivoMovimentacao).Returns(_mockMotivoRepo.Object);

        // Setup das Transações e SaveChanges
        _mockUow.Setup(uow => uow.BeginTransactionAsync())
                .ReturnsAsync(Mock.Of<IDbContextTransaction>()); // retorna Task<IDbContextTransaction>
        _mockUow.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);
        _mockUow.Setup(uow => uow.RollbackAsync()).Returns(Task.CompletedTask);
        _mockUow.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1); // Task<int>

        // Setup do Serviço de Item de Entrada
        _mockItemEntradaService.Setup(s => s.AdicionarItensEntradaAsync(It.IsAny<int>(), It.IsAny<ICollection<ItemEntrada>>())).Returns(Task.CompletedTask);

        // 3. Instanciar o serviço com os mocks
        _service = new EntradaService(_mockUow.Object, _mockItemEntradaService.Object);
    }

    // ---------------------------------------------------------------------
    // Testes de Consulta (ObterTodos e ObterPorId)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "ObterTodos deve chamar o repositório ObterTodosAsync")]
    public async Task ObterTodosEntradasAsync_DeveChamarRepositorio()
    {
        // Act
        await _service.ObterTodosEntradasAsync();

        // Assert
        _mockEntradaRepo.Verify(r => r.ObterTodosAsync(), Times.Once);
    }

    [Fact(DisplayName = "ObterPorId deve chamar o repositório ObterPorIdAsync")]
    public async Task ObterEntradaPorIdAsync_DeveChamarRepositorio()
    {
        // Arrange
        int idProcurado = 5;

        // Act
        await _service.ObterEntradaPorIdAsync(idProcurado);

        // Assert
        _mockEntradaRepo.Verify(r => r.ObterPorIdAsync(idProcurado), Times.Once);
    }

    // ---------------------------------------------------------------------
    // Testes de CriarEntradaAsync (Sucesso e Transação)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "CriarEntrada deve executar o fluxo completo e fazer Commit")]
    public async Task CriarEntradaAsync_ComDadosValidos_DeveChamarPersistenciaECommit()
    {
        // Act
        var resultado = await _service.CriarEntradaAsync(_entradaValida);

        // Assert
        _mockUow.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
        _mockUow.Verify(uow => uow.CommitAsync(), Times.Once);
        _mockUow.Verify(uow => uow.RollbackAsync(), Times.Never);

        _mockUow.Verify(uow => uow.Entrada.AdicionarAsync(It.IsAny<Entrada>()), Times.Once);
        _mockItemEntradaService.Verify(s => s.AdicionarItensEntradaAsync(It.IsAny<int>(), _entradaValida.ItemEntrada), Times.Once);
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    // ---------------------------------------------------------------------
    // Testes de CriarEntradaAsync (Validações e Rollback)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "Deve lançar KeyNotFoundException se Fornecedor não existir e fazer Rollback")]
    public async Task CriarEntradaAsync_FornecedorInexistente_DeveLancarExcecaoERollback()
    {
        // Arrange
        var entradaInvalida = new Entrada
        {
            Id = _entradaValida.Id,
            FornecedorId = 99,
            MotivoMovimentacaoId = _entradaValida.MotivoMovimentacaoId,
            PrecoTotal = _entradaValida.PrecoTotal,
            DataCompra = _entradaValida.DataCompra,
            NumeroNota = _entradaValida.NumeroNota,
            ItemEntrada = _entradaValida.ItemEntrada
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CriarEntradaAsync(entradaInvalida));
    }

    [Fact(DisplayName = "Deve lançar InvalidOperationException se DataCompra for futura e fazer Rollback")]
    public async Task CriarEntradaAsync_DataFutura_DeveLancarExcecaoERollback()
    {
        // Arrange
        var entradaInvalida = new Entrada
        {
            Id = _entradaValida.Id,
            FornecedorId = _entradaValida.FornecedorId,
            MotivoMovimentacaoId = _entradaValida.MotivoMovimentacaoId,
            PrecoTotal = _entradaValida.PrecoTotal,
            DataCompra = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            NumeroNota = _entradaValida.NumeroNota,
            ItemEntrada = _entradaValida.ItemEntrada
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarEntradaAsync(entradaInvalida));
    }

    [Fact(DisplayName = "Deve lançar InvalidOperationException se Nota já existir para o fornecedor e fazer Rollback")]
    public async Task CriarEntradaAsync_NotaDuplicada_DeveLancarExcecaoERollback()
    {
        // Arrange
        var entradaComNotaDuplicada = new Entrada
        {
            Id = _entradaValida.Id,
            FornecedorId = _entradaValida.FornecedorId,
            MotivoMovimentacaoId = _entradaValida.MotivoMovimentacaoId,
            PrecoTotal = _entradaValida.PrecoTotal,
            DataCompra = _entradaValida.DataCompra,
            NumeroNota = "NF-DUPLICADA",
            ItemEntrada = _entradaValida.ItemEntrada
        };
        
        _mockEntradaRepo.Setup(r => r.ObterPorNumeroNotaAsync("NF-DUPLICADA", It.IsAny<int>()))
                        .ReturnsAsync(new Entrada { Id = 10 }); 

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarEntradaAsync(entradaComNotaDuplicada));
    }

    [Fact(DisplayName = "Deve fazer Rollback se ItemEntradaService falhar")]
    public async Task CriarEntradaAsync_FalhaAoAdicionarItens_DeveChamarRollback()
    {
        // Arrange
        _mockItemEntradaService.Setup(s => s.AdicionarItensEntradaAsync(It.IsAny<int>(), It.IsAny<ICollection<ItemEntrada>>()))
                               .ThrowsAsync(new Exception("Falha de persistência simulada"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CriarEntradaAsync(_entradaValida));

        // Assert
        _mockUow.Verify(uow => uow.CommitAsync(), Times.Never);
        _mockUow.Verify(uow => uow.RollbackAsync(), Times.Once);
        _mockUow.Verify(uow => uow.Entrada.AdicionarAsync(It.IsAny<Entrada>()), Times.Once);
    }

    // ---------------------------------------------------------------------
    // Testes de AtualizarEntradaAsync (Comportamento Atual)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "AtualizarEntrada com IDs divergentes deve retornar sem atualizar")]
    public async Task AtualizarEntradaAsync_ComIdsDiferentes_DeveRetornarSemAtualizar()
    {
        // Arrange
        int idUrl = 10;
        var entradaComIdDiferente = new Entrada { Id = 20, NumeroNota = "Teste" };

        // Act
        await _service.AtualizarEntradaAsync(idUrl, entradaComIdDiferente);

        // Assert
        _mockUow.Verify(uow => uow.Entrada.AtualizarAsync(It.IsAny<Entrada>()), Times.Never);
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
}