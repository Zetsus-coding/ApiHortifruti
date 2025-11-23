using System.Net;
using System.Net.Http.Json;
using ApiHortifruti.Domain;
using ApiHortifruti.DTO.ItemSaida;
using ApiHortifruti.IntegrationTests.Integration.config;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiHortifruti.IntegrationTests.Integration;

public class SaidaIntegrationTests : BaseIntegrationTest
{
    public SaidaIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Fluxo Saída: Deve criar venda e baixar estoque do produto")]
    public async Task FluxoCompleto_Saida_DeveCriarEBaixarEstoque()
    {
        // ====================================================================
        // 1. ARRANGE (PREPARAR CENÁRIO)
        // ====================================================================
        int produtoId;
        int motivoId;
        decimal precoVenda = 5.00m;
        int estoqueInicial = 100;
        int quantidadeVenda = 10;

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // 1.1 Criar Dependências
            var categoria = new Categoria { Nome = "Frutas Venda" };
            var unidade = new UnidadeMedida { Nome = "Unidade", Abreviacao = "un" };
            db.Add(categoria);
            db.Add(unidade);
            await db.SaveChangesAsync();

            // 1.2 Criar Produto com ESTOQUE INICIAL
            var produto = new Produto
            {
                Nome = "Banana Prata",
                Codigo = "BAN-001",
                Preco = 3.00m, // Preço de custo/base
                QuantidadeAtual = estoqueInicial, // IMPORTANTE: Começa com 100
                QuantidadeMinima = 10,
                CategoriaId = categoria.Id,
                UnidadeMedidaId = unidade.Id,
                Ativo = true
            };
            db.Produto.Add(produto);

            // 1.3 Criar Motivo
            var motivo = new MotivoMovimentacao { Motivo = "Venda ao Consumidor", Ativo = true };
            db.MotivoMovimentacao.Add(motivo);

            await db.SaveChangesAsync();
            produtoId = produto.Id;
            motivoId = motivo.Id;
        }

        // ====================================================================
        // 2. ACT (REALIZAR A VENDA/SAÍDA)
        // ====================================================================

        var postDto = new PostSaidaDTO
        {
            MotivoMovimentacaoId = motivoId,
            CadastroCliente = "123.456.789-00",
            ValorTotal = 50.00m, // 10 itens * 5.00
            Desconto = false,
            ValorDesconto = null,
            
            ItemSaida = new List<ItemSaidaDTO>
            {
                new ItemSaidaDTO
                {
                    ProdutoId = produtoId,
                    Quantidade = quantidadeVenda // Vendendo 10
                    // Valor? Se tiver no DTO, adicione aqui: Valor = precoVenda
                }
            }
        };

        var responsePost = await HttpClient.PostAsJsonAsync("/api/Saida", postDto);

        // Debug caso falhe
        if (responsePost.StatusCode != HttpStatusCode.Created)
        {
            var erro = await responsePost.Content.ReadAsStringAsync();
            Assert.Fail($"Erro ao criar Saída: {erro}");
        }

        // ====================================================================
        // 3. ASSERT (VALIDAÇÃO)
        // ====================================================================

        var saidaCriada = await responsePost.Content.ReadFromJsonAsync<Saida>();
        saidaCriada.Should().NotBeNull();
        saidaCriada!.Id.Should().BeGreaterThan(0);

        // 3.1 Verificar se a Saída foi gravada no banco
        DbContext.ChangeTracker.Clear();
        var saidaNoBanco = await DbContext.Saida
            .Include(s => s.ItemSaida)
            .FirstOrDefaultAsync(s => s.Id == saidaCriada.Id);

        saidaNoBanco.Should().NotBeNull();
        saidaNoBanco!.ValorTotal.Should().Be(50.00m);
        saidaNoBanco.ItemSaida.Should().HaveCount(1);
        saidaNoBanco.ItemSaida.First().ProdutoId.Should().Be(produtoId);

        // 3.2 VERIFICAÇÃO DE ESTOQUE (Regra de Negócio Crítica)
        // Se tinha 100 e vendeu 10, tem que ter 90.
        var produtoAtualizado = await DbContext.Produto.FindAsync(produtoId);
        
        // Se o seu SaidaService implementa a baixa de estoque, essa linha vai passar.
        // Se não implementa, vai falhar (e indicaria que falta essa lógica no Service).
        produtoAtualizado!.QuantidadeAtual.Should().Be(estoqueInicial - quantidadeVenda, 
            "a saída de produtos deve descontar do estoque atual");
    }
}