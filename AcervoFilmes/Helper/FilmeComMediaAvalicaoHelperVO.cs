using AcervoFilmes.Enum;

namespace AcervoFilmes.Helper
{
    public class FilmeComMediaAvalicaoHelperVO
    {
        public required string Titulo { get; set; }
        public required GenerosEnum Genero { get; set; }
        public required int MesFilme { get; set; }
        public required int AnoFilme { get; set; }
        public float? MediaAvaliacao { get; set; }
        public required bool FilmeTemAvaliacao { get; set; }
    }
}
