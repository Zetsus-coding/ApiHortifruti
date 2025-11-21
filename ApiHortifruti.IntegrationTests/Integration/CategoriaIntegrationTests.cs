// Arquivo: Integration/CategoriaIntegrationTests.cs
using System.Net;
using System.Net.Http.Json;
using ApiHortifruti.Domain; // Ajuste se necessário
using ApiHortifruti.IntegrationTests.Integration.Config;
using FluentAssertions;
using Xunit;

namespace ApiHortifruti.IntegrationTests.Integration;

public class CategoriaIntegrationTests : BaseIntegrationTest
{
    public CategoriaIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_DeveRetornarListaVazia_QuandoNaoHouverCategorias()
    {
        // Act
        var response = await Client.GetAsync("/api/Categoria"); // Verifique a rota exata do seu Controller

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var lista = await response.Content.ReadFromJsonAsync<List<Categoria>>(); // Ajuste o DTO/Model de retorno
        lista.Should().BeEmpty();
    }
}