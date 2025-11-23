using System.Net;
using System.Net.Http.Json;
using ApiHortifruti.Domain;
using ApiHortifruti.IntegrationTests.Integration.config; // Importa a config
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ApiHortifruti.IntegrationTests.Integration;

public class CategoriaIntegrationTests : BaseIntegrationTest
{
    public CategoriaIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Fluxo Completo: Criar, Ler, Atualizar e Deletar Categoria")]
    public async Task FluxoCompleto_Categoria_DeveFuncionarCorretamente()
    {
        // ====================================================================
        // 1. CREATE (POST)
        // ====================================================================
        
        // Arrange
        var novaCategoria = new { Nome = "Tubérculos Teste" }; // DTO anônimo

        // Act
        var responsePost = await HttpClient.PostAsJsonAsync("/api/Categoria", novaCategoria);

        // Assert
        responsePost.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var categoriaCriada = await responsePost.Content.ReadFromJsonAsync<Categoria>();
        categoriaCriada.Should().NotBeNull();
        categoriaCriada!.Id.Should().BeGreaterThan(0);
        categoriaCriada.Nome.Should().Be("Tubérculos Teste");

        // Validação Extra: Confere se realmente salvou no banco Docker
        var categoriaNoBanco = await DbContext.Categoria.FindAsync(categoriaCriada.Id);
        categoriaNoBanco.Should().NotBeNull();

        // ====================================================================
        // 2. GET (Consultar por ID)
        // ====================================================================
        
        // Act
        var responseGet = await HttpClient.GetAsync($"/api/Categoria/{categoriaCriada.Id}");

        // Assert
        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
        var categoriaGet = await responseGet.Content.ReadFromJsonAsync<Categoria>();
        categoriaGet!.Id.Should().Be(categoriaCriada.Id);

        // ====================================================================
        // 3. UPDATE (PUT)
        // ====================================================================

        // Arrange
        var categoriaAtualizada = new { Id = categoriaCriada.Id, Nome = "Tubérculos Frescos" };

        // Act
        var responsePut = await HttpClient.PutAsJsonAsync($"/api/Categoria/{categoriaCriada.Id}", categoriaAtualizada);

        // Assert
        responsePut.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verifica no banco se o nome mudou
        DbContext.ChangeTracker.Clear(); // Limpa cache do EF para forçar busca no banco
        var categoriaPosUpdate = await DbContext.Categoria.FindAsync(categoriaCriada.Id);
        categoriaPosUpdate!.Nome.Should().Be("Tubérculos Frescos");

        // ====================================================================
        // 4. DELETE
        // ====================================================================

        // Act
        var responseDelete = await HttpClient.DeleteAsync($"/api/Categoria/{categoriaCriada.Id}");

        // Assert
        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verifica no banco se sumiu
        DbContext.ChangeTracker.Clear();
        var categoriaDeletada = await DbContext.Categoria.FindAsync(categoriaCriada.Id);
        categoriaDeletada.Should().BeNull();
    }
}