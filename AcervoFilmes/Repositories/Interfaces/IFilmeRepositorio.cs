using AcervoFilmes.Enum;
using AcervoFilmes.Helper;
using AcervoFilmes.Models;
using Microsoft.AspNetCore.Mvc;

namespace AcervoFilmes.Repositories.Interfaces
{
    public interface IFilmeRepositorio
    {
        Task<List<FilmeModel>> BuscarTodosFilmes();
        Task<List<FilmeModel>> BuscarFilmesComPaginacao(int pagina, int tamanhoPagina);
        Task<FilmeModel> BuscarFilmePeloTítulo(string titulo);
        Task<FilmeModel> AdicionarFilme(FilmeModel filme);
        Task<FilmeModel> AtualizarFilme(FilmeModel filme, string titulo);
        Task<bool> RemoverFilme(string titulo);
        Task<List<AvaliacaoFilmeHelperVO>> BuscarAvaliacoesGeneroEpoca(int anoInicio, int anoFinal, GenerosEnum genero);
        Task<List<AvaliacaoFilmeHelperVO>> BuscarFilmePorAvaliacao(int? nota, string? comentario);
        Task<float> MediaAvaliacaoFilme(string titulo);
        Task<List<FilmeComMediaAvalicaoHelperVO>> MediaAvaliacaoTodosFilmes();
        Task<float> MediaAvaliacaoGeneroEpoca(GenerosEnum genero, int anoInicio, int anoFinal);
        Task<List<FilmeModel>> FilmesAno(int ano);
        Task<List<String>> StreamingsDispFilme(string titulo);
        Task<List<FilmeModel>> FilmesNoStreaming(string streaming);
    }
}
