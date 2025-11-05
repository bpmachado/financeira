using financeira.Config;
using financeira.Controller.Common;
using financeira.Controller.Mappers;
using financeira.Repository;
using financeira.Service;
using Financeira.Data;
using Financeira.Repository;
using Financeira.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

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

// Configuração do Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext() // permite adicionar propriedades customizadas (ex: CorrelationId)
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// 🧩 Registro de serviços e repositórios
builder.Services.AddScoped<IContratoService, ContratoService>();
builder.Services.AddScoped<IContratoRepository, ContratoRepository>();
builder.Services.AddScoped<IContratoMapper, ContratoMapper>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();
builder.Services.AddScoped<IPagamentoMapper, PagamentoMapper>();
builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();


});

// Swagger e Controllers
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

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
