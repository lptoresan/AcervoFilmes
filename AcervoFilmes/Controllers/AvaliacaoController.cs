using AcervoFilmes.Models;
using AcervoFilmes.Repositories;
using AcervoFilmes.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcervoFilmes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {

        private readonly IAvaliacaoRepositorio _avaliacaoRepositorio;

        public AvaliacaoController(IAvaliacaoRepositorio avaliacaoRepositorio)
        {
            _avaliacaoRepositorio = avaliacaoRepositorio;
        }

        [HttpPost("adicionarAvaliacao")]
        public async Task<ActionResult<AvaliacaoModel>> AdicionarAvaliacao([FromBody] AvaliacaoModel avaliacaoModel, string tituloFilme)
        {
            AvaliacaoModel avaliacao = await _avaliacaoRepositorio.AdicionarAvaliacao(avaliacaoModel, tituloFilme);
            return Ok(avaliacao);
        }

        [HttpPut("atualizarAvaliacao")]
        public async Task<ActionResult<AvaliacaoModel>> AtualizarAvaliacao([FromBody] AvaliacaoModel avaliacaoModel, int id)
        {
            avaliacaoModel.Id = id;
            AvaliacaoModel avaliacao = await _avaliacaoRepositorio.AtualizarAvaliacao(avaliacaoModel, id);
            return Ok(avaliacao);
        }

        [HttpDelete("deletarAvaliacao")]
        public async Task<ActionResult<AvaliacaoModel>> RemoverAvaliacao(int id)
        {
            bool avaliacaoApagada = await _avaliacaoRepositorio.RemoverAvaliacao(id);
            return Ok(avaliacaoApagada);
        }

        [HttpGet("avaliacoes")]
        public async Task<ActionResult<List<AvaliacaoModel>>> BuscarTodasAvaliacoes()
        {
            List<AvaliacaoModel> avaliacoes = await _avaliacaoRepositorio.BuscarTodasAvaliacoes();
            return Ok(avaliacoes);
        }

        [HttpGet("avaliacoesComPaginacao")]
        public async Task<ActionResult<List<AvaliacaoModel>>> BuscarAvaliacoesComPaginacao(int pagina = 1, int tamanhoPagina = 5)
        {
            List<AvaliacaoModel> avaliacoes = await _avaliacaoRepositorio.BuscarAvaliacoesPaginado(pagina, tamanhoPagina);
            return Ok(avaliacoes);
        }

        [HttpGet("buscarAvaliacaoFilme")]
        public async Task<ActionResult<List<AvaliacaoModel>>> BuscarAvaliacoesFilme(string titulo)
        {
            List<AvaliacaoModel> avaliacoesFilme = await _avaliacaoRepositorio.BuscarAvaliacoesFilme(titulo);
            return Ok(avaliacoesFilme);
        }
    }
}
