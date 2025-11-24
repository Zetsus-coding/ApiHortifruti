// using ApiHortifruti.Controllers;
// using ApiHortifruti.Data.Repository.Interfaces;
// using ApiHortifruti.Domain;
// using ApiHortifruti.Service;
// using ApiHortifruti.Service.Interfaces;
// using Moq;
// using Xunit;

// namespace ApiHortifruti.Tests;

// public class FornecedorServiceTests
// {
//     private readonly Mock<IUnityOfWork> _mockUow;
//     private readonly Mock<IFornecedorRepository> _mockFornecedorRepo;
//     private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
//     private readonly FornecedorService _service;
    
//     private readonly DateOnly _hojeAtual = DateOnly.FromDateTime(DateTime.Today);

//     private readonly List<Fornecedor> _fornecedoresFake = new List<Fornecedor>
//     {
//         new Fornecedor { Id = 1, NomeFantasia = "Fazenda Bom Fruto", CadastroPessoa = "11111111000100", DataRegistro = new DateOnly(2024, 1, 1) },
//         new Fornecedor { Id = 2, NomeFantasia = "Distribuidora Verde", CadastroPessoa = "22222222000200", DataRegistro = new DateOnly(2024, 5, 15) }
//     };

//     public FornecedorServiceTests()
//     {
//         _mockUow = new Mock<IUnityOfWork>();
//         _mockFornecedorRepo = new Mock<IFornecedorRepository>();
//         _mockDateTimeProvider = new Mock<IDateTimeProvider>();

//         // Configurar o UoW para retornar o Repositório de Fornecedor
//         _mockUow.Setup(uow => uow.Fornecedor).Returns(_mockFornecedorRepo.Object);

//         // Configurar o Mock do IDateTimeProvider
//         _mockDateTimeProvider.Setup(p => p.Today).Returns(_hojeAtual);

//         // Configurações Comuns (ObterTodos e ObterPorId)
//         _mockFornecedorRepo.Setup(r => r.ObterTodosAsync())
//                            .ReturnsAsync(_fornecedoresFake);

//         _mockFornecedorRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
//                            .ReturnsAsync((int id) => _fornecedoresFake.FirstOrDefault(f => f.Id == id));
        
//         // Instanciar o serviço (Assumindo a injeção do IDateTimeProvider)
//         _service = new FornecedorService(_mockUow.Object);
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Consulta (GET)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "ObterTodosOsFornecedores deve retornar a lista completa")]
//     public async Task ObterTodosOsFornecedoresAsync_DeveRetornarTodos()
//     {
//         // Act
//         var resultado = await _service.ObterTodosOsFornecedoresAsync();

//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal(_fornecedoresFake.Count, resultado.Count());
//         _mockFornecedorRepo.Verify(r => r.ObterTodosAsync(), Times.Once);
//     }

//     [Fact(DisplayName = "ObterFornecedorPorId deve retornar o fornecedor correto")]
//     public async Task ObterFornecedorPorIdAsync_DeveRetornarFornecedorExistente()
//     {
//         // Arrange
//         int idExistente = 1;

//         // Act
//         var resultado = await _service.ObterFornecedorPorIdAsync(idExistente);

//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal(idExistente, resultado.Id);
//         _mockFornecedorRepo.Verify(r => r.ObterPorIdAsync(idExistente), Times.Once);
//     }

//     [Fact(DisplayName = "ObterFornecedorPorId deve retornar nulo para ID inexistente")]
//     public async Task ObterFornecedorPorIdAsync_DeveRetornarNuloParaInexistente()
//     {
//         // Arrange
//         int idInexistente = 99;

//         // Act
//         var resultado = await _service.ObterFornecedorPorIdAsync(idInexistente);

//         // Assert
//         Assert.Null(resultado);
//         _mockFornecedorRepo.Verify(r => r.ObterPorIdAsync(idInexistente), Times.Once);
//     }

//     // ---------------------------------------------------------------------
//     // Teste de Criação (POST)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "CriarFornecedor deve adicionar e retornar o objeto com DataRegistro definida")]
//     public async Task CriarFornecedorAsync_DeveAdicionarEIncluirDataRegistro()
//     {
//         // Arrange
//         var novoFornecedor = new Fornecedor { Id = 0, NomeFantasia = "Novo Fornecedor", CadastroPessoa = "99999999000100" };
        
//         // Configurar o mock para retornar o objeto adicionado (com o Id e a DataRegistro)
//         _mockFornecedorRepo
//             .Setup(r => r.AdicionarAsync(It.IsAny<Fornecedor>()))
//             .ReturnsAsync((Fornecedor f) => 
//             {
//                 // Simula o que o serviço faz: define a data
//                 // Se o IDateTimeProvider NÃO foi injetado: f.DataRegistro = DateOnly.FromDateTime(DateTime.Now);
//                 f.DataRegistro = _hojeAtual; // Assumindo a refatoração ou alinhamento com _hojeFixo
//                 return f;
//             });
        
//         // Act
//         var resultado = await _service.CriarFornecedorAsync(novoFornecedor);

//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal("Novo Fornecedor", resultado.NomeFantasia);
//         // Verifica se a DataRegistro foi definida pelo serviço
//         Assert.Equal(_hojeAtual, resultado.DataRegistro); 
        
//         _mockFornecedorRepo.Verify(r => r.AdicionarAsync(novoFornecedor), Times.Once);
        
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Atualização (PUT)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "AtualizarFornecedor deve chamar AtualizarAsync e SaveChangesAsync")]
//     public async Task AtualizarFornecedorAsync_DeveAtualizar()
//     {
//         // Arrange
//         int idAtualizar = 1;
//         var fornecedorAtualizado = new Fornecedor { Id = idAtualizar, NomeFantasia = "Fazenda Bom Fruto - Atualizada", CadastroPessoa = "11111111000100" };
        
//         // Act
//         await _service.AtualizarFornecedorAsync(idAtualizar, fornecedorAtualizado);

//         // Assert
//         _mockFornecedorRepo.Verify(r => r.AtualizarAsync(fornecedorAtualizado), Times.Once);
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
//     }
    
//     [Fact(DisplayName = "AtualizarFornecedor com IDs divergentes deve lançar ArgumentException")]
//     public async Task AtualizarFornecedorAsync_ComIdsDiferentes_DeveLancarExcecao()
//     {
//         // Arrange
//         int idUrl = 10;
//         var fornecedorComIdDiferente = new Fornecedor { Id = 20, NomeFantasia = "Teste" };
        
//         // Act & Assert
//         // O teste deve garantir que o serviço lança a exceção se a validação falhar
//         await Assert.ThrowsAsync<ArgumentException>(
//             () => _service.AtualizarFornecedorAsync(idUrl, fornecedorComIdDiferente));

//         // Assert (Verificar que NENHUM método de persistência foi chamado)
//         _mockFornecedorRepo.Verify(r => r.AtualizarAsync(It.IsAny<Fornecedor>()), Times.Never);
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never);
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Exclusão (DELETE)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "DeletarFornecedor com ID existente deve chamar Deletar e SaveChanges")]
//     public async Task DeletarFornecedorAsync_ComIdExistente_DeveChamarDeletarESaveChanges()
//     {
//         // Arrange
//         int idParaDeletar = 1; // ID que existe na lista _fornecedoresFake
//         var fornecedorEsperado = _fornecedoresFake.First(f => f.Id == idParaDeletar);

//         // Act
//         await _service.DeletarFornecedorAsync(idParaDeletar);

//         // Assert
//         // Verifica se buscou o fornecedor antes
//         _mockFornecedorRepo.Verify(r => r.ObterPorIdAsync(idParaDeletar), Times.Once);

//         // Verifica se chamou o método de deletar passando o objeto correto
//         _mockFornecedorRepo.Verify(r => r.DeletarAsync(fornecedorEsperado), Times.Once);

//         // Verifica se salvou as alterações no banco
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
//     }

//     [Fact(DisplayName = "DeletarFornecedor com ID inexistente deve lançar NotFoundException")]
//     public async Task DeletarFornecedorAsync_ComIdInexistente_DeveLancarNotFoundException()
//     {
//         // Arrange
//         int idInexistente = 99; // ID que não está na lista _fornecedoresFake

//         // Act & Assert
//         await Assert.ThrowsAsync<ApiHortifruti.Exceptions.NotFoundException>(
//             () => _service.DeletarFornecedorAsync(idInexistente));

//         // Verifica se NÃO tentou deletar nem salvar
//         _mockFornecedorRepo.Verify(r => r.DeletarAsync(It.IsAny<Fornecedor>()), Times.Never);
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never);
//     }
// }