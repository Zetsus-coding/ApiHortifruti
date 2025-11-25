// using ApiHortifruti.Data.Repository.Interfaces;
// using ApiHortifruti.Domain;
// using ApiHortifruti.Service;
// using Moq;
// using Xunit;

// namespace ApiHortifruti.Tests;

// public class HistoricoProdutoServiceTests
// {
//     private readonly Mock<IUnityOfWork> _mockUow;
//     private readonly Mock<IHistoricoProdutoRepository> _mockHistoricoRepo;
//     private readonly HistoricoProdutoService _service;

//     // Dados Fakes
//     private readonly List<HistoricoProduto> _historicoFake = new List<HistoricoProduto>
//     {
//         // Histórico do Produto 1
//         new HistoricoProduto { Id = 1, ProdutoId = 1, PrecoProduto = 5.00m, DataAlteracao = new DateOnly(2024, 1, 1) },
//         new HistoricoProduto { Id = 2, ProdutoId = 1, PrecoProduto = 6.00m, DataAlteracao = new DateOnly(2024, 2, 1) },
//         // Histórico do Produto 2
//         new HistoricoProduto { Id = 3, ProdutoId = 2, PrecoProduto = 2.50m, DataAlteracao = new DateOnly(2024, 1, 15) }
//     };

//     public HistoricoProdutoServiceTests()
//     {
//         _mockUow = new Mock<IUnityOfWork>();
//         _mockHistoricoRepo = new Mock<IHistoricoProdutoRepository>();

//         // Configurar UoW
//         _mockUow.Setup(uow => uow.HistoricoProduto).Returns(_mockHistoricoRepo.Object);
//         _mockUow.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

//         // Configurações Comuns de Consulta
//         _mockHistoricoRepo.Setup(r => r.ObterTodosAsync()).ReturnsAsync(_historicoFake);
//         _mockHistoricoRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
//                           .ReturnsAsync((int id) => _historicoFake.FirstOrDefault(h => h.Id == id));

//         // Instanciar serviço
//         _service = new HistoricoProdutoService(_mockUow.Object);
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Consulta (GET)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "ObterTodosHistoricos deve retornar a lista completa")]
//     public async Task ObterTodosOsHistoricosProdutosAsync_DeveRetornarTodos()
//     {
//         // Act
//         var resultado = await _service.ObterTodosOsHistoricosProdutosAsync();

//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal(_historicoFake.Count, resultado.Count());
//         _mockHistoricoRepo.Verify(r => r.ObterTodosAsync(), Times.Once);
//     }

//     [Fact(DisplayName = "ObterHistoricoPorId deve retornar histórico existente")]
//     public async Task ObterHistoricoProdutoPorIdAsync_DeveRetornarHistorico()
//     {
//         // Arrange
//         int idExistente = 1;

//         // Act
//         var resultado = await _service.ObterHistoricoProdutoPorIdAsync(idExistente);

//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal(idExistente, resultado.Id);
//         _mockHistoricoRepo.Verify(r => r.ObterPorIdAsync(idExistente), Times.Once);
//     }

//     // ---------------------------------------------------------------------
//     // Teste da Funcionalidade Faltante (Por Produto ID)
//     // ---------------------------------------------------------------------
    
//     // ⚠️ Este teste só passará se você implementar o método no Service e no Repository
//     // No momento, seu Service lança NotImplementedException.
//     // Se quiser testar agora, precisará adicionar o Setup para um método customizado no mock.
//     /*
//     [Fact(DisplayName = "ObterHistoricoPorProdutoId deve retornar lista filtrada por produto")]
//     public async Task ObterHistoricoProdutoPorProdutoIdAsync_DeveRetornarFiltrado()
//     {
//         // Arrange
//         int produtoId = 1;
//         var historicoFiltrado = _historicoFake.Where(h => h.ProdutoId == produtoId).ToList();
        
//         // Você precisa garantir que IHistoricoProdutoRepository tenha esse método definido
//         // _mockHistoricoRepo.Setup(r => r.ObterPorProdutoIdAsync(produtoId)).ReturnsAsync(historicoFiltrado);

//         // Act
//         var resultado = await _service.ObterHistoricoProdutoPorProdutoIdAsync(produtoId);

//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal(2, resultado.Count()); // Espera 2 registros para o Produto 1
//         Assert.All(resultado, h => Assert.Equal(produtoId, h.ProdutoId));
//     }
//     */

//     // ---------------------------------------------------------------------
//     // Testes de Escrita (POST/PUT/DELETE) - Validando UoW
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "CriarHistorico deve chamar Adicionar e SaveChanges")]
//     public async Task CriarHistoricoProdutoAsync_DeveSalvar()
//     {
//         // Arrange
//         var novoHistorico = new HistoricoProduto { Id = 0, ProdutoId = 1, PrecoProduto = 10m };
//         _mockHistoricoRepo.Setup(r => r.AdicionarAsync(It.IsAny<HistoricoProduto>())).ReturnsAsync(novoHistorico);

//         // Act
//         var resultado = await _service.CriarHistoricoProdutoAsync(novoHistorico);

//         // Assert
//         _mockHistoricoRepo.Verify(r => r.AdicionarAsync(novoHistorico), Times.Once);
//         // Se você aplicar a refatoração no Service, descomente a linha abaixo:
//         // _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once); 
//     }

//     // [Fact(DisplayName = "AtualizarHistorico deve chamar Atualizar e SaveChanges")]
//     // public async Task AtualizarHistoricoProdutoAsync_DeveAtualizar()
//     // {
//     //     // Arrange
//     //     int idAtualizar = 1;
//     //     var historicoAtualizado = new HistoricoProduto { Id = idAtualizar, ProdutoId = 1, PrecoProduto = 12m };

//     //     // Act
//     //     await _service.AtualizarHistoricoProdutoAsync(idAtualizar, historicoAtualizado);

//     //     // Assert
//     //     _mockHistoricoRepo.Verify(r => r.AtualizarAsync(historicoAtualizado), Times.Once);
//     //     // Se você aplicar a refatoração no Service, descomente a linha abaixo:
//     //     // _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
//     // }

//     // [Fact(DisplayName = "DeletarHistorico deve chamar Deletar e SaveChanges")]
//     // public async Task DeletarHistoricoProdutoAsync_DeveDeletar()
//     // {
//     //     // Arrange
//     //     int idDeletar = 1;

//     //     // Act
//     //     await _service.DeletarHistoricoProdutoAsync(idDeletar);

//     //     // Assert
//     //     _mockHistoricoRepo.Verify(r => r.DeletarAsync(idDeletar), Times.Once);
//     //     // Se você aplicar a refatoração no Service, descomente a linha abaixo:
//     //     // _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
//     // }
// }