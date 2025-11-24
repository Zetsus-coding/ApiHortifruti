using System.Net;
using System.Net.Http.Json;
using ApiHortifruti.Domain;
using ApiHortifruti.DTO.ItemEntrada; // Necessário para ItemEntradaDTO
using ApiHortifruti.IntegrationTests.Integration.config;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiHortifruti.IntegrationTests.Integration;

public class EntradaIntegrationTests : BaseIntegrationTest
{
    public EntradaIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Fluxo Completo: Preparar Dados, Criar Entrada (POST) e Consultar (GET)")]
    public async Task FluxoCompleto_Entrada_DeveCriarEValidar()
    {
        // ====================================================================
        // 1. ARRANGE (PREPARAÇÃO DO CENÁRIO - SEED NO BANCO)
        // ====================================================================

        // Variáveis para armazenar os IDs gerados
        int fornecedorId, motivoId, produtoId;
        string numeroNota = "NF-" + Guid.NewGuid().ToString().Substring(0, 8); // Garante nota única
        string codigoFornecedorProduto = "COD-REF-01";

        // Usamos um escopo manual para inserir os dados iniciais
        // (Isso simula dados que já existiriam no sistema antes da entrada)
        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // 1.1 Criar Dependências do Produto (Categoria e Unidade)
            var categoria = new Categoria { Nome = "Legumes Teste" };
            var unidade = new UnidadeMedida { Nome = "Quilo", Abreviacao = "kg" };

            db.Categoria.Add(categoria);
            db.UnidadeMedida.Add(unidade);
            await db.SaveChangesAsync();

            // 1.2 Criar o Produto (Estoque inicial 0)
            var produto = new Produto
            {
                Nome = "Batata Inglesa",
                Codigo = "BAT-001",
                Preco = 4.50m,
                QuantidadeAtual = 0,
                QuantidadeMinima = 10,
                CategoriaId = categoria.Id,
                UnidadeMedidaId = unidade.Id,
                Ativo = true,
                Descricao = "Batata lavada"
            };
            db.Produto.Add(produto);
            await db.SaveChangesAsync();
            produtoId = produto.Id;

            // 1.3 Criar Fornecedor
            var fornecedor = new Fornecedor
            {
                NomeFantasia = "Hortifruti Fornecedor Ltda",
                CadastroPessoa = "12345678000199",
                Email = "contato@fornecedor.com",
                Telefone = "1199999999",
                Ativo = true,
                DataRegistro = DateOnly.FromDateTime(DateTime.Now)
            };
            db.Fornecedor.Add(fornecedor);

            // 1.4 Criar Motivo de Movimentação
            var motivo = new MotivoMovimentacao { Motivo = "Compra Estoque", Ativo = true };
            db.MotivoMovimentacao.Add(motivo);

            await db.SaveChangesAsync();

            fornecedorId = fornecedor.Id;
            motivoId = motivo.Id;
        }

        // ====================================================================
        // 2. ACT (AÇÃO - POST NA API)
        // ====================================================================

        // Montando o DTO exatamente como sua API espera (PostEntradaDTO)
        var postDto = new PostEntradaDTO
        {
            FornecedorId = fornecedorId,
            MotivoMovimentacaoId = motivoId,
            NumeroNota = numeroNota,
            DataCompra = DateOnly.FromDateTime(DateTime.Now),
            PrecoTotal = 50.00m, // (10 * 5.00)

            // Lista de Itens (ItemEntradaDTO)
            ItemEntrada = new List<ItemEntradaDTO>
            {
                new ItemEntradaDTO
                {
                    ProdutoId = produtoId,
                    Quantidade = 10.0m,      // Comprando 10kg
                    PrecoUnitario = 5.00m,   // a R$ 5,00 cada
                    CodigoFornecedor = codigoFornecedorProduto,
                    Lote = "LOTE-2025",
                    Validade = null // Opcional no DTO
                }
            }
        };

        var responsePost = await HttpClient.PostAsJsonAsync("/api/Entrada", postDto);

        // ====================================================================
        // 3. ASSERT (VALIDAÇÃO DO POST)
        // ====================================================================

        // 3.1 Valida Status HTTP 201 Created
        // Debug de Erro 400
        if (responsePost.StatusCode != HttpStatusCode.Created)
        {
            var erroMsg = await responsePost.Content.ReadAsStringAsync();
            // Isso vai forçar o teste a falhar e mostrar o JSON de erro da API na janela de output
            Assert.Fail($"A API retornou 400. Detalhes: {erroMsg}");
        }

        // 3.2 Lê o retorno (Sua controller retorna a Entidade 'Entrada' no POST)
        var entradaCriada = await responsePost.Content.ReadFromJsonAsync<Entrada>();
        entradaCriada.Should().NotBeNull();
        entradaCriada!.Id.Should().BeGreaterThan(0);
        entradaCriada.NumeroNota.Should().Be(numeroNota);

        // ====================================================================
        // 4. VERIFICAÇÃO NO BANCO DE DADOS (SIDE EFFECTS)
        // ====================================================================

        // Limpa o cache do EF para garantir leitura fresca do banco
        DbContext.ChangeTracker.Clear();

        // 4.1 Verifica se a Entrada e os Itens foram gravados
        var entradaNoBanco = await DbContext.Entrada
            .Include(e => e.ItemEntrada)
            .FirstOrDefaultAsync(e => e.Id == entradaCriada.Id);

        entradaNoBanco.Should().NotBeNull();
        entradaNoBanco!.PrecoTotal.Should().Be(50.00m);
        entradaNoBanco.ItemEntrada.Should().HaveCount(1);
        entradaNoBanco.ItemEntrada.First().ProdutoId.Should().Be(produtoId);
        entradaNoBanco.ItemEntrada.First().Quantidade.Should().Be(10.0m);

        // 4.2 Verifica se o Vínculo FornecedorProduto foi criado (Regra de Negócio do Service)
        var vinculo = await DbContext.FornecedorProduto
            .FirstOrDefaultAsync(fp => fp.FornecedorId == fornecedorId && fp.ProdutoId == produtoId);

        vinculo.Should().NotBeNull("o sistema deveria criar automaticamente o vínculo FornecedorProduto");
        vinculo!.CodigoFornecedor.Should().Be(codigoFornecedorProduto);

        // ====================================================================
        // 5. VALIDAÇÃO DO GET (CONSULTA)
        // ====================================================================

        // Vamos testar se o DTO de Leitura (GetEntradaSimplesDTO) está funcionando
        var responseGet = await HttpClient.GetAsync($"/api/Entrada/{entradaCriada.Id}");

        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);

        var entradaSimples = await responseGet.Content.ReadFromJsonAsync<GetEntradaSimplesDTO>();

        entradaSimples.Should().NotBeNull();
        entradaSimples!.Id.Should().Be(entradaCriada.Id);
        entradaSimples.PrecoTotal.Should().Be(50.00m);
        entradaSimples.NomeFantasiaFornecedor.Should().Be("Hortifruti Fornecedor Ltda"); // Veio do Include?
        entradaSimples.Motivo.Should().Be("Compra Estoque");
    }
}