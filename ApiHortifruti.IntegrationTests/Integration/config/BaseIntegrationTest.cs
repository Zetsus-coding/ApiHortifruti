using ApiHortifruti;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ApiHortifruti.IntegrationTests.Integration.Config;

[Collection("Integration Tests")] // Evita que testes de integração rodem em paralelo (opcional, mas seguro)
public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly AppDbContext DbContext;
    protected readonly HttpClient Client;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        // Cria um escopo para resolver serviços (Repositories, DbContext)
        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Garante que o banco e tabelas existam no container antes de começar
        DbContext.Database.EnsureCreated(); 

        // Cria o client HTTP que já vai enviar as credenciais do usuário fake
        Client = factory.CreateClient();
    }
}