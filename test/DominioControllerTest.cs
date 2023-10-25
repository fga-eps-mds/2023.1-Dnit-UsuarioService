using test.Fixtures;
using app.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using auth;
using api;
using System.Collections.Generic;

namespace test
{
    public class DominioControllerTest : AuthTest
    {
        DominioController dominioController;

         public DominioControllerTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dominioController = fixture.GetService<DominioController>(testOutputHelper)!;

            AutenticarUsuario(dominioController);
        }

        [Fact]
        public void ObterLista_QuandoMetodoForChamado_DeveRetornarListaDeUFs()
        {
            var resultado = dominioController.ObterLista();
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact]
        public void ObterListaDePermissoes_QuandoNaoTemPermissao_DeveBloquear()
        {
            AutenticarUsuario(dominioController, permissoes: new());
            Assert.Throws<AuthForbiddenException>(() => dominioController.ObterListaDePermissoes());
        }

        [Fact]
        public void ObterListaDePermissoes_QuandoNaoPermissao_DeveRetornarOk()
        {
            AutenticarUsuario(dominioController, permissoes: new(){Permissao.PerfilVisualizar});
            
            var resposta = dominioController.ObterListaDePermissoes();

            Assert.IsType<OkObjectResult>(resposta);

            var retorno = (resposta as OkObjectResult)!.Value as List<CategoriaPermissaoModel>;

            Assert.NotEmpty(retorno);
        }
    }
}
