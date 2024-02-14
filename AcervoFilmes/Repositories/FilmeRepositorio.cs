using AcervoFilmes.Data;
using AcervoFilmes.Enum;
using AcervoFilmes.Helper;
using AcervoFilmes.Models;
using AcervoFilmes.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AcervoFilmes.Repositories
{
    public class FilmeRepositorio : IFilmeRepositorio
    {
        private readonly AcervoFilmeDbContext _dbContext;
        
        public FilmeRepositorio(AcervoFilmeDbContext filmeContext) 
        { 
            _dbContext = filmeContext;
        }
 
        public async Task<List<FilmeModel>> BuscarTodosFilmes()
        {
            return await _dbContext.filmes.OrderBy(f => f.Titulo).ToListAsync();
        }

        public async Task<List<FilmeModel>> BuscarFilmesComPaginacao(int pagina, int tamanhoPagina)
        {
            int indiceInicial = (pagina - 1) * tamanhoPagina;
            int indiceFinal = indiceInicial + tamanhoPagina;

            var filmes = await _dbContext.filmes
                .OrderBy(f => f.Titulo)
                .Skip(indiceInicial)
                .Take(tamanhoPagina)
                .ToListAsync();

            return filmes;
        }

        public async Task<FilmeModel> BuscarFilmePeloTítulo(string titulo)
        {
            FilmeModel filmeTitulo = await _dbContext.filmes.FirstOrDefaultAsync(f => f.Titulo == titulo.ToUpper());

            if (filmeTitulo == null)
            {
                throw new Exception($"Não foi encontrado nenhum filme para o título {titulo}");
            }
            return filmeTitulo;
        }

        public async Task<FilmeModel> AdicionarFilme(FilmeModel filme)
        {   
            if ((int)filme.Genero < 0 || (int)filme.Genero > 19)
            {
                throw new Exception("O gênero informado não está em nosso catálogo. Informe um gênero válido para o filme.");
            }

            if (filme.Mes < 0 || filme.Mes > 12)
            {
                throw new Exception("O mês informado é inválido.");
            }

            filme.Titulo = filme.Titulo.ToUpper();
            await _dbContext.filmes.AddAsync(filme);
            await _dbContext.SaveChangesAsync();
            return filme;
        }

        public async Task<FilmeModel> AtualizarFilme(FilmeModel filme, string titulo)
        {
            FilmeModel filmeTitulo = await BuscarFilmePeloTítulo(titulo.ToUpper());

            if (filmeTitulo == null)
            {
                throw new Exception($"O filme para o título: {titulo} não foi encontrado no banco de dados.");
            }

            if ((int)filme.Genero < 0 || (int)filme.Genero > 19)
            {
                throw new Exception("O gênero informado não está em nosso catálogo. Informe um gênero válido para o filme.");
            }

            if (filme.Mes < 0 || filme.Mes > 12)
            {
                throw new Exception("O mês informado é inválido.");
            }

            filmeTitulo.Genero = filme.Genero;
            filmeTitulo.Ano = filme.Ano;
            filmeTitulo.Mes = filme.Mes;
            filmeTitulo.StreamingsDisponivel = filme.StreamingsDisponivel;

            _dbContext.filmes.Update(filmeTitulo);
            await _dbContext.SaveChangesAsync();
            return filmeTitulo;
        }

        public async Task<bool> RemoverFilme(string titulo)
        {
            FilmeModel filmeTitulo = await BuscarFilmePeloTítulo(titulo.ToUpper());

            if (filmeTitulo == null)
            {
                throw new Exception($"O filme para o título: {titulo} não foi encontrado no banco de dados.");
            }

            _dbContext.filmes.Remove(filmeTitulo);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<AvaliacaoFilmeHelperVO>> BuscarAvaliacoesGeneroEpoca(int anoInicio, int anoFinal, GenerosEnum genero)
        {
            var queryFilmes = from f in _dbContext.filmes
                              join a in _dbContext.avaliacoes on f.Titulo equals a.FilmeTitulo
                              where f.Genero.Equals(genero) && f.Ano >= anoInicio && f.Ano <= anoFinal
                              orderby f.Titulo descending
                              select new
                              {
                               idAvaliacao = a.Id,
                               tituloFilme = f.Titulo,
                               generoFilme = f.Genero,
                               mesFilme = f.Mes,
                               anoFilme = f.Ano,
                               streamingsDisp = f.StreamingsDisponivel,
                               notaFilme = a.Nota,
                               comentarioNota = a.Comentario
                               };

            var filmesComNota = await queryFilmes.ToListAsync();

            if (!filmesComNota.Any())
            {
                throw new Exception($"Não foi localizado nenhum filme lançado entre {anoInicio}" + $" à {anoFinal}" + $"para o gênero {genero.ToString()}");
            }

            List<AvaliacaoFilmeHelperVO> listaFilmesVO = new List<AvaliacaoFilmeHelperVO>();

            foreach (var item in filmesComNota)
            {
                listaFilmesVO.Add(new AvaliacaoFilmeHelperVO
                {
                    AvaliacaoId = item.idAvaliacao,
                    Titulo = item.tituloFilme,
                    Genero = item.generoFilme,
                    MesFilme = item.mesFilme,
                    AnoFilme = item.anoFilme,
                    StreamingsDisponivel = item.streamingsDisp,
                    Nota = item.notaFilme,
                    Comentario = item.comentarioNota
                }); ;
            }

            return listaFilmesVO;
        }

        public async Task<List<AvaliacaoFilmeHelperVO>> BuscarFilmePorAvaliacao(int? nota, string? comentario)
        {   
            if (nota == null && comentario == null)
            {
                throw new Exception("É preciso informar uma nota ou um comentário para podermos buscar os filmes de acordo com as avaliações.");
            }

            var queryFilmesAvaliados = from f in _dbContext.filmes
                                       join a in _dbContext.avaliacoes on f.Titulo equals a.FilmeTitulo
                                       where (comentario != null ? a.Comentario.ToLower().Contains(comentario.ToLower()) : true) 
                                                && (nota != null ? a.Nota.Equals(nota) : true)
                                       orderby f.Titulo descending
                                       select new
                                        {
                                           idAvaliacao = a.Id,
                                           tituloFilme = f.Titulo,
                                           generoFilme = f.Genero,
                                           mesFilme = f.Mes,
                                           anoFilme = f.Ano,
                                           streamingsDisp = f.StreamingsDisponivel,
                                           notaFilme = a.Nota,
                                           comentarioNota = a.Comentario
                                       };
            if (!queryFilmesAvaliados.Any())
            {
                throw new Exception($"Nenhum filme localizado para a nota/comentario pesquisado.");
            }

            var filmesComAvaliacao = await queryFilmesAvaliados.ToListAsync();

            List<AvaliacaoFilmeHelperVO> listaFilmesVO = new List<AvaliacaoFilmeHelperVO>();

            foreach (var item in filmesComAvaliacao)
            {
                listaFilmesVO.Add(new AvaliacaoFilmeHelperVO
                {
                    AvaliacaoId = item.idAvaliacao,
                    Titulo = item.tituloFilme,
                    Genero = item.generoFilme,
                    MesFilme = item.mesFilme,
                    AnoFilme = item.anoFilme,
                    StreamingsDisponivel = item.streamingsDisp,
                    Nota = item.notaFilme,
                    Comentario = item.comentarioNota
                });
            }

            return listaFilmesVO;
        }

        public async Task<float> MediaAvaliacaoFilme(string titulo)
        {
            int somaNotas = 0;
            int contagemNotas = 0;
            float mediaAvaliacoes;

            var queryAvaliacoesFilme = from f in _dbContext.filmes
                                       join a in _dbContext.avaliacoes on f.Titulo equals a.FilmeTitulo
                                       where f.Titulo.Equals(titulo.ToUpper())
                                       group a by f.Titulo into grouped
                                       select new
                                       {
                                           SomaDasNotas = grouped.Sum(a => a.Nota),
                                           TotalDeAvaliacoes = grouped.Count()
                                       };

            var avaliacoesFilme = await queryAvaliacoesFilme.ToListAsync();

            if (avaliacoesFilme.Any())
            {
                somaNotas = avaliacoesFilme[0].SomaDasNotas;
                contagemNotas = avaliacoesFilme[0].TotalDeAvaliacoes;
            } else
            {
                throw new Exception($"Não foram localizadas avaliações para o filme {titulo}");
            }

            mediaAvaliacoes = (float)somaNotas / (float)contagemNotas;
            return mediaAvaliacoes;
        }

        public async Task<List<FilmeComMediaAvalicaoHelperVO>> MediaAvaliacaoTodosFilmes()
        {   
            // Query para pegar os filmes com avaliações e posteriormente fazer o cálculo de média
            var queryAvaliacoesFilme = from f in _dbContext.filmes
                                       join a in _dbContext.avaliacoes on f.Titulo equals a.FilmeTitulo
                                       group a by f.Titulo into grouped
                                       select new
                                       {
                                           Titulo = _dbContext.filmes.First(f => f.Titulo == grouped.Key).Titulo,
                                           Genero = _dbContext.filmes.First(f => f.Titulo == grouped.Key).Genero,
                                           MesFilme = _dbContext.filmes.First(f => f.Titulo == grouped.Key).Mes,
                                           AnoFilme = _dbContext.filmes.First(f => f.Titulo == grouped.Key).Ano,
                                           SomaDasNotas = grouped.Sum(a => a.Nota),
                                           TotalDeAvaliacoes = grouped.Count()
                                       };

            var avaliacoesFilme = await queryAvaliacoesFilme.ToListAsync();

            List<FilmeComMediaAvalicaoHelperVO> listaFilmesVO = new List<FilmeComMediaAvalicaoHelperVO>();

            if (avaliacoesFilme.Any())
            {
                float mediaAvaliacao;

                foreach (var item in avaliacoesFilme)
                {
                    if (item.TotalDeAvaliacoes > 0)
                    {
                        mediaAvaliacao = (float)item.SomaDasNotas / (float)item.TotalDeAvaliacoes;
                    } else
                    {
                        mediaAvaliacao = 0;
                    }
                    listaFilmesVO.Add(new FilmeComMediaAvalicaoHelperVO
                    { 
                        Titulo = item.Titulo,
                        Genero = item.Genero,
                        MesFilme = item.MesFilme,
                        AnoFilme = item.AnoFilme,
                        MediaAvaliacao = (float)mediaAvaliacao,
                        FilmeTemAvaliacao = true
                    });
                }
            }
            // Query para adicionar ao catalógo os filmes que não possuem avaliação
            var queryFilmesSemAvaliacao = from f in _dbContext.filmes
                                          join a in _dbContext.avaliacoes on f.Titulo equals a.FilmeTitulo into gj
                                          from avaliacao in gj.DefaultIfEmpty()
                                          where avaliacao == null
                                          select new
                                          {
                                              Titulo = f.Titulo,
                                              Genero = f.Genero,
                                              MesFilme = f.Mes,
                                              AnoFilme = f.Ano,
                                          };

            var semAvaliacoesFilme = await queryFilmesSemAvaliacao.ToListAsync();

            if (semAvaliacoesFilme.Any())
            {
                foreach (var item in semAvaliacoesFilme)
                {
                    listaFilmesVO.Add(new FilmeComMediaAvalicaoHelperVO
                    {
                        Titulo = item.Titulo,
                        Genero = item.Genero,
                        MesFilme = item.MesFilme,
                        AnoFilme = item.AnoFilme,
                        MediaAvaliacao = null,
                        FilmeTemAvaliacao = false
                    });
                }
            }

            return listaFilmesVO;
        }

        public async Task<float> MediaAvaliacaoGeneroEpoca(GenerosEnum genero, int anoInicio, int anoFinal)
        {
            float mediaGeneroEpoca;

            var queryAvaliacaoGeneroEpoca = from f in _dbContext.filmes
                                            join a in _dbContext.avaliacoes on f.Titulo equals a.FilmeTitulo
                                            where f.Genero.Equals(genero) &&
                                                  f.Ano >= anoInicio &&
                                                  f.Ano <= anoFinal
                                            select new
                                            {
                                                Titulo = f.Titulo,
                                                Nota = a.Nota
                                            };

            var avaliacoesGeneroEpoca = await queryAvaliacaoGeneroEpoca.ToListAsync();

            if (avaliacoesGeneroEpoca.Any())
            {
                float somaNotas = 0;
                int contagemNotas = 0;
                foreach (var item  in avaliacoesGeneroEpoca)
                {
                    somaNotas += item.Nota;
                    contagemNotas++;
                }
                mediaGeneroEpoca = somaNotas / contagemNotas;
                return mediaGeneroEpoca;
            } else
            {
                throw new Exception($"Não foram localizadas avaliações para o genero {genero.ToString()} na época informada");
            }     
        }

        public async Task<List<FilmeModel>> FilmesAno(int ano)
        {
            List<FilmeModel> filmesAno = await _dbContext.filmes.Where(f => f.Ano == ano).ToListAsync();

            if (filmesAno == null)
            {
                throw new Exception($"Não foi encontrado nenhum filme no ano {ano}");
            }

            return filmesAno;
        }

        public async Task<List<string>> StreamingsDispFilme(string titulo)
        {
            FilmeModel filmeTitulo = await BuscarFilmePeloTítulo(titulo.ToUpper());

            if (filmeTitulo == null)
            {
                throw new Exception($"Não foi encontrado nenhum filme com o título {titulo} na nossa base de dados!");
            }

            List<String> streamingsFilme = filmeTitulo.StreamingsDisponivel;
            return streamingsFilme;
        }

        public async Task<List<FilmeModel>> FilmesNoStreaming(string streaming) 
        {
            List<FilmeModel> filmesNoStreaming = await _dbContext.filmes
                                                        .Where(f => f.StreamingsDisponivel.Any(s => s.ToUpper().Contains(streaming.ToUpper())))
                                                        .ToListAsync();
            return filmesNoStreaming;
        }
    }
}
