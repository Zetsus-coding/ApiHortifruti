using System.Net;
using System.Net.Http.Json;
using ApiHortifruti.Domain;
using ApiHortifruti.IntegrationTests.Integration.config;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiHortifruti.IntegrationTests.Integration;

public class FornecedorIntegrationTests : BaseIntegrationTest
{
    public FornecedorIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Fluxo CRUD Fornecedor: Criar, Atualizar e Deletar")]
    public async Task FluxoCompleto_Fornecedor_DeveFuncionarCorretamente()
    {
        // ====================================================================
        // 1. CREATE (POST)
        // ====================================================================
        
        // Arrange
        var novoFornecedor = new PostFornecedorDTO
        {
            NomeFantasia = "Distribuidora de Teste Ltda",
            CadastroPessoa = "12.345.678/0001-99",
            Email = "contato@distribuidora.com",
            Telefone = "(11) 98765-4321", // Formato validado pelo Regex do DTO
            TelefoneExtra = null
        };

        // Act
        var responsePost = await HttpClient.PostAsJsonAsync("/api/Fornecedor", novoFornecedor);

        // Assert
        responsePost.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var fornecedorCriado = await responsePost.Content.ReadFromJsonAsync<Fornecedor>();
        fornecedorCriado.Should().NotBeNull();
        fornecedorCriado!.Id.Should().BeGreaterThan(0);
        fornecedorCriado.NomeFantasia.Should().Be(novoFornecedor.NomeFantasia);

        // Validação no Banco
        DbContext.ChangeTracker.Clear();
        var fornecedorNoBanco = await DbContext.Fornecedor.FindAsync(fornecedorCriado.Id);
        fornecedorNoBanco.Should().NotBeNull();
        fornecedorNoBanco!.Email.Should().Be("contato@distribuidora.com");

        // ====================================================================
        // 2. UPDATE (PUT)
        // ====================================================================

        // Arrange
        // Nota: O PUT geralmente espera a entidade ou um DTO. 
        // Baseado no seu Controller, ele espera a entidade 'Fornecedor'.
        // Vamos ajustar os dados do objeto criado para enviar no PUT.
        fornecedorCriado.NomeFantasia = "Distribuidora Atualizada SA";
        fornecedorCriado.Telefone = "(21) 99999-8888";

        // Act
        var responsePut = await HttpClient.PutAsJsonAsync($"/api/Fornecedor/{fornecedorCriado.Id}", fornecedorCriado);

        // Assert
        responsePut.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verifica atualização no Banco
        DbContext.ChangeTracker.Clear();
        var fornecedorAtualizado = await DbContext.Fornecedor.FindAsync(fornecedorCriado.Id);
        fornecedorAtualizado!.NomeFantasia.Should().Be("Distribuidora Atualizada SA");
        fornecedorAtualizado.Telefone.Should().Be("(21) 99999-8888");

        // ====================================================================
        // 3. DELETE
        // ====================================================================

        // Act
        var responseDelete = await HttpClient.DeleteAsync($"/api/Fornecedor/{fornecedorCriado.Id}");

        // Assert
        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verifica se sumiu do banco
        DbContext.ChangeTracker.Clear();
        var fornecedorDeletado = await DbContext.Fornecedor.FindAsync(fornecedorCriado.Id);
        fornecedorDeletado.Should().BeNull();
    }

    [Fact(DisplayName = "Consultar Fornecedor com Produtos: Deve retornar lista vinculada")]
    public async Task ObterProdutosPorFornecedorId_DeveRetornarDTOComLista()
    {
        // ====================================================================
        // 1. ARRANGE (SEED DE DADOS RELACIONADOS)
        // ====================================================================
        // Precisamos criar: Categoria -> Unidade -> Produto -> Fornecedor -> FornecedorProduto
        
        int fornecedorId;
        string nomeProduto = "Tomate Italiano";

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Criar Produto e suas dependências
            var categoria = new Categoria { Nome = "Frutos" };
            var unidade = new UnidadeMedida { Nome = "Quilo", Abreviacao = "kg" };
            db.Categoria.Add(categoria);
            db.UnidadeMedida.Add(unidade);
            await db.SaveChangesAsync();

            var produto = new Produto 
            { 
                Nome = nomeProduto, 
                Codigo = "TOM-001", 
                CategoriaId = categoria.Id, 
                UnidadeMedidaId = unidade.Id,
                QuantidadeMinima = 10,
                Preco = 8.50m,
                Ativo = true
            };
            db.Produto.Add(produto);

            // Criar Fornecedor
            var fornecedor = new Fornecedor
            {
                NomeFantasia = "Horta do Zé",
                CadastroPessoa = "99.999.999/0001-99",
                Email = "ze@horta.com",
                Telefone = "(11) 90000-0000",
                Ativo = true,
                DataRegistro = DateOnly.FromDateTime(DateTime.Now)
            };
            db.Fornecedor.Add(fornecedor);
            await db.SaveChangesAsync();

            // VINCULAR Fornecedor ao Produto (Tabela FornecedorProduto)
            var vinculo = new FornecedorProduto
            {
                FornecedorId = fornecedor.Id,
                ProdutoId = produto.Id,
                CodigoFornecedor = "COD-ZE-01",
                Disponibilidade = true,
                DataRegistro = DateOnly.FromDateTime(DateTime.Now)
            };
            db.FornecedorProduto.Add(vinculo);
            await db.SaveChangesAsync();

            fornecedorId = fornecedor.Id;
        }

        // ====================================================================
        // 2. ACT
        // ====================================================================
        var response = await HttpClient.GetAsync($"/api/Fornecedor/{fornecedorId}/produtos");

        // ====================================================================
        // 3. ASSERT
        // ====================================================================
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dtoRetorno = await response.Content.ReadFromJsonAsync<FornecedorComListaProdutosDTO>();
        
        dtoRetorno.Should().NotBeNull();
        dtoRetorno!.Id.Should().Be(fornecedorId);
        dtoRetorno.NomeFantasia.Should().Be("Horta do Zé");
        
        // Valida se a lista de produtos veio preenchida
        dtoRetorno.Produtos.Should().HaveCountGreaterThanOrEqualTo(1);
        
        // Verifica se o produto criado está na lista (assumindo que o DTO de produto tenha 'Nome')
        var produtoDaLista = dtoRetorno.Produtos.First();
        // Nota: Como não tenho o GetProdutoComDetalhesFornecimentoDTO, 
        // vou assumir que ele tem propriedades básicas ou checar apenas que existe.
        produtoDaLista.Should().NotBeNull();
    }
}