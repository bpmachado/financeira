using financeira.Controller.Mappers;
using financeira.Service;
using Financeira.Data;
using Financeira.Repository;
using Financeira.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔌 Configura o DbContext com PostgreSQL e pooling
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.CommandTimeout(100);
        npgsqlOptions.EnableRetryOnFailure(); // reconexão automática
    });

    options.EnableSensitiveDataLogging();
    options.LogTo(Console.WriteLine, LogLevel.Information);
});

// 🧩 Registro de serviços e repositórios
builder.Services.AddScoped<IContratoService, ContratoService>();
builder.Services.AddScoped<IContratoRepository, ContratoRepository>();
builder.Services.AddScoped<IContratoMapper, ContratoMapper>();

// 🧩 Swagger e Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Financeira API",
        Version = "v1",
        Description = "API para gerenciamento de contratos financeiros"
    });

    c.EnableAnnotations(); // 🔥 Isso ativa os atributos como [SwaggerOperation]
});

var app = builder.Build();

// 🌐 Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ✅ Verifica conexão com o banco ANTES de iniciar o app
await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        var conectado = await db.Database.CanConnectAsync();
        Console.WriteLine(conectado
            ? "✅ Conexão com o banco de dados estabelecida."
            : "⚠️ Não foi possível conectar ao banco de dados.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro ao conectar com o banco: {ex.Message}");
    }
}

await app.RunAsync();
