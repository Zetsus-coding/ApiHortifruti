using System.Net;
using System.Net.Http.Json;
using ApiHortifruti.Domain;
using ApiHortifruti.IntegrationTests.Integration.config;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiHortifruti.IntegrationTests.Integration;

public class FornecedorProdutoIntegrationTests : BaseIntegrationTest
{
    public FornecedorProdutoIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Fluxo FornecedorProduto: Vincular, Consultar, Atualizar e Desvincular")]
    public async Task FluxoCompleto_FornecedorProduto_DeveFuncionar()
    {
        // ====================================================================
        // 1. ARRANGE (PREPARAR DEPENDÊNCIAS NO BANCO)
        // ====================================================================

        int produtoId;
        int fornecedorId;

        // Criamos os dados básicos necessários (Categoria -> Produto e Fornecedor)
        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // 1.1 Criar Categoria e Unidade
            var categoria = new Categoria { Nome = "Raízes" };
            var unidade = new UnidadeMedida { Nome = "Quilo", Abreviacao = "kg" };
            db.Categoria.Add(categoria);
            db.UnidadeMedida.Add(unidade);
            await db.SaveChangesAsync();

            // 1.2 Criar Produto
            var produto = new Produto
            {
                Nome = "Cenoura",
                Codigo = "CEN-001",
                Preco = 3.50m,
                CategoriaId = categoria.Id,
                UnidadeMedidaId = unidade.Id,
                QuantidadeMinima = 50,
                Ativo = true
            };
            db.Produto.Add(produto);

            // 1.3 Criar Fornecedor
            var fornecedor = new Fornecedor
            {
                NomeFantasia = "Fornecedor Cenoura Ltda",
                CadastroPessoa = "55.555.555/0001-55",
                Email = "cenoura@agro.com",
                Telefone = "11999999999",
                Ativo = true,
                DataRegistro = DateOnly.FromDateTime(DateTime.Now)
            };
            db.Fornecedor.Add(fornecedor);

            await db.SaveChangesAsync();
            produtoId = produto.Id;
            fornecedorId = fornecedor.Id;
        }

        // ====================================================================
        // 2. CREATE (POST) - VINCULAR FORNECEDOR AO PRODUTO
        // ====================================================================

        // Montando o objeto exatamente como o PostFornecedorProdutoDTO pede
        var novoVinculoDto = new PostFornecedorProdutoDTO
        {
            FornecedorId = fornecedorId,
            ProdutoId = produtoId,
            CodigoFornecedor = "COD-CEN-EXT"
        };

        var responsePost = await HttpClient.PostAsJsonAsync("/api/FornecedorProduto", novoVinculoDto);

        // Assert POST
        // Debug de Erro 400
        if (responsePost.StatusCode != HttpStatusCode.Created)
        {
            var erroMsg = await responsePost.Content.ReadAsStringAsync();
            // Isso vai forçar o teste a falhar e mostrar o JSON de erro da API na janela de output
            Assert.Fail($"A API retornou 400. Detalhes: {erroMsg}");
        }

        // Verifica no banco se gravou
        DbContext.ChangeTracker.Clear();
        // FindAsync com chave composta (FornecedorId, ProdutoId)
        var vinculoNoBanco = await DbContext.FornecedorProduto.FindAsync(fornecedorId, produtoId);

        vinculoNoBanco.Should().NotBeNull();
        vinculoNoBanco!.CodigoFornecedor.Should().Be("COD-CEN-EXT");

        // ====================================================================
        // 3. GET (CONSULTAR POR ID COMPOSTO)
        // ====================================================================

        var responseGet = await HttpClient.GetAsync($"/api/FornecedorProduto/{fornecedorId}/{produtoId}");

        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);

        // O GET retorna a Entidade FornecedorProduto completa
        var vinculoRetornado = await responseGet.Content.ReadFromJsonAsync<FornecedorProduto>();

        vinculoRetornado!.FornecedorId.Should().Be(fornecedorId);
        vinculoRetornado.ProdutoId.Should().Be(produtoId);

        // ====================================================================
        // 4. UPDATE (PUT)
        // ====================================================================

        // O seu Controller espera a entidade completa no PUT.
        // Vamos modificar o objeto que recebemos do GET e enviar de volta.
        vinculoRetornado.CodigoFornecedor = "COD-ALTERADO";
        vinculoRetornado.Disponibilidade = false; // Testando alteração de status

        var responsePut = await HttpClient.PutAsJsonAsync($"/api/FornecedorProduto/{fornecedorId}/{produtoId}", vinculoRetornado);

        // Assert PUT
        responsePut.StatusCode.Should().Be(HttpStatusCode.NoContent);

        DbContext.ChangeTracker.Clear();
        var vinculoAtualizado = await DbContext.FornecedorProduto.FindAsync(fornecedorId, produtoId);
        vinculoAtualizado!.CodigoFornecedor.Should().Be("COD-ALTERADO");
        vinculoAtualizado.Disponibilidade.Should().BeFalse();

        // ====================================================================
        // 5. DELETE (DESVINCULAR)
        // ====================================================================

        var responseDelete = await HttpClient.DeleteAsync($"/api/FornecedorProduto/{fornecedorId}/{produtoId}");

        // Assert DELETE
        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        DbContext.ChangeTracker.Clear();
        var vinculoDeletado = await DbContext.FornecedorProduto.FindAsync(fornecedorId, produtoId);
        vinculoDeletado.Should().BeNull();
    }
}