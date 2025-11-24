using Microsoft.Extensions.DependencyInjection;
using ApiHortifruti.Data;

namespace ApiHortifruti.IntegrationTests.Integration.config;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    // --- ADICIONE ESTA PROPRIEDADE ---
    protected readonly IServiceScopeFactory _scopeFactory; 
    // ---------------------------------

    protected readonly HttpClient HttpClient;
    protected readonly AppDbContext DbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        // --- INICIALIZE ELA AQUI ---
        _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        // ---------------------------

        var scope = factory.Services.CreateScope();
        HttpClient = factory.CreateClient();
        DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        DbContext.Database.EnsureCreated();
    }
}