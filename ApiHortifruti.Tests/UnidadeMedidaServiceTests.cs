// using ApiHortifruti.Data.Repository.Interfaces;
// using ApiHortifruti.Domain;
// using ApiHortifruti.Service;
// using Moq;
// using Xunit;
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using System.Linq;

// namespace ApiHortifruti.Tests;

// public class UnidadeMedidaServiceTests
// {
//     private readonly Mock<IUnityOfWork> _mockUow;
//     private readonly Mock<IUnidadeMedidaRepository> _mockUnidadeRepo;
//     private readonly UnidadeMedidaService _service;

//     // Dados Fakes
//     private readonly List<UnidadeMedida> _unidadesFake = new List<UnidadeMedida>
//     {
//         new UnidadeMedida { Id = 1, Nome = "Quilograma", Abreviacao = "kg" },
//         new UnidadeMedida { Id = 2, Nome = "Unidade", Abreviacao = "un" },
//         new UnidadeMedida { Id = 3, Nome = "Litro", Abreviacao = "l" }
//     };

//     public UnidadeMedidaServiceTests()
//     {
//         _mockUow = new Mock<IUnityOfWork>();
//         _mockUnidadeRepo = new Mock<IUnidadeMedidaRepository>();

//         // Configurar UoW para retornar o repositório mockado
//         _mockUow.Setup(uow => uow.UnidadeMedida).Returns(_mockUnidadeRepo.Object);
        
//         // Configurar SaveChanges do UoW
//         _mockUow.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

//         // Configurar comportamento padrão de Consulta
//         _mockUnidadeRepo.Setup(r => r.ObterTodosAsync())
//                         .ReturnsAsync(_unidadesFake);

//         _mockUnidadeRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
//                         .ReturnsAsync((int id) => _unidadesFake.FirstOrDefault(u => u.Id == id));

//         // Instanciar o serviço
//         _service = new UnidadeMedidaService(_mockUow.Object);
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Consulta (GET)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "ObterTodas deve retornar a lista completa")]
//     public async Task ObterTodasAsUnidadesMedidaAsync_DeveRetornarTodas()
//     {
//         // Act
//         var resultado = await _service.ObterTodasAsUnidadesMedidaAsync();

//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal(_unidadesFake.Count, resultado.Count());
        
//         _mockUnidadeRepo.Verify(r => r.ObterTodosAsync(), Times.Once);
//     }

//     [Fact(DisplayName = "ObterPorId deve retornar unidade existente")]
//     public async Task ObterUnidadeMedidaPorIdAsync_DeveRetornarUnidade()
//     {
//         // Arrange
//         int idExistente = 1;

//         // Act
//         var resultado = await _service.ObterUnidadeMedidaPorIdAsync(idExistente);

//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal("Quilograma", resultado.Nome);
        
//         _mockUnidadeRepo.Verify(r => r.ObterPorIdAsync(idExistente), Times.Once);
//     }

//     [Fact(DisplayName = "ObterPorId deve retornar nulo se não existir")]
//     public async Task ObterUnidadeMedidaPorIdAsync_Inexistente_DeveRetornarNulo()
//     {
//         // Arrange
//         int idInexistente = 99;

//         // Act
//         var resultado = await _service.ObterUnidadeMedidaPorIdAsync(idInexistente);

//         // Assert
//         Assert.Null(resultado);
//         _mockUnidadeRepo.Verify(r => r.ObterPorIdAsync(idInexistente), Times.Once);
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Criação (POST)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "CriarUnidadeMedida deve chamar Adicionar e SaveChanges")]
//     public async Task CriarUnidadeMedidaAsync_DeveSalvar()
//     {
//         // Arrange
//         var novaUnidade = new UnidadeMedida { Id = 0, Nome = "Metro", Abreviacao = "m" };
        
//         // Act
//         var resultado = await _service.CriarUnidadeMedidaAsync(novaUnidade);

//         // Assert
//         Assert.Equal(novaUnidade, resultado);
        
//         // Verifica se o método de adicionar foi chamado no repositório
//         _mockUnidadeRepo.Verify(r => r.AdicionarAsync(novaUnidade), Times.Once);
        
//         // Verifica se o SaveChanges foi chamado no UoW (CRUCIAL para persistência)
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Atualização (PUT)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "AtualizarUnidadeMedida deve chamar Atualizar e SaveChanges")]
//     public async Task AtualizarUnidadeMedidaAsync_DeveAtualizar()
//     {
//         // Arrange
//         int idAtualizar = 2;
//         var unidadeAtualizada = new UnidadeMedida { Id = idAtualizar, Nome = "Caixa", Abreviacao = "cx" };

//         // Act
//         await _service.AtualizarUnidadeMedidaAsync(idAtualizar, unidadeAtualizada);

//         // Assert
//         _mockUnidadeRepo.Verify(r => r.AtualizarAsync(unidadeAtualizada), Times.Once);
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
//     }

//     [Fact(DisplayName = "AtualizarUnidadeMedida com IDs divergentes deve lançar ArgumentException")]
//     public async Task AtualizarUnidadeMedidaAsync_IdDiferente_DeveLancarExcecao()
//     {
//         // Arrange
//         int idUrl = 10;
//         var unidadeBody = new UnidadeMedida { Id = 20, Nome = "Teste", Abreviacao = "t" };

//         // Act & Assert
//         var exception = await Assert.ThrowsAsync<ArgumentException>(
//             () => _service.AtualizarUnidadeMedidaAsync(idUrl, unidadeBody));

//         Assert.Contains("O ID da unidade de medida na URL não corresponde", exception.Message);

//         // Verifica que NENHUM método de persistência foi chamado
//         _mockUnidadeRepo.Verify(r => r.AtualizarAsync(It.IsAny<UnidadeMedida>()), Times.Never);
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never);
//     }

//     // ---------------------------------------------------------------------
//     // Testes de Exclusão (DELETE)
//     // ---------------------------------------------------------------------

//     [Fact(DisplayName = "DeletarUnidadeMedida com ID existente deve chamar Deletar e SaveChanges")]
//     public async Task DeletarUnidadeMedidaAsync_ComIdExistente_DeveChamarDeletarESaveChanges()
//     {
//         // Arrange
//         int idDeletar = 1; // ID existente na lista _unidadesFake ("Quilograma")
        
//         // Recuperamos o objeto exato da lista fake para garantir que o Mock valide a instância correta
//         var unidadeEsperada = _unidadesFake.First(u => u.Id == idDeletar);

//         // Act
//         await _service.DeletarUnidadeMedidaAsync(idDeletar);

//         // Assert
//         //Verifica se o serviço buscou a unidade pelo ID antes de tentar deletar
//         _mockUnidadeRepo.Verify(r => r.ObterPorIdAsync(idDeletar), Times.Once);

//         //Verifica se o método DeletarAsync foi chamado passando o objeto correto
//         _mockUnidadeRepo.Verify(r => r.DeletarAsync(unidadeEsperada), Times.Once);

//         //Verifica se as alterações foram persistidas no banco
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
//     }

//     [Fact(DisplayName = "DeletarUnidadeMedida com ID inexistente deve lançar NotFoundException")]
//     public async Task DeletarUnidadeMedidaAsync_ComIdInexistente_DeveLancarNotFoundException()
//     {
//         // Arrange
//         int idInexistente = 99; // ID que não está na lista _unidadesFake

//         // Act & Assert
//         // Verifica se lança a exceção personalizada NotFoundException
//         await Assert.ThrowsAsync<ApiHortifruti.Exceptions.NotFoundException>(
//             () => _service.DeletarUnidadeMedidaAsync(idInexistente));

//         // Assert Side Effects (Garantir que nada foi alterado)
//         _mockUnidadeRepo.Verify(r => r.ObterPorIdAsync(idInexistente), Times.Once); // Tentou buscar
//         _mockUnidadeRepo.Verify(r => r.DeletarAsync(It.IsAny<UnidadeMedida>()), Times.Never); // NÃO tentou deletar
//         _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never); // NÃO tentou salvar
//     }
// }