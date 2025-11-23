using System.Net;
using System.Net.Http.Json;
using ApiHortifruti.Domain;
using ApiHortifruti.IntegrationTests.Integration.config;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiHortifruti.IntegrationTests.Integration;

public class ProdutoIntegrationTests : BaseIntegrationTest
{
    public ProdutoIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Fluxo Produto: Criar, Ler, Atualizar Preço (Histórico) e Estoque Crítico")]
    public async Task FluxoCompleto_Produto_DeveFuncionarCorretamente()
    {
        // ====================================================================
        // 1. ARRANGE (DEPENDÊNCIAS)
        // ====================================================================
        int categoriaId, unidadeId;

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var cat = new Categoria { Nome = "Frutas Tropicais" };
            var und = new UnidadeMedida { Nome = "Caixa", Abreviacao = "cx" };
            db.Add(cat);
            db.Add(und);
            await db.SaveChangesAsync();
            categoriaId = cat.Id;
            unidadeId = und.Id;
        }

        // ====================================================================
        // 2. CREATE (POST)
        // ====================================================================
        var postDto = new PostProdutoDTO
        {
            Nome = "Manga Palmer",
            Codigo = "MNG-001",
            Preco = 8.00m,
            QuantidadeMinima = 20, // Minimo 20
            CategoriaId = categoriaId,
            UnidadeMedidaId = unidadeId,
            Descricao = "Manga doce"
        };

        var responsePost = await HttpClient.PostAsJsonAsync("/api/Produto", postDto);

        // Assert Post
        if (responsePost.StatusCode != HttpStatusCode.Created)
        {
            var erro = await responsePost.Content.ReadAsStringAsync();
            Assert.Fail($"Erro no POST: {erro}");
        }

        var produtoCriado = await responsePost.Content.ReadFromJsonAsync<Produto>();
        produtoCriado.Should().NotBeNull();
        
        // Verificações no Banco
        DbContext.ChangeTracker.Clear();
        var produtoNoBanco = await DbContext.Produto
            .Include(p => p.HistoricoProduto)
            .FirstOrDefaultAsync(p => p.Id == produtoCriado.Id);

        produtoNoBanco!.Nome.Should().Be("Manga Palmer");
        produtoNoBanco.QuantidadeAtual.Should().Be(0); // Nasce com 0
        
        // Verifica se gerou o primeiro histórico
        produtoNoBanco.HistoricoProduto.Should().HaveCount(1);
        produtoNoBanco.HistoricoProduto.First().PrecoProduto.Should().Be(8.00m);

        // ====================================================================
        // 3. GET (CONSULTA POR CÓDIGO)
        // ====================================================================
        var responseGetCodigo = await HttpClient.GetAsync($"/api/Produto/codigo/{postDto.Codigo}");
        responseGetCodigo.StatusCode.Should().Be(HttpStatusCode.OK);
        var produtoPorCodigo = await responseGetCodigo.Content.ReadFromJsonAsync<Produto>();
        produtoPorCodigo!.Id.Should().Be(produtoCriado.Id);

        // ====================================================================
        // 4. TESTE DE ESTOQUE CRÍTICO
        // ====================================================================
        // O produto tem QtdAtual = 0 e QtdMinima = 20. Ele DEVE aparecer no crítico.
        
        var responseCritico = await HttpClient.GetAsync("/api/Produto/estoque-critico");
        responseCritico.StatusCode.Should().Be(HttpStatusCode.OK);
        var listaCritica = await responseCritico.Content.ReadFromJsonAsync<List<GetProdutoEstoqueCriticoDTO>>();
        
        listaCritica.Should().Contain(p => p.Codigo == "MNG-001");

        // ====================================================================
        // 5. UPDATE (PUT) - Alterando Preço para gerar Histórico
        // ====================================================================
        var putDto = new PutProdutoDTO
        {
            IdProduto = produtoCriado.Id,
            Nome = "Manga Palmer Madura",
            Codigo = "MNG-001",
            Preco = 12.00m, // Aumentou o preço
            QuantidadeMinima = 20,
            CategoriaId = categoriaId,
            UnidadeMedidaId = unidadeId,
            Descricao = "Bem doce",
            Ativo = true
        };

        var responsePut = await HttpClient.PutAsJsonAsync($"/api/Produto/{produtoCriado.Id}", putDto);
        
        if (responsePut.StatusCode != HttpStatusCode.NoContent)
        {
            var erro = await responsePut.Content.ReadAsStringAsync();
            Assert.Fail($"Erro no PUT: {erro}");
        }

        DbContext.ChangeTracker.Clear();
        var produtoAtualizado = await DbContext.Produto
            .Include(p => p.HistoricoProduto)
            .FirstOrDefaultAsync(p => p.Id == produtoCriado.Id);

        produtoAtualizado!.Preco.Should().Be(12.00m);
        
        // Deve ter 2 históricos agora (8.00 e 12.00)
        produtoAtualizado.HistoricoProduto.Should().HaveCount(2);
        produtoAtualizado.HistoricoProduto.Last().PrecoProduto.Should().Be(12.00m);

        // ====================================================================
        // 6. DELETE
        // ====================================================================
        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // Removemos o histórico manualmente antes de tentar deletar o produto
            var historicos = db.HistoricoProduto.Where(h => h.ProdutoId == produtoCriado.Id);
            db.HistoricoProduto.RemoveRange(historicos);
            await db.SaveChangesAsync();
        }
        // ---------------------------------------------------------------

        var responseDelete = await HttpClient.DeleteAsync($"/api/Produto/{produtoCriado.Id}");
    }
}