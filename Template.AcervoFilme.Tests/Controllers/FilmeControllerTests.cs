using AcervoFilmes.Controllers;
using AcervoFilmes.Models;
using AcervoFilmes.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Template.AcervoFilme.Tests.Controllers
{
    public class FilmeControllerTests
    {
        private FilmeController filmeController;

        public FilmeControllerTests() 
        { 
            filmeController = new FilmeController(new Mock<IFilmeRepositorio>().Object);
        }

        [Fact]
        public async Task Post_SendingNewFilme()
        {   

            var result = await filmeController.CadastrarFilme(new FilmeModel { Titulo = Guid.NewGuid().ToString(), 
                                                                         Genero = AcervoFilmes.Enum.GenerosEnum.Acao, 
                                                                         Ano = Guid.NewGuid().GetHashCode(), 
                                                                         Mes = Guid.NewGuid().GetHashCode()});

            var actionResult = result as ActionResult<FilmeModel>;
            var okResult = actionResult.Result as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<Microsoft.AspNetCore.Mvc.ActionResult<FilmeModel>>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Put_UpdatingFilme()
        {
            var filmeModel = new FilmeModel
            {
                Titulo = "Jacaré",
                Genero = AcervoFilmes.Enum.GenerosEnum.Drama,
                Mes = 3,
                Ano = 1972
            };
            var tituloExistente = "Jacaré";

            var result = await filmeController.AtualizarFilme(filmeModel, tituloExistente);
            var okResult = result.Result as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<Microsoft.AspNetCore.Mvc.ActionResult<FilmeModel>>(result);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task Delete_Filme()
        {
            var titulo = "Jacaré";
            var result = await filmeController.RemoverFilme(titulo);
            var okResult = result.Result as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<Microsoft.AspNetCore.Mvc.ActionResult<FilmeModel>>(result);
            Assert.NotNull(result.Result);
        }
    }
}
