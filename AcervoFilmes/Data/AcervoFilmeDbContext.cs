using AcervoFilmes.Data.Map;
using AcervoFilmes.Models;
using Microsoft.EntityFrameworkCore;

namespace AcervoFilmes.Data
{
    public class AcervoFilmeDbContext : DbContext
    {
        public AcervoFilmeDbContext(DbContextOptions<AcervoFilmeDbContext> options) : base(options)
        { 
        }

        // filmes e avaliacoes postos como minusculos pois o Postgre(pgAdmin4) é case sensitive
        // e obriga a usar "" caso o nome da tabela estivesse com Filmes ou Avaliacoes
        public DbSet<FilmeModel> filmes { get; set; }
        public DbSet<AvaliacaoModel> avaliacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FilmeMap());
            modelBuilder.ApplyConfiguration(new AvaliacoesMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
