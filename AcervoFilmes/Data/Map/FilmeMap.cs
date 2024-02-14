using AcervoFilmes.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AcervoFilmes.Data.Map
{
    public class FilmeMap : IEntityTypeConfiguration<FilmeModel>
    {
        public void Configure(EntityTypeBuilder<FilmeModel> builder)
        {
            // Chave primária
            builder.HasKey(f => f.Titulo);

            // Propriedades obrigatórias
            builder.Property(f => f.Genero).IsRequired();
            builder.Property(f => f.Ano).IsRequired().HasMaxLength(4);
            builder.Property(f => f.Mes).IsRequired().HasMaxLength(2);
            builder.Property(f => f.StreamingsDisponivel).HasMaxLength(255);
        }
    }
}
