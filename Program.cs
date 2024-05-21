using System;
using System.Security.Cryptography;
using System.Text;
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

// Add services to the container.
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

// Configura��o do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:8081") // Substitua pelo endere�o do seu aplicativo React
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Aqui est� onde voc� pode usar a fun��o GenerateSecretKey() para obter a chave secreta
var secretKey = GenerateSecretKey();

builder.Services.AddSingleton<IJwtService>(new JwtService(secretKey)); // Registro do servi�o JwtService
builder.Services.AddScoped<IUserService, UserService>(); // Registro do servi�o UserService
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Registro do servi�o UserRepository

// Adicione a configura��o de controladores e formata��o JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
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

// Adicione middleware de autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Mapear controladores de API
app.MapControllers();

// Modifica��o para escutar em todas as interfaces de rede na porta 5000
app.Run("http://0.0.0.0:5000");

// Fun��o para gerar uma chave secreta com tamanho adequado
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
