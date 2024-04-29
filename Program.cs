using System;
using System.Security.Cryptography;
using System.Text;
using Ecouni_Projeto.Data;
using Ecouni_Projeto.Services.Interfaces;
using Ecouni_Projeto.Services.Repositories;
using Ecouni_Projeto.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

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

// Aqui está onde você pode usar a função GenerateSecretKey() para obter a chave secreta
var secretKey = GenerateSecretKey();

builder.Services.AddSingleton<IJwtService>(new JwtService(secretKey)); // Registro do serviço JwtService
builder.Services.AddScoped<IUserService, UserService>(); // Registro do serviço UserService
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Registro do serviço UserRepository

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

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