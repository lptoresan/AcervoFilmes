using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcervoFilmes.Models
{
    public class AvaliacaoModel
    {
        [Key, Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required int Nota { get; set; }

        public string? Comentario { get; set; }

        public string? FilmeTitulo { get; set; }

        public virtual FilmeModel? Filme { get; set; }
    }
}
