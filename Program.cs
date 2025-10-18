using Microsoft.EntityFrameworkCore;
using Hortifruti.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
builder.Services.AddApplicationServices();

var app = builder.Build();

app.MapControllers(); // Precisa ser colocado antes do MapOpenApi e do MapScalarApiReference, pois os dois se baseiam nesse mapeamento dos controllers(?)

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Layout = ScalarLayout.Classic;
        options.Theme = ScalarTheme.Moon;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();
