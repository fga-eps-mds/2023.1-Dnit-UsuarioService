using test.Fixtures;
using app.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace test
{
    public class DominioControllerTest : TestBed<Base>, IDisposable
    {
        DominioController dominioController;

         public DominioControllerTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dominioController = fixture.GetService<DominioController>(testOutputHelper)!;
        }

        [Fact]
        public void ObterLista_QuandoMetodoForChamado_DeveRetornarListaDeUFs()
        {
            var resultado = dominioController.ObterLista();
            Assert.IsType<OkObjectResult>(resultado);
        }
    }
}
