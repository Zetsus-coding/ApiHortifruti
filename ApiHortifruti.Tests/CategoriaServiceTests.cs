using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service;
using ApiHortifruti.Exceptions;
using Moq;
using Xunit;

namespace ApiHortifruti.Tests;

public class CategoriaServiceTests
{
    // Mock do Unit of Work para simular a camada de Repository
    private readonly Mock<IUnityOfWork> _mockUow;
    private readonly CategoriaService _service;

    // Dados de teste
    private readonly List<Categoria> _categoriasFake = new List<Categoria>
    {
        new Categoria { Id = 1, Nome = "Frutas" },
        new Categoria { Id = 2, Nome = "Verduras" }
    };

    public CategoriaServiceTests()
    {
        
        _mockUow = new Mock<IUnityOfWork>();

        // Configurar o Mock do Repositório (que está dentro do UoW)
        var mockCategoriaRepository = new Mock<ICategoriaRepository>();

        // Configurar o comportamento do método ObterTodosAsync
        mockCategoriaRepository.Setup(r => r.ObterTodosAsync())
        .ReturnsAsync(_categoriasFake);

        // Configurar o comportamento do método ObterPorIdAsync
        mockCategoriaRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
        .ReturnsAsync((int id) => _categoriasFake.FirstOrDefault(c => c.Id == id));

        // Configurar o Mock do UoW para retornar o Repositório de Categoria
        _mockUow.Setup(uow => uow.Categoria).Returns(mockCategoriaRepository.Object);


        // Instanciar o serviço injetando o mock do UoW
        _service = new CategoriaService(_mockUow.Object);
    }

    // ---------------------------------------------------------------------
    // Testes para ObterTodosCategoriasAsync
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "ObterTodos deve retornar a lista completa de categorias")]
    public async Task ObterTodosCategoriasAsync_DeveRetornarTodasCategorias()
    {
        // Act
        var resultado = await _service.ObterTodasAsCategoriasAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(_categoriasFake.Count, resultado.Count());
        
        // Verificar se o método correto no repositório foi chamado
        _mockUow.Verify(uow => uow.Categoria.ObterTodosAsync(), Times.Once);
    }

    [Fact(DisplayName = "ObterTodos deve propagar exceção em caso de falha no repositório")]
    public async Task ObterTodosCategoriasAsync_DevePropagarExcecao()
    {
        // Arrange
        // Configurar o mock para lançar uma exceção ao chamar o método do repositório
        _mockUow.Setup(uow => uow.Categoria.ObterTodosAsync())
                .ThrowsAsync(new Exception("Erro de teste simulado"));

        // Act & Assert
        // O teste deve garantir que a exceção seja lançada (propagada) pelo serviço
        await Assert.ThrowsAsync<Exception>(() => _service.ObterTodasAsCategoriasAsync());

        // Verificar se o método correto no repositório foi chamado
        _mockUow.Verify(uow => uow.Categoria.ObterTodosAsync(), Times.Once);
    }
    
    // ---------------------------------------------------------------------
    // Testes para ObterCategoriaPorIdAsync
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "ObterPorId com ID válido deve retornar a categoria")]
    public async Task ObterCategoriaPorIdAsync_ComIdValido_DeveRetornarCategoria()
    {
        // Arrange
        int idProcurado = 1;
        
        // Act
        var resultado = await _service.ObterCategoriaPorIdAsync(idProcurado);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(idProcurado, resultado.Id);
        
        // Verificar se o método correto no repositório foi chamado
        _mockUow.Verify(uow => uow.Categoria.ObterPorIdAsync(idProcurado), Times.Once);
    }

    [Fact(DisplayName = "ObterPorId com ID inexistente deve retornar nulo")]
    public async Task ObterCategoriaPorIdAsync_ComIdInexistente_DeveRetornarNulo()
    {
        // Arrange
        int idInexistente = 99;
        
        // Act
        var resultado = await _service.ObterCategoriaPorIdAsync(idInexistente);

        // Assert
        Assert.Null(resultado);
        
        // Verificar se o método correto no repositório foi chamado
        _mockUow.Verify(uow => uow.Categoria.ObterPorIdAsync(idInexistente), Times.Once);
    }
    
    // ---------------------------------------------------------------------
    // Testes para CriarCategoriaAsync
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "CriarCategoria deve chamar Adicionar e SaveChanges e retornar o objeto")]
    public async Task CriarCategoriaAsync_DeveChamarAdicionarESaveChanges()
    {
        // Arrange
        var novaCategoria = new Categoria { Nome = "Raizes" };
        
        // Act
        var resultado = await _service.CriarCategoriaAsync(novaCategoria);
        
        // Assert
        Assert.Equal(novaCategoria, resultado);
        
        // Verificar se o método AdicionarAsync do repositório foi chamado com o objeto correto
        _mockUow.Verify(uow => uow.Categoria.AdicionarAsync(novaCategoria), Times.Once);
        
        // Verificar se o SaveChangesAsync do Unit of Work foi chamado (Persistência)
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    // ---------------------------------------------------------------------
    // Testes para AtualizarCategoriaAsync
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "AtualizarCategoria com IDs correspondentes deve chamar Atualizar e SaveChanges")]
    public async Task AtualizarCategoriaAsync_ComIdsCorretos_DeveChamarAtualizarESaveChanges()
    {
        // Arrange
        int idAtualizar = 1;
        var categoriaAtualizada = new Categoria { Id = idAtualizar, Nome = "Frutas Frescas" };
        
        // Act
        await _service.AtualizarCategoriaAsync(idAtualizar, categoriaAtualizada);

        // Assert
        // Verificar se o método AtualizarAsync do repositório foi chamado com o objeto correto
        _mockUow.Verify(uow => uow.Categoria.AtualizarAsync(categoriaAtualizada), Times.Once);
        
        // Verificar se o SaveChangesAsync do Unit of Work foi chamado
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Fact(DisplayName = "AtualizarCategoria com IDs divergentes deve lançar ArgumentException")]
    public async Task AtualizarCategoriaAsync_ComIdsDiferentes_DeveLancarExcecao()
    {
        // Arrange
        int idUrl = 10;
        var categoriaComIdDiferente = new Categoria { Id = 20, Nome = "Teste" };
        
        // Act & Assert
        // O teste deve garantir que o serviço lança a exceção de lógica de negócios (validação de ID)
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.AtualizarCategoriaAsync(idUrl, categoriaComIdDiferente));

        _mockUow.Verify(uow => uow.Categoria.AtualizarAsync(It.IsAny<Categoria>()), Times.Never);
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }

    // ---------------------------------------------------------------------
    // Testes para DeletarCategoriaAsync
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "DeletarCategoria com ID existente deve chamar Deletar e SaveChanges")]
    public async Task DeletarCategoriaAsync_ComIdExistente_DeveChamarDeletarESaveChanges()
    {
        // Arrange
        int idParaDeletar = 1; // ID que já existe na lista _categoriasFake (Frutas)
        
        // Precisamos pegar a referência do objeto que o Mock vai retornar para garantir que é o mesmo passado para o Deletar
        var categoriaEsperada = _categoriasFake.First(c => c.Id == idParaDeletar);

        // Act
        await _service.DeletarCategoriaAsync(idParaDeletar);

        // Assert
        // Verifica se o serviço buscou a categoria pelo ID antes de deletar
        _mockUow.Verify(uow => uow.Categoria.ObterPorIdAsync(idParaDeletar), Times.Once);

        // Verifica se o método DeletarAsync foi chamado passando exatamente o objeto retornado
        _mockUow.Verify(uow => uow.Categoria.DeletarAsync(categoriaEsperada), Times.Once);

        // Verifica se as alterações foram persistidas
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Fact(DisplayName = "DeletarCategoria com ID inexistente deve lançar NotFoundException")]
    public async Task DeletarCategoriaAsync_ComIdInexistente_DeveLancarNotFoundException()
    {
        // Arrange
        int idInexistente = 99; // Um ID que não está na lista _categoriasFake

        // Act & Assert
        // Verifica se lança a exceção personalizada NotFoundException
        await Assert.ThrowsAsync<ApiHortifruti.Exceptions.NotFoundException>(
            () => _service.DeletarCategoriaAsync(idInexistente));

        // Verificações de segurança para garantir que nada foi alterado no banco
        _mockUow.Verify(uow => uow.Categoria.ObterPorIdAsync(idInexistente), Times.Once); // Tentou buscar
        _mockUow.Verify(uow => uow.Categoria.DeletarAsync(It.IsAny<Categoria>()), Times.Never); // NÃO tentou deletar
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never); // NÃO tentou salvar
    }
}