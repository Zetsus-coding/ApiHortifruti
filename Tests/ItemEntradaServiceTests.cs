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

public class ItemEntradaServiceTests
{
    private readonly Mock<IUnityOfWork> _mockUow;
    private readonly Mock<IItemEntradaRepository> _mockItemEntradaRepo;
    private readonly Mock<IProdutoRepository> _mockProdutoRepo;
    private readonly ItemEntradaService _service;

    // Dados Fakes
    private readonly List<Produto> _produtosFake = new List<Produto>
    {
        // Produto 1: Estoque inicial 100
        new Produto { Id = 1, Nome = "Maçã", QuantidadeAtual = 100, Preco = 5.00m }, 
        // Produto 2: Estoque inicial 0
        new Produto { Id = 2, Nome = "Banana", QuantidadeAtual = 0, Preco = 2.00m } 
    };

    public ItemEntradaServiceTests()
    {
        _mockUow = new Mock<IUnityOfWork>();
        _mockItemEntradaRepo = new Mock<IItemEntradaRepository>();
        _mockProdutoRepo = new Mock<IProdutoRepository>();

        // Configurar o UoW para retornar os Repositórios mockados
        _mockUow.Setup(uow => uow.ItensEntrada).Returns(_mockItemEntradaRepo.Object);
        _mockUow.Setup(uow => uow.Produto).Returns(_mockProdutoRepo.Object);

        // Configurar comportamento padrão do ProdutoRepository (Simular banco de dados em memória)
        _mockProdutoRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                        .ReturnsAsync((int id) => _produtosFake.FirstOrDefault(p => p.Id == id));

        // Instanciar o serviço
        _service = new ItemEntradaService(_mockUow.Object);
    }

    // ---------------------------------------------------------------------
    // Testes de Sucesso (Caminho Feliz)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "AdicionarItens deve salvar itens e atualizar o estoque dos produtos")]
    public async Task AdicionarItensEntradaAsync_Sucesso_DeveAtualizarEstoque()
    {
        // Arrange
        int entradaId = 10;
        var itensParaAdicionar = new List<ItemEntrada>
        {
            new ItemEntrada { ProdutoId = 1, Quantidade = 50 }, // Vai somar 50 ao estoque de 100
            new ItemEntrada { ProdutoId = 2, Quantidade = 20 }  // Vai somar 20 ao estoque de 0
        };

        // Act
        await _service.AdicionarItensEntradaAsync(entradaId, itensParaAdicionar);

        // Assert
        
        // 1. Verifica se o método de adicionar itens foi chamado no repositório
        _mockItemEntradaRepo.Verify(r => r.AdicionarItensEntradaAsync(itensParaAdicionar), Times.Once);

        // 2. Verifica se o estoque dos produtos foi atualizado corretamente na memória
        var produto1 = _produtosFake.First(p => p.Id == 1);
        var produto2 = _produtosFake.First(p => p.Id == 2);

        Assert.Equal(150, produto1.QuantidadeAtual); // 100 + 50 = 150
        Assert.Equal(20, produto2.QuantidadeAtual);  // 0 + 20 = 20

        // 3. Verifica se o método de atualizar produto foi chamado para cada item
        _mockProdutoRepo.Verify(r => r.AtualizarAsync(It.IsAny<Produto>()), Times.Exactly(2));
    }

    // ---------------------------------------------------------------------
    // Testes de Validação (Exceptions)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "Deve lançar InvalidOperationException se a lista de itens for nula")]
    public async Task AdicionarItensEntradaAsync_ListaNula_DeveLancarExcecao()
    {
        // Arrange
        List<ItemEntrada>? itensNulos = null;

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.AdicionarItensEntradaAsync(1, itensNulos!));
    }

    [Fact(DisplayName = "Deve lançar InvalidOperationException se a lista de itens estiver vazia")]
    public async Task AdicionarItensEntradaAsync_ListaVazia_DeveLancarExcecao()
    {
        // Arrange
        var itensVazios = new List<ItemEntrada>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.AdicionarItensEntradaAsync(1, itensVazios));
    }

    [Fact(DisplayName = "Deve lançar InvalidOperationException se algum item tiver quantidade <= 0")]
    public async Task AdicionarItensEntradaAsync_QuantidadeInvalida_DeveLancarExcecao()
    {
        // Arrange
        var itensInvalidos = new List<ItemEntrada>
        {
            new ItemEntrada { ProdutoId = 1, Quantidade = 10 },
            new ItemEntrada { ProdutoId = 2, Quantidade = 0 } // Item inválido
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.AdicionarItensEntradaAsync(1, itensInvalidos));
        
        Assert.Equal("A quantidade de todos os itens deve ser maior que zero", exception.Message);
        
        // Garante que nada foi chamado nos repositórios
        _mockItemEntradaRepo.Verify(r => r.AdicionarItensEntradaAsync(It.IsAny<IEnumerable<ItemEntrada>>()), Times.Never);
    }

    [Fact(DisplayName = "Deve lançar KeyNotFoundException se o produto não existir")]
    public async Task AdicionarItensEntradaAsync_ProdutoInexistente_DeveLancarExcecao()
    {
        // Arrange
        var itensComProdutoInexistente = new List<ItemEntrada>
        {
            new ItemEntrada { ProdutoId = 99, Quantidade = 10 } // ID 99 não existe no _produtosFake
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.AdicionarItensEntradaAsync(1, itensComProdutoInexistente));
        
        // Verifica que tentou adicionar os itens (pois a validação de produto ocorre depois),
        // mas falhou ao tentar atualizar o produto.
        _mockItemEntradaRepo.Verify(r => r.AdicionarItensEntradaAsync(It.IsAny<IEnumerable<ItemEntrada>>()), Times.Once);
        _mockProdutoRepo.Verify(r => r.AtualizarAsync(It.IsAny<Produto>()), Times.Never);
    }
}