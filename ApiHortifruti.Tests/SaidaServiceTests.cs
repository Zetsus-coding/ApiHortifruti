using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service;
using ApiHortifruti.Service.Interfaces; // Para IItemSaidaService e IDateTimeProvider
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage; // Para IDbContextTransaction

namespace ApiHortifruti.Tests;

public class SaidaServiceTests
{
    private readonly Mock<IUnityOfWork> _mockUow;
    private readonly Mock<ISaidaRepository> _mockSaidaRepo;
    private readonly Mock<IMotivoMovimentacaoRepository> _mockMotivoRepo;
    private readonly Mock<IItemSaidaService> _mockItemSaidaService;
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
    private readonly SaidaService _service;

    // Data fixa para testes
    private readonly DateOnly _hojeFixo = new DateOnly(2025, 11, 19);
    private readonly TimeOnly _horaFixa = new TimeOnly(10, 0, 0);

    // Entidades Fake
    private readonly MotivoMovimentacao _motivoFake = new MotivoMovimentacao { Id = 1, Motivo = "Venda" };

    public SaidaServiceTests()
    {
        _mockUow = new Mock<IUnityOfWork>();
        _mockSaidaRepo = new Mock<ISaidaRepository>();
        _mockMotivoRepo = new Mock<IMotivoMovimentacaoRepository>();
        _mockItemSaidaService = new Mock<IItemSaidaService>();
        _mockDateTimeProvider = new Mock<IDateTimeProvider>();

        // Configurar UoW
        _mockUow.Setup(uow => uow.Saida).Returns(_mockSaidaRepo.Object);
        _mockUow.Setup(uow => uow.MotivoMovimentacao).Returns(_mockMotivoRepo.Object);

        // Configurar Transações
        _mockUow.Setup(uow => uow.BeginTransactionAsync()).ReturnsAsync(Mock.Of<IDbContextTransaction>());
        _mockUow.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);
        _mockUow.Setup(uow => uow.RollbackAsync()).Returns(Task.CompletedTask);
        _mockUow.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

        // Configurar Data
        _mockDateTimeProvider.Setup(p => p.Today).Returns(_hojeFixo);
        _mockDateTimeProvider.Setup(p => p.Now).Returns(_hojeFixo.ToDateTime(_horaFixa));

        // Configurar Motivo (Existe ID=1)
        _mockMotivoRepo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(_motivoFake);
        _mockMotivoRepo.Setup(r => r.ObterPorIdAsync(It.IsNotIn(1))).ReturnsAsync((MotivoMovimentacao)null!);

        // Instanciar Serviço (Com dependências)
        _service = new SaidaService(_mockUow.Object, _mockItemSaidaService.Object, _mockDateTimeProvider.Object);
    }

    // ---------------------------------------------------------------------
    // Testes de Criação (Caminho Feliz)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "CriarSaida deve calcular valor final e salvar com sucesso")]
    public async Task CriarSaidaAsync_SemDesconto_DeveSalvarCorretamente()
    {
        // Arrange
        var novaSaida = new Saida
        {
            Id = 0,
            MotivoMovimentacaoId = 1,
            ValorTotal = 100m,
            Desconto = false,
            ItemSaida = new List<ItemSaida> { new ItemSaida { ProdutoId = 1, Quantidade = 1 } }
        };

        // Act
        var resultado = await _service.CriarSaidaAsync(novaSaida);

        // Assert
        Assert.Equal(100m, resultado.ValorFinal);
        Assert.Equal(0m, resultado.ValorDesconto); // Sem desconto = 0
        Assert.Equal(_hojeFixo, resultado.DataSaida); // Data deve ser a mockada

        _mockSaidaRepo.Verify(r => r.AdicionarAsync(novaSaida), Times.Once);
        _mockItemSaidaService.Verify(s => s.AdicionarItensSaidaAsync(It.IsAny<int>(), novaSaida.ItemSaida), Times.Once);
        _mockUow.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact(DisplayName = "CriarSaida com desconto válido deve abater do valor final")]
    public async Task CriarSaidaAsync_ComDescontoValido_DeveAbaterValor()
    {
        // Arrange
        var novaSaida = new Saida
        {
            Id = 0,
            MotivoMovimentacaoId = 1,
            ValorTotal = 100m,
            Desconto = true,
            ValorDesconto = 10m, // 10% é válido (< 50%)
            ItemSaida = new List<ItemSaida> { new ItemSaida() }
        };

        // Act
        var resultado = await _service.CriarSaidaAsync(novaSaida);

        // Assert
        Assert.Equal(90m, resultado.ValorFinal); // 100 - 10 = 90
        _mockUow.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    // ---------------------------------------------------------------------
    // Testes de Validação (Exceptions)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "Deve lançar InvalidOperationException se Motivo não existir")]
    public async Task CriarSaidaAsync_MotivoInexistente_DeveLancarExcecao()
    {
        // Arrange
        var saidaInvalida = new Saida { MotivoMovimentacaoId = 99 }; // ID 99 não existe

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarSaidaAsync(saidaInvalida));
        _mockUow.Verify(uow => uow.RollbackAsync(), Times.Once);
    }

    [Fact(DisplayName = "Deve lançar InvalidOperationException se não houver itens")]
    public async Task CriarSaidaAsync_SemItens_DeveLancarExcecao()
    {
        // Arrange
        var saidaSemItens = new Saida 
        { 
            MotivoMovimentacaoId = 1, 
            ItemSaida = new List<ItemSaida>() // Lista vazia
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarSaidaAsync(saidaSemItens));
        Assert.Equal("É obrigatório adicionar ao menos um item na saída", ex.Message);
        _mockUow.Verify(uow => uow.RollbackAsync(), Times.Once);
    }

    [Fact(DisplayName = "Deve lançar InvalidOperationException se desconto for negativo")]
    public async Task CriarSaidaAsync_DescontoNegativo_DeveLancarExcecao()
    {
        // Arrange
        var saidaDescontoNegativo = new Saida
        {
            MotivoMovimentacaoId = 1,
            ValorTotal = 100m,
            Desconto = true,
            ValorDesconto = -10m,
            ItemSaida = new List<ItemSaida> { new ItemSaida() }
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarSaidaAsync(saidaDescontoNegativo));
    }

    [Fact(DisplayName = "Deve lançar InvalidOperationException se desconto for maior que 50%")]
    public async Task CriarSaidaAsync_DescontoExcessivo_DeveLancarExcecao()
    {
        // Arrange
        var saidaDescontoAlto = new Saida
        {
            MotivoMovimentacaoId = 1,
            ValorTotal = 100m,
            Desconto = true,
            ValorDesconto = 51m, // > 50%
            ItemSaida = new List<ItemSaida> { new ItemSaida() }
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarSaidaAsync(saidaDescontoAlto));
        Assert.Contains("não pode ser maior que 50%", ex.Message);
    }
}