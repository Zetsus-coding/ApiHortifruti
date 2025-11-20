using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ApiHortifruti.Tests;

public class ItemSaidaServiceTests
{
    private readonly Mock<IUnityOfWork> _mockUow;
    private readonly Mock<IItemSaidaRepository> _mockItemSaidaRepo;
    private readonly Mock<IProdutoRepository> _mockProdutoRepo;
    private readonly ItemSaidaService _service;

    // Dados Fakes
    private readonly List<Produto> _produtosFake = new List<Produto>
    {
        // Produto 1: Estoque 100
        new Produto { Id = 1, Nome = "Maçã", QuantidadeAtual = 100, Preco = 5.00m }, 
        // Produto 2: Estoque 10
        new Produto { Id = 2, Nome = "Banana", QuantidadeAtual = 10, Preco = 2.00m } 
    };

    public ItemSaidaServiceTests()
    {
        _mockUow = new Mock<IUnityOfWork>();
        _mockItemSaidaRepo = new Mock<IItemSaidaRepository>();
        _mockProdutoRepo = new Mock<IProdutoRepository>();

        // Configurar UoW
        _mockUow.Setup(uow => uow.ItensSaida).Returns(_mockItemSaidaRepo.Object);
        _mockUow.Setup(uow => uow.Produto).Returns(_mockProdutoRepo.Object);

        // Configurar ProdutoRepository para retornar dados em memória
        _mockProdutoRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                        .ReturnsAsync((int id) => _produtosFake.FirstOrDefault(p => p.Id == id));

        // Instanciar serviço
        _service = new ItemSaidaService(_mockUow.Object);
    }

    // ---------------------------------------------------------------------
    // Testes de Sucesso (Caminho Feliz)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "AdicionarItensSaida deve salvar itens e DECREMENTAR o estoque")]
    public async Task AdicionarItensSaidaAsync_Sucesso_DeveBaixarEstoque()
    {
        // Arrange
        int saidaId = 10;
        var itensParaAdicionar = new List<ItemSaida>
        {
            new ItemSaida { ProdutoId = 1, Quantidade = 20 }, // 100 - 20 = 80
            new ItemSaida { ProdutoId = 2, Quantidade = 5 }   // 10 - 5 = 5
        };

        // Act
        await _service.AdicionarItensSaidaAsync(saidaId, itensParaAdicionar);

        // Assert
        
        // 1. Verifica se o repositório de itens foi chamado
        _mockItemSaidaRepo.Verify(r => r.AdicionarItensSaidaAsync(itensParaAdicionar), Times.Once);

        // 2. Verifica se o estoque foi atualizado na memória fake
        var produto1 = _produtosFake.First(p => p.Id == 1);
        var produto2 = _produtosFake.First(p => p.Id == 2);

        Assert.Equal(80, produto1.QuantidadeAtual);
        Assert.Equal(5, produto2.QuantidadeAtual);

        // 3. Verifica se o update do produto foi persistido
        _mockProdutoRepo.Verify(r => r.AtualizarAsync(It.IsAny<Produto>()), Times.Exactly(2));
    }

    // ---------------------------------------------------------------------
    // Testes de Validação de Negócio (Estoque)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "Deve lançar InvalidOperationException se Estoque for insuficiente")]
    public async Task AdicionarItensSaidaAsync_EstoqueInsuficiente_DeveLancarExcecao()
    {
        // Arrange
        var itensSemEstoque = new List<ItemSaida>
        {
            // Produto 2 tem apenas 10 no estoque, tentando tirar 15
            new ItemSaida { ProdutoId = 2, Quantidade = 15 } 
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.AdicionarItensSaidaAsync(1, itensSemEstoque));

        Assert.Contains("Estoque insuficiente", exception.Message);

        // Verifica que o produto NÃO foi atualizado
        _mockProdutoRepo.Verify(r => r.AtualizarAsync(It.IsAny<Produto>()), Times.Never);
    }

    [Fact(DisplayName = "Deve lançar InvalidOperationException se Produto não existir")]
    public async Task AdicionarItensSaidaAsync_ProdutoInexistente_DeveLancarExcecao()
    {
        // Arrange
        var itensInvalidos = new List<ItemSaida>
        {
            new ItemSaida { ProdutoId = 99, Quantidade = 1 } 
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.AdicionarItensSaidaAsync(1, itensInvalidos));
        
        _mockProdutoRepo.Verify(r => r.AtualizarAsync(It.IsAny<Produto>()), Times.Never);
    }

    // ---------------------------------------------------------------------
    // Testes de Validação Básica (Lista)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "Deve lançar InvalidOperationException se lista for nula")]
    public async Task AdicionarItensSaidaAsync_ListaNula_DeveLancarExcecao()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.AdicionarItensSaidaAsync(1, null!));
    }

    [Fact(DisplayName = "Deve lançar InvalidOperationException se lista for vazia")]
    public async Task AdicionarItensSaidaAsync_ListaVazia_DeveLancarExcecao()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.AdicionarItensSaidaAsync(1, new List<ItemSaida>()));
    }

    [Fact(DisplayName = "Deve lançar InvalidOperationException se quantidade for <= 0")]
    public async Task AdicionarItensSaidaAsync_QuantidadeZero_DeveLancarExcecao()
    {
        var itensZerados = new List<ItemSaida>
        {
            new ItemSaida { ProdutoId = 1, Quantidade = 0 }
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.AdicionarItensSaidaAsync(1, itensZerados));
    }
}