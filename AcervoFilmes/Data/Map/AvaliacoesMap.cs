using AcervoFilmes.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AcervoFilmes.Data.Map
{
    public class AvaliacoesMap : IEntityTypeConfiguration<AvaliacaoModel>
    {
        public void Configure(EntityTypeBuilder<AvaliacaoModel> builder)
        {
            // Chave primária
            builder.HasKey(a => a.Id);

            // Propriedades obrigatórias
            builder.Property(a => a.Nota).IsRequired();
            builder.Property(a => a.Comentario).IsRequired().HasMaxLength(50);

            builder.Property(a => a.FilmeTitulo).IsRequired();
            builder.HasOne(a => a.Filme);
        }
    }
}
