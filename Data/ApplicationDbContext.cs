using Ecouni_Projeto.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecouni_Projeto.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cadastrar> Cadastrar { get; set; }
        public DbSet<DownloadApp> DownloadApps { get; set; }
        public DbSet<SobreNos> SobreNos { get; set; }
        public DbSet<Contato> Contatos { get; set; }
    }
}
