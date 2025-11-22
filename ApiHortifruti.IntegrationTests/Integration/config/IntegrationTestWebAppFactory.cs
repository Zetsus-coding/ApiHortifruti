using ApiHortifruti; // Referência para o Program e AppDbContext da API
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MySql;
using Xunit;

namespace ApiHortifruti.IntegrationTests.Integration.Config;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Configuração do Container MySQL
    // Usamos a imagem 8.0 para bater com a versão que você definiu no Program.cs
    private readonly MySqlContainer _dbContainer = new MySqlBuilder()
        .WithImage("mysql:8.0") 
        .WithDatabase("hortifrutidb_test")
        .WithUsername("root")
        .WithPassword("senha_forte_testes")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // REMOVER a configuração do banco original (appsettings)
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // ADICIONAR o banco do Container Docker
            var connectionString = _dbContainer.GetConnectionString();
            services.AddDbContext<AppDbContext>(options =>
            {
                // Usa a string de conexão gerada dinamicamente pelo Docker
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            // SOBRESCREVER a Autenticação para usar o Handler Fake
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
        });
    }

    // Inicia o container antes dos testes
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    // Derruba o container após os testes
    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}