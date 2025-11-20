using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Service;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore.Storage;

namespace ApiHortifruti.Tests;

public class FuncionarioServiceTests
{
    private readonly Mock<IUnityOfWork> _mockUow;
    private readonly Mock<IFuncionarioRepository> _mockFuncionarioRepo;
    private readonly FuncionarioService _service;

    // Dados Fakes
    private readonly List<Funcionario> _funcionariosFake = new List<Funcionario>
    {
        new Funcionario { Id = 1, Nome = "João Silva", Cpf = "111.111.111-11", Ativo = true },
        new Funcionario { Id = 2, Nome = "Maria Oliveira", Cpf = "222.222.222-22", Ativo = true }
    };

    public FuncionarioServiceTests()
    {
        _mockUow = new Mock<IUnityOfWork>();
        _mockFuncionarioRepo = new Mock<IFuncionarioRepository>();

        // Configurar UoW
        _mockUow.Setup(uow => uow.Funcionario).Returns(_mockFuncionarioRepo.Object);

        // Configurações de Transação
        _mockUow.Setup(uow => uow.BeginTransactionAsync())
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());
        _mockUow.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);
        _mockUow.Setup(uow => uow.RollbackAsync()).Returns(Task.CompletedTask);
        _mockUow.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

        // Configurações de Consulta
        _mockFuncionarioRepo.Setup(r => r.ObterTodosAsync()).ReturnsAsync(_funcionariosFake);
        _mockFuncionarioRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                            .ReturnsAsync((int id) => _funcionariosFake.FirstOrDefault(f => f.Id == id));

        // Instanciar serviço
        _service = new FuncionarioService(_mockUow.Object);
    }

    // ---------------------------------------------------------------------
    // Testes de Consulta (GET)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "ObterTodosFuncionarios deve retornar a lista completa")]
    public async Task ObterTodosOsFuncionariosAsync_DeveRetornarTodos()
    {
        // Act
        var resultado = await _service.ObterTodosOsFuncionariosAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(_funcionariosFake.Count, resultado.Count());
        _mockFuncionarioRepo.Verify(r => r.ObterTodosAsync(), Times.Once);
    }

    [Fact(DisplayName = "ObterFuncionarioPorId deve retornar funcionário existente")]
    public async Task ObterFuncionarioPorIdAsync_DeveRetornarFuncionario()
    {
        // Arrange
        int idExistente = 1;

        // Act
        var resultado = await _service.ObterFuncionarioPorIdAsync(idExistente);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(idExistente, resultado.Id);
        _mockFuncionarioRepo.Verify(r => r.ObterPorIdAsync(idExistente), Times.Once);
    }

    // ---------------------------------------------------------------------
    // Testes de Criação (POST) - Com Transação
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "CriarFuncionario deve Adicionar, Salvar e Commitar a transação")]
    public async Task CriarFuncionarioAsync_Sucesso_DeveExecutarTransacaoCompleta()
    {
        // Arrange
        var novoFuncionario = new Funcionario { Id = 0, Nome = "Novo", Cpf = "333" };
        
        // Act
        var resultado = await _service.CriarFuncionarioAsync(novoFuncionario);

        // Assert
        Assert.NotNull(resultado);
        
        // Verifica fluxo da transação
        _mockUow.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
        _mockFuncionarioRepo.Verify(r => r.AdicionarAsync(novoFuncionario), Times.Once);
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mockUow.Verify(uow => uow.CommitAsync(), Times.Once);
        _mockUow.Verify(uow => uow.RollbackAsync(), Times.Never);
    }

    [Fact(DisplayName = "CriarFuncionario deve chamar Rollback se ocorrer erro")]
    public async Task CriarFuncionarioAsync_Erro_DeveChamarRollback()
    {
        // Arrange
        var novoFuncionario = new Funcionario { Id = 0, Nome = "Novo", Cpf = "333" };
        
        // Simula erro ao adicionar
        _mockFuncionarioRepo.Setup(r => r.AdicionarAsync(It.IsAny<Funcionario>()))
                            .ThrowsAsync(new Exception("Erro de banco de dados"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CriarFuncionarioAsync(novoFuncionario));

        // Assert
        _mockUow.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
        _mockUow.Verify(uow => uow.CommitAsync(), Times.Never);
        _mockUow.Verify(uow => uow.RollbackAsync(), Times.Once); // Rollback deve ser chamado
    }

    // ---------------------------------------------------------------------
    // Testes de Atualização (PUT)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "AtualizarFuncionario deve chamar AtualizarAsync e SaveChangesAsync")]
    public async Task AtualizarFuncionarioAsync_Sucesso_DeveAtualizar()
    {
        // Arrange
        int idAtualizar = 1;
        var funcionarioAtualizado = new Funcionario { Id = idAtualizar, Nome = "João Atualizado" };

        // Act
        await _service.AtualizarFuncionarioAsync(idAtualizar, funcionarioAtualizado);

        // Assert
        _mockFuncionarioRepo.Verify(r => r.AtualizarAsync(funcionarioAtualizado), Times.Once);
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Fact(DisplayName = "AtualizarFuncionario com IDs divergentes deve lançar ArgumentException")]
    public async Task AtualizarFuncionarioAsync_IdDiferente_DeveLancarExcecao()
    {
        // Arrange
        int idUrl = 10;
        var funcionarioBody = new Funcionario { Id = 20, Nome = "Teste" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.AtualizarFuncionarioAsync(idUrl, funcionarioBody));

        // Verifica que persistência não foi chamada
        _mockFuncionarioRepo.Verify(r => r.AtualizarAsync(It.IsAny<Funcionario>()), Times.Never);
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }

    // ---------------------------------------------------------------------
    // Testes de Deleção (DELETE)
    // ---------------------------------------------------------------------

    [Fact(DisplayName = "DeletarFuncionario deve chamar DeletarAsync e SaveChangesAsync")]
    public async Task DeletarFuncionarioAsync_Sucesso_DeveDeletar()
    {
        // Arrange
        int idDeletar = 1;

        // Act
        await _service.DeletarFuncionarioAsync(idDeletar);

        // Assert
        _mockFuncionarioRepo.Verify(r => r.DeletarAsync(idDeletar), Times.Once);
        _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}