using System;
using System.Security.Cryptography;
using Ecouni_Projeto.Data;
using Ecouni_Projeto.Services.Interfaces;
using Ecouni_Projeto.Services.Repositories;
using Ecouni_Projeto.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configuração da string de conexão
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: int.MaxValue,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("*") // Substitua pelo endereço do seu aplicativo React se necessário
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Geração da chave secreta para JWT
var secretKey = GenerateSecretKey();

builder.Services.AddSingleton<IJwtService>(new JwtService(secretKey)); // Registro do serviço JwtService
builder.Services.AddScoped<IUserService, UserService>(); // Registro do serviço UserService
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Registro do serviço UserRepository

// Adição de controladores e configuração da formatação JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    });

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Uso do middleware CORS
app.UseCors("AllowReactApp");

app.UseRouting();

// Adição de middleware de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Configuração das rotas de controle
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Mapeamento de controladores de API
app.MapControllers();

// Configuração para escutar em todas as interfaces de rede na porta 5000
app.Run("http://0.0.0.0:5000");

// Função para gerar uma chave secreta com tamanho adequado
string GenerateSecretKey()
{
    using (var rng = RandomNumberGenerator.Create())
    {
        int keySize = 256 / 8; // Tamanho da chave em bytes (256 bits)
        var keyBytes = new byte[keySize];
        rng.GetBytes(keyBytes);
        return Convert.ToBase64String(keyBytes);
    }
}
