using AcervoFilmes.Enum;

namespace AcervoFilmes.Helper
{
    public class AvaliacaoFilmeHelperVO
    {
        public required int AvaliacaoId { get; set; }
        public required string Titulo { get; set; }
        public required GenerosEnum Genero { get; set; }
        public required int MesFilme { get; set; }
        public required int AnoFilme { get; set; }
        public List<String>? StreamingsDisponivel { get; set; }
        public  int Nota { get; set; }
        public string? Comentario { get; set; }

    }
}
