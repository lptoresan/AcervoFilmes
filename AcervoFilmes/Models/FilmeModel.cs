using AcervoFilmes.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcervoFilmes.Models
{
    public class FilmeModel
    {
        [Key, Column("Titulo")]
        public string? Titulo { get; set; }
        public required GenerosEnum Genero { get; set; }
        public required int Mes { get; set; }
        public required int Ano {  get; set; }
        public List<String>? StreamingsDisponivel {  get; set; }

    }
}