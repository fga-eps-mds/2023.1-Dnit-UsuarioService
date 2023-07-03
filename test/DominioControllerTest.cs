using app.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using repositorio.Interfaces;
using Xunit;

namespace test
{
    public class DominioControllerTest
    {
        [Fact]
        public void ObterLista_QuandoMetodoForChamado_DeveRetornarListaDeUFs()
        {
            Mock<IUnidadeFederativaRepositorio> usuarioFederativaRepositorioMock = new();

            var controller = new DominioController(usuarioFederativaRepositorioMock.Object);

            var resultado = controller.ObterLista();

            usuarioFederativaRepositorioMock.Verify(repo => repo.ObterDominio(), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }
    }
}
