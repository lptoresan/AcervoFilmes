using AcervoFilmes.Controllers;
using AcervoFilmes.Models;
using AcervoFilmes.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Template.AcervoFilme.Tests.Controllers
{
    public class AvaliacaoControllerTests
    {
        private AvaliacaoController avaliacaoController;

        public AvaliacaoControllerTests()
        {
            avaliacaoController = new AvaliacaoController(new Mock<IAvaliacaoRepositorio>().Object);
        }

        [Fact]
        public async Task Post_SendingNewAvaliacao()
        {
            AvaliacaoModel avaliacaoModel = new AvaliacaoModel
            {
                Nota = 4,
                Comentario = "Achei bom",
                FilmeTitulo = "Jacaré"
            };

            string titulo = "Jacaré";

            var result = await avaliacaoController.AdicionarAvaliacao(avaliacaoModel, titulo);

            var actionResult = result as ActionResult<AvaliacaoModel>;
            var okResult = actionResult.Result as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<Microsoft.AspNetCore.Mvc.ActionResult<AvaliacaoModel>>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Put_UpdatingAvalicao()
        {
            var avaliacaoModel = new AvaliacaoModel
            {
                Nota = 4,
                Comentario = "Achei bom",
                FilmeTitulo = "Jacaré"
            };
            var idAvaliacao = 1;

            var result = await avaliacaoController.AtualizarAvaliacao(avaliacaoModel, idAvaliacao);
            var okResult = result.Result as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<Microsoft.AspNetCore.Mvc.ActionResult<AvaliacaoModel>>(result);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task Delete_Avaliacao()
        {
            var id = 1;
            var result = await avaliacaoController.RemoverAvaliacao(id);
            var okResult = result.Result as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<Microsoft.AspNetCore.Mvc.ActionResult<AvaliacaoModel>>(result);
            Assert.NotNull(result.Result);
        }
    }
}
