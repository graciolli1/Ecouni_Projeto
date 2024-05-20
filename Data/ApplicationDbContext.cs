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
        public DbSet<Coleta> Coleta { get; set; }
        public DbSet<Cadastrar> Cadastrar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coleta>()
                .HasOne(c => c.Cadastrar)
                .WithMany()
                .HasForeignKey(c => c.Cadastrarid)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<DownloadApp> DownloadApps { get; set; }
    }
}
