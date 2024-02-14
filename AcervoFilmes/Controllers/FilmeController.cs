using AcervoFilmes.Enum;
using AcervoFilmes.Helper;
using AcervoFilmes.Models;
using AcervoFilmes.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AcervoFilmes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmeController : ControllerBase
    {
        
        private readonly IFilmeRepositorio _filmeRepositorio;

        public FilmeController(IFilmeRepositorio filmeRepositorio) 
        {
            _filmeRepositorio = filmeRepositorio;
        }

        [HttpPost("cadastrarFilme")]
        public async Task<ActionResult<FilmeModel>> CadastrarFilme([FromBody] FilmeModel filmeModel)
        {
            FilmeModel filme = await _filmeRepositorio.AdicionarFilme(filmeModel);
            return Ok(filme);
        }

        [HttpPut("atualizarFilme")]
        public async Task<ActionResult<FilmeModel>> AtualizarFilme([FromBody] FilmeModel filmeModel, string titulo)
        {
            filmeModel.Titulo = titulo;
            FilmeModel filme = await _filmeRepositorio.AtualizarFilme(filmeModel, titulo);
            return Ok(filme);
        }

        [HttpDelete("deletarFilme")]
        public async Task<ActionResult<FilmeModel>> RemoverFilme(string titulo)
        {
            bool filmeApagado = await _filmeRepositorio.RemoverFilme(titulo);
            return Ok(filmeApagado);
        }

        [HttpGet("filmes")]
        public async Task<ActionResult<List<FilmeModel>>> BuscarTodosFilmes()
        {
            List<FilmeModel> filmes = await _filmeRepositorio.BuscarTodosFilmes(); 
            return Ok(filmes);
        }

        [HttpGet("filmesComPaginacao")]
        public async Task<ActionResult<List<FilmeModel>>> BuscarFilmesComPaginacao(int pagina = 1, int tamanhoPagina = 5)
        {
            List<FilmeModel> filmes = await _filmeRepositorio.BuscarFilmesComPaginacao(pagina, tamanhoPagina);
            return Ok(filmes);
        }

        [HttpGet("buscarPorTitulo")]
        public async Task<ActionResult<FilmeModel>> BuscarFilmesPorTitulo(string titulo)
        {
            FilmeModel filmesTitulos = await _filmeRepositorio.BuscarFilmePeloTítulo(titulo);
            return Ok(filmesTitulos);
        }

        [HttpGet("BuscarAvalEpocaGenero")]
        public async Task<ActionResult<List<AvaliacaoFilmeHelperVO>>> BuscarAvaliacoesEpoca(int anoInicio, int anoFinal, GenerosEnum genero)
        {
            List<AvaliacaoFilmeHelperVO> avaliacoesEpoca = await _filmeRepositorio.BuscarAvaliacoesGeneroEpoca(anoInicio, anoFinal, genero);    
            return Ok(avaliacoesEpoca);
        }

        [HttpGet("BuscarFilmePorAvaliacao")]
        public async Task<ActionResult<List<AvaliacaoFilmeHelperVO>>> BuscarFilmePorAvaliacao(int? nota, string? comentario)
        {
            List<AvaliacaoFilmeHelperVO> filmesPelaAvaliacao = await _filmeRepositorio.BuscarFilmePorAvaliacao(nota, comentario);
            return Ok(filmesPelaAvaliacao);
        }

        [HttpGet("mediaAvaliacaoFilme")]
        public async Task<ActionResult<float>> BuscarMediaFilme(string titulo)
        {
            float mediaFilme = await _filmeRepositorio.MediaAvaliacaoFilme(titulo);

            return mediaFilme;
        }

        [HttpGet("mediaAvaliacaoCatalogo")]
        public async Task<ActionResult<List<FilmeComMediaAvalicaoHelperVO>>> BuscarMediaAvaliacaoCatalogo()
        {
            List<FilmeComMediaAvalicaoHelperVO> mediaCatalogo = await _filmeRepositorio.MediaAvaliacaoTodosFilmes();

            return mediaCatalogo;
        }

        [HttpGet("mediaAvaliacaoGeneroEpoca")]
        public async Task<float> MediaAvaliacaoGeneroEpoca(GenerosEnum genero, int anoInicio, int anoFinal)
        {
            float mediaGeneroEpoca = await _filmeRepositorio.MediaAvaliacaoGeneroEpoca(genero, anoInicio, anoFinal);
            return mediaGeneroEpoca;
        }

        [HttpGet("filmesAno")]
        public async Task<ActionResult<List<FilmeModel>>> FilmesAno(int ano)
        {
            List<FilmeModel> filmesAno = await _filmeRepositorio.FilmesAno(ano);
            return filmesAno;
        }

        [HttpGet("streamingDoFilme")]
        public async Task<ActionResult<List<string>>> StreamingsDispFilme(string titulo)
        {
            List<string> streamingsFilme = await _filmeRepositorio.StreamingsDispFilme(titulo);
            return streamingsFilme;
        }

        [HttpGet("filmesNoStreaming")]
        public async Task<ActionResult<List<FilmeModel>>> FilmesNoStreaming(string streaming)
        {
            List<FilmeModel> filmesStreaming = await _filmeRepositorio.FilmesNoStreaming(streaming);
            return filmesStreaming;
        }
    }
}
