// using ApiHortifruti.Data.Repository.Interfaces;
// using ApiHortifruti.Domain;
// using ApiHortifruti.Service;
// using Moq;
// using ApiHortifruti.Exceptions;

// namespace ApiHortifruti.Tests;

// public class MotivoMovimentacaoServiceTests
// {
//     private readonly Mock<IUnityOfWork> _mockUow;
//     private readonly Mock<IMotivoMovimentacaoRepository> _mockMotivoRepo;
//     private readonly MotivoMovimentacaoService _service;

//     // Dados Fakes
//     private readonly List<MotivoMovimentacao> _motivosFake = new List<MotivoMovimentacao>
//     {
//         new MotivoMovimentacao { Id = 1, Motivo = "Compra", Ativo = true },
//         new MotivoMovimentacao { Id = 2, Motivo = "Venda", Ativo = true },
//         new MotivoMovimentacao { Id = 3, Motivo = "Perda/Descarte", Ativo = true }
//     };

//     public MotivoMovimentacaoServiceTests()
//     {
//         _mockUow = new Mock<IUnityOfWork>();
//         _mockMotivoRepo = new Mock<IMotivoMovimentacaoRepository>();

//         // Configurar UoW
//         _mockUow.Setup(uow => uow.MotivoMovimentacao).Returns(_mockMotivoRepo.Object);
        
//         // Configurar SaveChanges (retorna 1 afetado)
//         _mockUow.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

//         // Configurar Consultas
//         _mockMotivoRepo.Setup(r => r.ObterTodosAsync()).ReturnsAsync(_motivosFake);
//         _mockMotivoRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
//                        .ReturnsAsync((int id) => _motivosFake.FirstOrDefault(m => m.Id == id));

//         // Instanciar serviço
//         _service = new MotivoMovimentacaoService(_mockUow.Object);
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Consulta (GET)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "ObterTodos deve retornar a lista completa")]
//     public async Task ObterTodosMotivosAsync_DeveRetornarTodos()
//     {
//         // Act
//         var resultado = await _service.ObterTodosOsMotivosMovimentacaoAsync();

//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal(_motivosFake.Count, resultado.Count());
//         _mockMotivoRepo.Verify(r => r.ObterTodosAsync(), Times.Once);
//     }

//     [Fact(DisplayName = "ObterPorId deve retornar motivo existente")]
//     public async Task ObterMotivoPorIdAsync_DeveRetornarMotivo()
//     {
//         // Arrange
//         int idExistente = 1;

//         // Act
//         var resultado = await _service.ObterMotivoMovimentacaoPorIdAsync(idExistente);

//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal("Compra", resultado.Motivo);
//         _mockMotivoRepo.Verify(r => r.ObterPorIdAsync(idExistente), Times.Once);
//     }

//     [Fact(DisplayName = "ObterPorId deve retornar nulo se não existir")]
//     public async Task ObterMotivoPorIdAsync_Inexistente_DeveRetornarNulo()
//     {
//         // Arrange
//         int idInexistente = 99;

//         // Act
//         var resultado = await _service.ObterMotivoMovimentacaoPorIdAsync(idInexistente);

//         // Assert
//         Assert.Null(resultado);
//         _mockMotivoRepo.Verify(r => r.ObterPorIdAsync(idInexistente), Times.Once);
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Escrita (POST/PUT/DELETE)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "CriarMotivo deve chamar Adicionar e SaveChanges")]
//     public async Task CriarMotivoAsync_DeveSalvar()
//     {
//         // Arrange
//         var novoMotivo = new MotivoMovimentacao { Id = 0, Motivo = "Doação", Ativo = true };
//         _mockMotivoRepo.Setup(r => r.AdicionarAsync(It.IsAny<MotivoMovimentacao>())).ReturnsAsync(novoMotivo);

//         // Act
//         var resultado = await _service.CriarMotivoMovimentacaoAsync(novoMotivo);

//         // Assert
//         Assert.Equal(novoMotivo, resultado);
//         _mockMotivoRepo.Verify(r => r.AdicionarAsync(novoMotivo), Times.Once);
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once); // Garante que o serviço está salvando
//     }

//     [Fact(DisplayName = "AtualizarMotivo deve chamar Atualizar e SaveChanges")]
//     public async Task AtualizarMotivoAsync_DeveAtualizar()
//     {
//         // Arrange
//         int idAtualizar = 3;
//         var motivoAtualizado = new MotivoMovimentacao { Id = idAtualizar, Motivo = "Perda", Ativo = false };

//         // Act
//         await _service.AtualizarMotivoMovimentacaoAsync(idAtualizar, motivoAtualizado);

//         // Assert
//         _mockMotivoRepo.Verify(r => r.AtualizarAsync(motivoAtualizado), Times.Once);
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
//     }

//     [Fact(DisplayName = "AtualizarMotivo com IDs divergentes deve lançar ArgumentException")]
//     public async Task AtualizarMotivoAsync_IdDiferente_DeveLancarExcecao()
//     {
//         // Arrange
//         int idUrl = 10;
//         var motivoBody = new MotivoMovimentacao { Id = 20, Motivo = "Teste" };

//         // Act & Assert
//         await Assert.ThrowsAsync<ArgumentException>(
//             () => _service.AtualizarMotivoMovimentacaoAsync(idUrl, motivoBody));

//         // Verifica que nada foi salvo
//         _mockMotivoRepo.Verify(r => r.AtualizarAsync(It.IsAny<MotivoMovimentacao>()), Times.Never);
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never);
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Exclusão (DELETE)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "DeletarMotivo com ID existente deve chamar Deletar e SaveChanges")]
//     public async Task DeletarMotivoAsync_ComIdExistente_DeveDeletar()
//     {
//         // Arrange
//         int idDeletar = 1; // ID existente na lista _motivosFake
        
//         // Recuperamos o objeto da lista fake para garantir que é exatamente a mesma instância
//         var motivoEsperado = _motivosFake.First(m => m.Id == idDeletar);

//         // Act
//         await _service.DeletarMotivoMovimentacaoAsync(idDeletar);

//         // Assert
//         //Verifica se buscou pelo ID
//         _mockMotivoRepo.Verify(r => r.ObterPorIdAsync(idDeletar), Times.Once);

//         //Verifica se chamou o DeletarAsync passando o OBJETO (não o ID)
//         _mockMotivoRepo.Verify(r => r.DeletarAsync(motivoEsperado), Times.Once);
        
//         //Verifica se salvou
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
//     }

//     [Fact(DisplayName = "DeletarMotivo com ID inexistente deve lançar NotFoundException")]
//     public async Task DeletarMotivoAsync_ComIdInexistente_DeveLancarExcecao()
//     {
//         // Arrange
//         int idInexistente = 99; // ID que não está na lista _motivosFake

//         // Act & Assert
//         await Assert.ThrowsAsync<NotFoundException>(
//             () => _service.DeletarMotivoMovimentacaoAsync(idInexistente));

//         // Assert
//         // Verifica que buscou, mas NÃO deletou e NÃO salvou
//         _mockMotivoRepo.Verify(r => r.ObterPorIdAsync(idInexistente), Times.Once);
//         _mockMotivoRepo.Verify(r => r.DeletarAsync(It.IsAny<MotivoMovimentacao>()), Times.Never);
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never);
//     }
// }