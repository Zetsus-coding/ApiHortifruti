using Microsoft.EntityFrameworkCore;
using ApiHortifruti.Data;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ApiHortifruti.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Configurações da conexão no appsettings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Conexão com o banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configurações do keycloak no appsettings
var keycloakConfig = builder.Configuration.GetSection("Keycloak");

// Configuração da autenticação JWT
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    // Configurações do JWTBearer
    .AddJwtBearer(options =>
    {
        options.Authority = keycloakConfig["Authority"];
        options.Audience = keycloakConfig["Audience"];

        // A Web API não precisa de um segredo (Client Secret) para validar o token
        options.RequireHttpsMetadata = builder.Environment.IsProduction(); // Requer HTTPS em produção

        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Valida o emissor (Keycloak Authority)
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Keycloak:Authority"],

            // Valida a audiência (Client ID)
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Keycloak:Audience"],

            // Keycloak publica as chaves públicas (JWKS) no endpoint de Authority,
            // o middleware usa isso para validar a assinatura do token.
            ValidateIssuerSigningKey = true,
            // Valida o tempo de vida do token garantindo que o mesmo não expirou
            ValidateLifetime = true,

            // Remove o tempo de tolerância quando o token expira
            ClockSkew = TimeSpan.Zero,

            // Permite a leitura de claims de roles/papéis
            // Faz um mapeamento entre as claims que o ASP.NET espera e as que o Keycloak utiliza
            RoleClaimType = "realm_access/aspnet-api/roles",
            NameClaimType = "preferred_username",
        };
        
        // Debug
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<Program>>();

                logger.LogError(
                    context.Exception,
                    "Autenticação falhou: Path: {Path}, Erro: {Message}",
                    context.Request.Path,
                    context.Exception.Message
                );

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddOpenApi(); // Adiciona o OpenApi
builder.Services.AddAuthorization(); // Adiciona o serviço de autorização para ser usar o [Authorize]
builder.Services.AddControllers();
builder.Services.AddApplicationServices(); // Faz o AddScoped automático do services e repositories (e sua respectivas interfaces)

var app = builder.Build();

app.UseHttpsRedirection();
app.UseMiddleware(typeof(GlobalExceptionHandlingMiddleware));

app.UseAuthentication(); // Identificação (leitura do token)
app.UseAuthorization(); // Autorização (utilização do [Authorize])

app.MapControllers(); // Precisa ser colocado antes do MapOpenApi e do MapScalarApiReference, pois os dois se baseiam nesse mapeamento dos controllers(?) 

// Configurações de documentação
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Layout = ScalarLayout.Classic;
        options.Theme = ScalarTheme.Moon;
    });
}

app.Run();
