using System.Security.Claims;
using financeira.Config;
using financeira.Controller.Common;
using financeira.Controller.Mappers;
using financeira.Repository;
using financeira.Service;
using Financeira.Data;
using Financeira.Repository;
using Financeira.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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


// Token fixo:
var fixedToken = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1c3VhcmlvLWZpeG8iLCJyb2xlIjoiYWRtaW4iLCJleHAiOjQwMDAwMDAwMDB9.fJ_QemGTuq69W2yocgC7qrSZwL6EXmoq9zGN2NWU3S0";

// Configuração de autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Recupera o header Authorization: Bearer + Token
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == fixedToken)
            {
                // Cria uma identidade mínima válida
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, "usuario-fixo"),
                    new Claim(ClaimTypes.Role, "Admin")
                 };
                var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                context.Principal = new ClaimsPrincipal(identity);
                context.Success();
            }

            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();


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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
