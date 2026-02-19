using Microsoft.EntityFrameworkCore;
using Sistema_de_votaciones.Models;

namespace Sistema_de_votaciones.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Votante> Votantes { get; set; }
        public DbSet<Candidato> Candidatos { get; set; }

        public DbSet<Voto> Votos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Votante>()
                .Property(v => v.SiVoto)
                .HasDefaultValue(false);

            modelBuilder.Entity<Candidato>()
                .Property(c => c.votos)
                .HasDefaultValue(0);
        }
    }
}
