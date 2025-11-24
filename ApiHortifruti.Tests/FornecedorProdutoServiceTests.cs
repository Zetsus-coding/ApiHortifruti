// using ApiHortifruti.Data.Repository.Interfaces;
// using ApiHortifruti.Domain;
// using ApiHortifruti.Service;
// using ApiHortifruti.Service.Interfaces; // Ajuste para o namespace da sua interface IDateTimeProvider
// using Moq;
// using Xunit;
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using System.Linq;
// using Microsoft.EntityFrameworkCore.Storage;

// namespace ApiHortifruti.Tests;

// public class FornecedorProdutoServiceTests
// {
//     private readonly Mock<IUnityOfWork> _mockUow;
//     private readonly Mock<IFornecedorProdutoRepository> _mockFornProdRepo;
//     private readonly Mock<IFornecedorRepository> _mockFornecedorRepo;
//     private readonly Mock<IProdutoRepository> _mockProdutoRepo;
//     private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
//     private readonly FornecedorProdutoService _service;

//     // Data Fixa
//     private readonly DateOnly _hojeFixo = new DateOnly(2025, 11, 19);

//     // Dados Fakes
//     private readonly Fornecedor _fornecedorFake = new Fornecedor { Id = 1, NomeFantasia = "Fornecedor A" };
//     private readonly Produto _produtoFake = new Produto { Id = 1, Nome = "Produto A" };
//     private readonly List<FornecedorProduto> _relacoesFake = new List<FornecedorProduto>
//     {
//         new FornecedorProduto { FornecedorId = 1, ProdutoId = 1, Disponibilidade = true },
//         new FornecedorProduto { FornecedorId = 2, ProdutoId = 2, Disponibilidade = false }
//     };

//     public FornecedorProdutoServiceTests()
//     {
//         _mockUow = new Mock<IUnityOfWork>();
//         _mockFornProdRepo = new Mock<IFornecedorProdutoRepository>();
//         _mockFornecedorRepo = new Mock<IFornecedorRepository>();
//         _mockProdutoRepo = new Mock<IProdutoRepository>();
//         _mockDateTimeProvider = new Mock<IDateTimeProvider>();

//         // Configurar UoW
//         _mockUow.Setup(uow => uow.FornecedorProduto).Returns(_mockFornProdRepo.Object);
//         _mockUow.Setup(uow => uow.Fornecedor).Returns(_mockFornecedorRepo.Object);
//         _mockUow.Setup(uow => uow.Produto).Returns(_mockProdutoRepo.Object);

//         // Configurar Transações
//         _mockUow.Setup(uow => uow.BeginTransactionAsync()).ReturnsAsync(Mock.Of<IDbContextTransaction>());
//         _mockUow.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);
//         _mockUow.Setup(uow => uow.RollbackAsync()).Returns(Task.CompletedTask);
//         _mockUow.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

//         // Configurar DataProvider
//         _mockDateTimeProvider.Setup(p => p.Today).Returns(_hojeFixo);

//         // Configurar Consultas Básicas
//         _mockFornProdRepo.Setup(r => r.ObterTodosAsync()).ReturnsAsync(_relacoesFake);
//         _mockFornProdRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>(), It.IsAny<int>()))
//                          .ReturnsAsync((int fid, int pid) => _relacoesFake.FirstOrDefault(fp => fp.FornecedorId == fid && fp.ProdutoId == pid));
        
//         // Configurar Dependências (Sempre encontra ID 1)
//         _mockFornecedorRepo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(_fornecedorFake);
//         _mockFornecedorRepo.Setup(r => r.ObterPorIdAsync(It.IsNotIn(1))).ReturnsAsync((Fornecedor)null!);

//         _mockProdutoRepo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(_produtoFake);
//         _mockProdutoRepo.Setup(r => r.ObterPorIdAsync(It.IsNotIn(1))).ReturnsAsync((Produto)null!);

//         // Instanciar Serviço
//         _service = new FornecedorProdutoService(_mockUow.Object, _mockDateTimeProvider.Object);
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Consulta
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "ObterTodos deve retornar lista completa")]
//     public async Task ObterTodosOsFornecedorProdutoAsync_DeveRetornarTodos()
//     {
//         var resultado = await _service.ObterTodosOsFornecedorProdutoAsync();
//         Assert.Equal(_relacoesFake.Count, resultado.Count());
//         _mockFornProdRepo.Verify(r => r.ObterTodosAsync(), Times.Once);
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Criação Única
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "Criar deve salvar, commitar e definir data se válido")]
//     public async Task CriarFornecedorProdutoAsync_Sucesso_DeveSalvar()
//     {
//         // Arrange (IDs diferentes de 1 para não conflitar com a lista fake de "existentes")
//         _mockFornecedorRepo.Setup(r => r.ObterPorIdAsync(10)).ReturnsAsync(new Fornecedor { Id = 10 });
//         _mockProdutoRepo.Setup(r => r.ObterPorIdAsync(10)).ReturnsAsync(new Produto { Id = 10 });
        
//         var novaRelacao = new FornecedorProduto { FornecedorId = 10, ProdutoId = 10 };

//         _mockFornProdRepo.Setup(r => r.AdicionarAsync(It.IsAny<FornecedorProduto>())).ReturnsAsync(novaRelacao);
        
//         // Act
//         var resultado = await _service.CriarFornecedorProdutoAsync(novaRelacao);

//         // Assert
//         Assert.True(resultado.Disponibilidade);
//         Assert.Equal(_hojeFixo, resultado.DataRegistro);
        
//         _mockFornProdRepo.Verify(r => r.AdicionarAsync(novaRelacao), Times.Once);
//         _mockUow.Verify(uow => uow.CommitAsync(), Times.Once);
//     }

//     [Fact(DisplayName = "Criar deve lançar exceção se Fornecedor não existir")]
//     public async Task CriarFornecedorProdutoAsync_FornecedorInexistente_DeveFalhar()
//     {
//         var novaRelacao = new FornecedorProduto { FornecedorId = 99, ProdutoId = 1 }; // 99 não existe
//         await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CriarFornecedorProdutoAsync(novaRelacao));
//     }

//     [Fact(DisplayName = "Criar deve lançar exceção se Relação já existir")]
//     public async Task CriarFornecedorProdutoAsync_RelacaoExistente_DeveFalhar()
//     {
//         // Arrange: Tenta criar 1-1 que já está na lista fake
//         var novaRelacao = new FornecedorProduto { FornecedorId = 1, ProdutoId = 1 }; 
        
//         // Act & Assert
//         await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarFornecedorProdutoAsync(novaRelacao));
//         _mockUow.Verify(uow => uow.RollbackAsync(), Times.Once); // Falha antes da transação
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Criação em Lote (Vários)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "CriarVarios deve salvar em lote se válido")]
//     public async Task CriarVariosFornecedorProdutosAsync_Sucesso_DeveSalvarBatch()
//     {
//         // Arrange
//         _mockFornecedorRepo.Setup(r => r.ObterPorIdAsync(10)).ReturnsAsync(new Fornecedor { Id = 10 });
//         _mockProdutoRepo.Setup(r => r.ObterPorIdAsync(10)).ReturnsAsync(new Produto { Id = 10 });
//         _mockProdutoRepo.Setup(r => r.ObterPorIdAsync(11)).ReturnsAsync(new Produto { Id = 11 });

//         var lista = new List<FornecedorProduto>
//         {
//             new FornecedorProduto { FornecedorId = 10, ProdutoId = 10 },
//             new FornecedorProduto { FornecedorId = 10, ProdutoId = 11 }
//         };

//         // Act
//         await _service.CriarVariosFornecedorProdutosAsync(lista);

//         // Assert
//         _mockFornProdRepo.Verify(r => r.AdicionarVariosAsync(lista), Times.Once);
//         _mockUow.Verify(uow => uow.CommitAsync(), Times.Once);
//     }

//     [Fact(DisplayName = "CriarVarios deve falhar se lista tiver duplicatas")]
//     public async Task CriarVariosFornecedorProdutosAsync_Duplicados_DeveFalhar()
//     {
//         var lista = new List<FornecedorProduto>
//         {
//             new FornecedorProduto { FornecedorId = 10, ProdutoId = 10 },
//             new FornecedorProduto { FornecedorId = 10, ProdutoId = 10 } // Duplicado
//         };

//         await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarVariosFornecedorProdutosAsync(lista));
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Atualização e Deleção
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "Atualizar deve salvar alterações")]
//     public async Task AtualizarFornecedorProdutoAsync_Sucesso_DeveSalvar()
//     {
//         var relacao = new FornecedorProduto { FornecedorId = 1, ProdutoId = 1, Disponibilidade = false };
        
//         await _service.AtualizarFornecedorProdutoAsync(1, 1, relacao);
        
//         _mockFornProdRepo.Verify(r => r.AtualizarAsync(relacao), Times.Once);
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
//     }

//     [Fact(DisplayName = "Atualizar deve lançar exceção se IDs divergirem")]
//     public async Task AtualizarFornecedorProdutoAsync_IdsDiferentes_DeveFalhar()
//     {
//         var relacao = new FornecedorProduto { FornecedorId = 1, ProdutoId = 1 };
//         await Assert.ThrowsAsync<ArgumentException>(() => _service.AtualizarFornecedorProdutoAsync(2, 1, relacao));
//     }

//     [Fact(DisplayName = "Deletar deve chamar repositório e salvar")]
//     public async Task DeletarFornecedorProdutoAsync_Sucesso_DeveDeletar()
//     {
//         await _service.DeletarFornecedorProdutoAsync(1, 1);
//         _mockFornProdRepo.Verify(r => r.DeletarAsync(1, 1), Times.Once);
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
//     }
// }