using AcervoFilmes.Models;

namespace AcervoFilmes.Repositories.Interfaces
{
    public interface IAvaliacaoRepositorio
    {
        Task<List<AvaliacaoModel>> BuscarTodasAvaliacoes();
        Task<AvaliacaoModel> BuscarAvaliacaoId(int id);
        Task<List<AvaliacaoModel>> BuscarAvaliacoesPaginado(int pagina, int tamanhoPagina);
        Task<List<AvaliacaoModel>> BuscarAvaliacoesFilme(string titulo);
        Task<AvaliacaoModel> AdicionarAvaliacao(AvaliacaoModel avaliacao, string tituloFilme);
        Task<AvaliacaoModel> AtualizarAvaliacao(AvaliacaoModel avaliacao, int id);
        Task<bool> RemoverAvaliacao(int id);
    }
}
