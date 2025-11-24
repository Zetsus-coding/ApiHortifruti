using System.Net;
using System.Net.Http.Json;
using ApiHortifruti.Domain;
using ApiHortifruti.IntegrationTests.Integration.config;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiHortifruti.IntegrationTests.Integration;

public class FinanceiroIntegrationTests : BaseIntegrationTest
{
    public FinanceiroIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Financeiro: Lucro Semanal deve subtrair Entradas de Saídas (Últimos 7 dias)")]
    public async Task ObterLucroSemanal_DeveRetornarCalculoCorreto()
    {
        var hoje = DateOnly.FromDateTime(DateTime.Now);

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // =================================================================
            // LIMPEZA DE DADOS (CORREÇÃO DO ERRO)
            // Removemos dados de outros testes para não interferir na soma
            // =================================================================
            db.Entrada.RemoveRange(db.Entrada);
            db.Saida.RemoveRange(db.Saida);
            await db.SaveChangesAsync();
            // =================================================================

            // Agora criamos os dados limpos para este teste
            var motivo = new MotivoMovimentacao { Motivo = "Compra Estoque", Ativo = true };
            db.MotivoMovimentacao.Add(motivo);

            var fornecedor = new Fornecedor 
            { 
                NomeFantasia = "Fornecedor Teste", 
                CadastroPessoa = "00000000000000",
                Email = "f@teste.com",
                Telefone = "00000000",
                Ativo = true,
                DataRegistro = hoje
            };
            db.Fornecedor.Add(fornecedor);
            
            await db.SaveChangesAsync();

            // Venda de R$ 500,00
            var saidaHoje = new Saida
            {
                MotivoMovimentacaoId = motivo.Id,
                DataSaida = hoje,
                HoraSaida = TimeOnly.FromDateTime(DateTime.Now),
                ValorTotal = 500.00m,
                ValorFinal = 500.00m,
                Desconto = false,
                CadastroCliente = "Cliente Balcão"
            };
            db.Saida.Add(saidaHoje);

            // Compra de R$ 150,00
            var entradaHoje = new Entrada
            {
                FornecedorId = fornecedor.Id,
                MotivoMovimentacaoId = motivo.Id,
                DataCompra = hoje,
                PrecoTotal = 150.00m,
                NumeroNota = "NF-LUCRO-01"
            };
            db.Entrada.Add(entradaHoje);

            // Dado antigo (Ignorar)
            var saidaAntiga = new Saida
            {
                MotivoMovimentacaoId = motivo.Id,
                DataSaida = hoje.AddDays(-10),
                HoraSaida = TimeOnly.MinValue,
                ValorTotal = 1000.00m,
                ValorFinal = 1000.00m,
                Desconto = false
            };
            db.Saida.Add(saidaAntiga);

            await db.SaveChangesAsync();
        }

        var response = await HttpClient.GetAsync("/api/Financeiro/lucro-semanal");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var lucroRetornado = await response.Content.ReadFromJsonAsync<decimal>();

        // 500 - 150 = 350
        lucroRetornado.Should().Be(350.00m);
    }

    [Fact(DisplayName = "Financeiro: Gastos Mensais deve somar apenas Entradas do mês atual")]
    public async Task ObterGastosMensais_DeveConsiderarApenasMesAtual()
    {
        var hoje = DateOnly.FromDateTime(DateTime.Now);
        var mesPassado = hoje.AddMonths(-1);

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // CORREÇÃO: Nome reduzido
            var motivo = new MotivoMovimentacao { Motivo = "Despesas", Ativo = true };
            var fornecedor = new Fornecedor { NomeFantasia = "Forn. Mensal", CadastroPessoa = "111", Email = "a@a.com", Telefone = "1", DataRegistro = hoje, Ativo = true };
            db.Add(motivo); db.Add(fornecedor); await db.SaveChangesAsync();

            db.Entrada.Add(new Entrada
            {
                FornecedorId = fornecedor.Id,
                MotivoMovimentacaoId = motivo.Id,
                PrecoTotal = 200.00m,
                DataCompra = hoje,
                NumeroNota = "NF-MES-ATUAL"
            });

            db.Entrada.Add(new Entrada
            {
                FornecedorId = fornecedor.Id,
                MotivoMovimentacaoId = motivo.Id,
                PrecoTotal = 500.00m,
                DataCompra = mesPassado,
                NumeroNota = "NF-MES-ANTERIOR"
            });

            await db.SaveChangesAsync();
        }

        var response = await HttpClient.GetAsync("/api/Financeiro/gastos-mensais");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var gastos = await response.Content.ReadFromJsonAsync<decimal>();
        gastos.Should().Be(200.00m);
    }

    [Fact(DisplayName = "Financeiro: Vendas Diarias deve somar apenas Saídas de hoje")]
    public async Task ObterVendasDiarias_DeveConsiderarApenasHoje()
    {
        //ARRANGE
        var hoje = DateOnly.FromDateTime(DateTime.Now);
        var ontem = hoje.AddDays(-1);

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // =================================================================
            // CORREÇÃO AQUI: Limpar dados antigos
            // Removemos todas as saídas anteriores para garantir que a soma comece do zero
            // =================================================================
            db.Saida.RemoveRange(db.Saida);
            await db.SaveChangesAsync();
            // =================================================================

            // Cria o motivo (se não existir, cria novo, ou busca um existente)
            var motivo = new MotivoMovimentacao { Motivo = "Vendas Diarias", Ativo = true };
            db.Add(motivo);
            await db.SaveChangesAsync();

            // Venda Hoje: R$ 80,00
            db.Saida.Add(new Saida
            {
                MotivoMovimentacaoId = motivo.Id,
                DataSaida = hoje,
                HoraSaida = TimeOnly.MinValue,
                ValorFinal = 80.00m,
                ValorTotal = 80.00m,
                Desconto = false
            });

            // Venda Ontem: R$ 100,00 (Não deve somar)
            db.Saida.Add(new Saida
            {
                MotivoMovimentacaoId = motivo.Id,
                DataSaida = ontem,
                HoraSaida = TimeOnly.MinValue,
                ValorFinal = 100.00m,
                ValorTotal = 100.00m,
                Desconto = false
            });

            await db.SaveChangesAsync();
        }

        // 2. ACT
        var response = await HttpClient.GetAsync("/api/Financeiro/vendas-diarias");

        // 3. ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var vendasDia = await response.Content.ReadFromJsonAsync<decimal>();

        // Agora sim deve ser apenas 80.00
        vendasDia.Should().Be(80.00m);
    }
}