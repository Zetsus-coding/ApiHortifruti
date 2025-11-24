using ApiHortifruti.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage; // Necessário para NonRetryingExecutionStrategy
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MySql;

namespace ApiHortifruti.IntegrationTests.Integration.config;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
        .WithImage("mysql:8.0") 
        .WithDatabase("hortifruti_test_db")
        .WithUsername("testuser")
        .WithPassword("testpass")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Remove a configuração original do banco
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

            var connectionString = _mySqlContainer.GetConnectionString();

            // 2. Configura o banco de teste FORÇANDO "Sem Retry"
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    // AQUI ESTÁ A CORREÇÃO MÁGICA:
                    mysqlOptions => mysqlOptions.ExecutionStrategy(d => new NonRetryingExecutionStrategy(d))
                ));
        });
    }

    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
    }
}