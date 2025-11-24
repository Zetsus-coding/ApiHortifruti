using System.Net;
using System.Net.Http.Json;
using ApiHortifruti.Domain;
using ApiHortifruti.IntegrationTests.Integration.config;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiHortifruti.IntegrationTests.Integration;

public class HistoricoProdutoIntegrationTests : BaseIntegrationTest
{
    public HistoricoProdutoIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Fluxo Histórico: Deve consultar histórico gerado automaticamente")]
    public async Task ConsultarHistorico_DeveRetornarRegistrosDoProduto()
    {
        // ====================================================================
        // 1. ARRANGE (PREPARAR DADOS)
        // ====================================================================
        
        int produtoIdAlvo;
        decimal precoInicial = 10.00m;
        decimal precoAtualizado = 15.00m;

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // 1.1 Criar dependências (Categoria e Unidade)
            var categoria = new Categoria { Nome = "Frutas Histórico" };
            var unidade = new UnidadeMedida { Nome = "Unidade", Abreviacao = "un" };
            db.Categoria.Add(categoria);
            db.UnidadeMedida.Add(unidade);
            await db.SaveChangesAsync();

            // 1.2 Criar o Produto ALVO (O que vamos consultar)
            var produtoAlvo = new Produto
            {
                Nome = "Maçã Histórica",
                Codigo = "HIST-001",
                Preco = precoAtualizado,
                CategoriaId = categoria.Id,
                UnidadeMedidaId = unidade.Id,
                QuantidadeMinima = 10,
                Ativo = true
            };
            db.Produto.Add(produtoAlvo);

            // 1.3 Criar um Produto "RUÍDO" (Para garantir que o filtro funciona e não traz dados dele)
            // CORREÇÃO: Precisamos criar esse produto de verdade para não dar erro de Foreign Key
            var produtoRuido = new Produto
            {
                Nome = "Laranja (Ruído)",
                Codigo = "RUIDO-001",
                Preco = 50.00m,
                CategoriaId = categoria.Id,
                UnidadeMedidaId = unidade.Id,
                QuantidadeMinima = 10,
                Ativo = true
            };
            db.Produto.Add(produtoRuido);

            await db.SaveChangesAsync(); // Salva para gerar os IDs
            produtoIdAlvo = produtoAlvo.Id;

            // 1.4 Criar Histórico Manualmente
            
            // Registros do Produto ALVO (Devem retornar)
            db.HistoricoProduto.Add(new HistoricoProduto
            {
                ProdutoId = produtoIdAlvo,
                PrecoProduto = precoInicial,
                DataAlteracao = DateOnly.FromDateTime(DateTime.Now.AddDays(-10))
            });

            db.HistoricoProduto.Add(new HistoricoProduto
            {
                ProdutoId = produtoIdAlvo,
                PrecoProduto = precoAtualizado,
                DataAlteracao = DateOnly.FromDateTime(DateTime.Now)
            });

            // Registro do Produto RUÍDO (NÃO deve retornar)
            // CORREÇÃO: Agora usamos o ID real do produtoRuido em vez de 9999
            db.HistoricoProduto.Add(new HistoricoProduto
            {
                ProdutoId = produtoRuido.Id, 
                PrecoProduto = 50.00m,
                DataAlteracao = DateOnly.FromDateTime(DateTime.Now)
            });

            await db.SaveChangesAsync();
        }

        // ====================================================================
        // 2. ACT (CONSULTAR HISTÓRICO POR PRODUTO)
        // ====================================================================
        
        var response = await HttpClient.GetAsync($"/api/HistoricoProduto/produto/{produtoIdAlvo}");

        // ====================================================================
        // 3. ASSERT
        // ====================================================================

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var historicoLista = await response.Content.ReadFromJsonAsync<List<HistoricoProduto>>();
        
        historicoLista.Should().NotBeNull();
        
        // Deve ter apenas 2 registros (ignorando o registro do produtoRuido)
        historicoLista.Should().HaveCount(2); 

        // Garante que os registros pertencem apenas ao produto alvo
        historicoLista.Should().OnlyContain(h => h.ProdutoId == produtoIdAlvo);
        
        // Opcional: Validar valores
        historicoLista!.Should().Contain(h => h.PrecoProduto == precoInicial);
        historicoLista!.Should().Contain(h => h.PrecoProduto == precoAtualizado);
    }
}