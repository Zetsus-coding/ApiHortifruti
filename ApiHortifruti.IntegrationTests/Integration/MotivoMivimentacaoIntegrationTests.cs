using System.Net;
using System.Net.Http.Json;
using ApiHortifruti.Domain;
using ApiHortifruti.IntegrationTests.Integration.config;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ApiHortifruti.IntegrationTests.Integration;

public class MotivoMovimentacaoIntegrationTests : BaseIntegrationTest
{
    public MotivoMovimentacaoIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Fluxo Motivo: Criar, Ler, Atualizar e Deletar")]
    public async Task FluxoCompleto_Motivo_DeveFuncionar()
    {
        // ====================================================================
        // 1. CREATE (POST)
        // ====================================================================
        
        // Usando o PostMotivoMovimentacaoDTO real
        var novoMotivoDto = new PostMotivoMovimentacaoDTO 
        {
            Motivo = "Venda Balcão" // Max 20 chars
        };

        var responsePost = await HttpClient.PostAsJsonAsync("/api/MotivoMovimentacao", novoMotivoDto);

        // Assert POST
        if (responsePost.StatusCode != HttpStatusCode.Created)
        {
            var erro = await responsePost.Content.ReadAsStringAsync();
            Assert.Fail($"Erro no POST: {erro}");
        }

        var motivoCriado = await responsePost.Content.ReadFromJsonAsync<MotivoMovimentacao>();
        motivoCriado.Should().NotBeNull();
        motivoCriado!.Id.Should().BeGreaterThan(0);
        motivoCriado.TipoMovimentacao.Should().Be("Venda Balcão");

        // Verifica no banco se gravou
        DbContext.ChangeTracker.Clear();
        var motivoNoBanco = await DbContext.MotivoMovimentacao.FindAsync(motivoCriado.Id);
        motivoNoBanco.Should().NotBeNull();
        motivoNoBanco!.TipoMovimentacao.Should().Be("Venda Balcão");

        // ====================================================================
        // 2. GET (CONSULTAR)
        // ====================================================================

        var responseGet = await HttpClient.GetAsync($"/api/MotivoMovimentacao/{motivoCriado.Id}");
        
        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
        var motivoRetornado = await responseGet.Content.ReadFromJsonAsync<MotivoMovimentacao>();
        
        motivoRetornado.Should().NotBeNull();
        motivoRetornado!.TipoMovimentacao.Should().Be("Venda Balcão");

        // ====================================================================
        // 3. UPDATE (PUT)
        // ====================================================================

        // Usando o PutMotivoMovimentacaoDTO real
        var putDto = new PutMotivoMovimentacaoDTO
        {
            Motivo = "Venda Online", // Alterando nome (Max 20 chars)
            Ativo = false            // Alterando status (Inativando)
        };

        var responsePut = await HttpClient.PutAsJsonAsync($"/api/MotivoMovimentacao/{motivoCriado.Id}", putDto);

        // Assert PUT
        if (responsePut.StatusCode != HttpStatusCode.NoContent)
        {
            var erro = await responsePut.Content.ReadAsStringAsync();
            Assert.Fail($"Erro no PUT: {erro}");
        }

        // Verifica no banco se os dados mudaram
        DbContext.ChangeTracker.Clear();
        var motivoAtualizado = await DbContext.MotivoMovimentacao.FindAsync(motivoCriado.Id);
        
        motivoAtualizado!.TipoMovimentacao.Should().Be("Venda Online");
        motivoAtualizado.Ativo.Should().BeFalse(); // Confere se inativou

        // ====================================================================
        // 4. DELETE
        // ====================================================================

        var responseDelete = await HttpClient.DeleteAsync($"/api/MotivoMovimentacao/{motivoCriado.Id}");
        
        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verifica se sumiu do banco
        DbContext.ChangeTracker.Clear();
        var motivoDeletado = await DbContext.MotivoMovimentacao.FindAsync(motivoCriado.Id);
        motivoDeletado.Should().BeNull();
    }
}