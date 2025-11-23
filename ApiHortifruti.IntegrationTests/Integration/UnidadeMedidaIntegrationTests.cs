using System.Net;
using System.Net.Http.Json;
using ApiHortifruti.Domain;
using ApiHortifruti.IntegrationTests.Integration.config;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ApiHortifruti.IntegrationTests.Integration;

public class UnidadeMedidaIntegrationTests : BaseIntegrationTest
{
    public UnidadeMedidaIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Fluxo UnidadeMedida: Criar, Ler, Atualizar e Deletar")]
    public async Task FluxoCompleto_UnidadeMedida_DeveFuncionar()
    {
        // ====================================================================
        // 1. CREATE (POST)
        // ====================================================================
        
        var postDto = new PostUnidadeMedidaDTO
        {
            Nome = "Litro",
            Abreviacao = "l" // Max 10 chars
        };

        var responsePost = await HttpClient.PostAsJsonAsync("/api/UnidadeMedida", postDto);

        // Assert POST
        if (responsePost.StatusCode != HttpStatusCode.Created)
        {
            var erro = await responsePost.Content.ReadAsStringAsync();
            Assert.Fail($"Erro no POST: {erro}");
        }

        var unidadeCriada = await responsePost.Content.ReadFromJsonAsync<UnidadeMedida>();
        unidadeCriada.Should().NotBeNull();
        unidadeCriada!.Id.Should().BeGreaterThan(0);
        unidadeCriada.Nome.Should().Be("Litro");

        // Verifica no banco
        DbContext.ChangeTracker.Clear();
        var unidadeNoBanco = await DbContext.UnidadeMedida.FindAsync(unidadeCriada.Id);
        unidadeNoBanco.Should().NotBeNull();
        unidadeNoBanco!.Abreviacao.Should().Be("l");

        // ====================================================================
        // 2. GET (CONSULTAR)
        // ====================================================================

        var responseGet = await HttpClient.GetAsync($"/api/UnidadeMedida/{unidadeCriada.Id}");
        
        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
        var unidadeRetornada = await responseGet.Content.ReadFromJsonAsync<UnidadeMedida>();
        
        unidadeRetornada.Should().NotBeNull();
        unidadeRetornada!.Nome.Should().Be("Litro");

        // ====================================================================
        // 3. UPDATE (PUT)
        // ====================================================================

        var putDto = new PutUnidadeMedidaDTO
        {
            Id = unidadeCriada.Id, // O DTO exige o ID
            Nome = "Mililitro",    // Alterando nome
            Abreviacao = "ml"      // Alterando abreviação
        };

        var responsePut = await HttpClient.PutAsJsonAsync($"/api/UnidadeMedida/{unidadeCriada.Id}", putDto);

        // Assert PUT
        if (responsePut.StatusCode != HttpStatusCode.NoContent)
        {
            var erro = await responsePut.Content.ReadAsStringAsync();
            Assert.Fail($"Erro no PUT: {erro}");
        }

        DbContext.ChangeTracker.Clear();
        var unidadeAtualizada = await DbContext.UnidadeMedida.FindAsync(unidadeCriada.Id);
        unidadeAtualizada!.Nome.Should().Be("Mililitro");
        unidadeAtualizada.Abreviacao.Should().Be("ml");

        // ====================================================================
        // 4. DELETE
        // ====================================================================

        var responseDelete = await HttpClient.DeleteAsync($"/api/UnidadeMedida/{unidadeCriada.Id}");
        
        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verifica se sumiu do banco
        DbContext.ChangeTracker.Clear();
        var unidadeDeletada = await DbContext.UnidadeMedida.FindAsync(unidadeCriada.Id);
        unidadeDeletada.Should().BeNull();
    }
}