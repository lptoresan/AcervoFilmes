using AcervoFilmes.Data;
using AcervoFilmes.Models;
using AcervoFilmes.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AcervoFilmes.Repositories
{
    public class AvaliacaoRepositorio : IAvaliacaoRepositorio
    {
        private readonly AcervoFilmeDbContext _dbContext;

        public AvaliacaoRepositorio(AcervoFilmeDbContext filmeContext)
        {
            _dbContext = filmeContext;
        }

        public async Task<List<AvaliacaoModel>> BuscarTodasAvaliacoes()
        {
            return await _dbContext.avaliacoes
                .Include(a => a.Filme)
                .OrderBy(a => a.Id)
                .ToListAsync();
        }

        public async Task<AvaliacaoModel> BuscarAvaliacaoId(int id)
        {
            AvaliacaoModel avaliacaoId = await _dbContext.avaliacoes.FirstOrDefaultAsync(a => a.Id == id);

            if (avaliacaoId == null)
            {
                throw new Exception($"Não foi encontrado nenhuma avaliacao para o id {id}");
            }
            return avaliacaoId;
        }

        public async Task<List<AvaliacaoModel>> BuscarAvaliacoesPaginado(int pagina, int tamanhoPagina)
        {
            int indiceInicial = (pagina - 1) * tamanhoPagina;
            int indiceFinal = indiceInicial + tamanhoPagina;

            var avaliacoes = await _dbContext.avaliacoes
                .OrderBy(a => a.Id)
                .Skip(indiceInicial)
                .Take(tamanhoPagina)
                .ToListAsync();

            return avaliacoes;
        }

        public async Task<List<AvaliacaoModel>> BuscarAvaliacoesFilme(string tituloFilme)
        {
            List<AvaliacaoModel> avaliacoes = await BuscarTodasAvaliacoes();

            List<AvaliacaoModel> avaliacoesFilme = avaliacoes.Where(a => a.FilmeTitulo.Equals(tituloFilme.ToUpper())).ToList();
            return avaliacoesFilme;
        }

        public async Task<AvaliacaoModel> AdicionarAvaliacao(AvaliacaoModel avaliacao, string tituloFilme)
        {   
            FilmeModel filmeParaAvaliar = await _dbContext.filmes.FirstOrDefaultAsync(f => f.Titulo == tituloFilme.ToUpper());

            if (filmeParaAvaliar == null)
            {
                throw new Exception($"Não há nenhum filme com o título {tituloFilme} em nossa base de dados!");
            }

            if (avaliacao.Nota < 0 || avaliacao.Nota > 5)
            {
                throw new Exception("A nota deve estar entre 1 e 5.");
            }

            avaliacao.FilmeTitulo = tituloFilme.ToUpper();

            await _dbContext.avaliacoes.AddAsync(avaliacao);
            await _dbContext.SaveChangesAsync();
            return avaliacao;
        }

        public async Task<AvaliacaoModel> AtualizarAvaliacao(AvaliacaoModel avaliacao, int id)
        {
            AvaliacaoModel avaliacaoId = await BuscarAvaliacaoId(id);

            if (avaliacaoId == null)
            {
                throw new Exception($"A avaliação para o {id} não foi localizada!");
            }

            if (avaliacao.Nota < 0 || avaliacao.Nota > 5)
            {
                throw new Exception("A nota deve estar entre 1 e 5.");
            }

            avaliacaoId.Nota = avaliacao.Nota;
            avaliacaoId.Comentario = avaliacao.Comentario;

            _dbContext.avaliacoes.Update(avaliacaoId);
            await _dbContext.SaveChangesAsync();

            return avaliacaoId;
        }

        public async Task<bool> RemoverAvaliacao(int id)
        {
            AvaliacaoModel avaliacaoId = await BuscarAvaliacaoId(id);

            if (avaliacaoId == null)
            {
                throw new Exception($"A avaliação para o {id} não foi localizada!");
            }

            _dbContext.avaliacoes.Remove(avaliacaoId);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
