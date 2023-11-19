using System.IO;
using app.Controllers;
using app.Entidades;
using auth;
using test.Stub;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.Fixtures;
using Xunit.Abstractions;
using api;
using api.Perfis;
using System.Threading.Tasks;
using System.Data.Common;
using System.Collections.Generic;


namespace test
{
    public class PerfilControllerTest : AuthTest, IDisposable
    {
        readonly AppDbContext dbContext;
        readonly PerfilController perfilController;

        public PerfilControllerTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
            perfilController = fixture.GetService<PerfilController>(testOutputHelper)!;

            AutenticarUsuario(perfilController);
        }

        [Fact]
        public void CriarPerfil_QuandoNaoTemPermissao_DeveBloquear()
        {
            var perfil = PerfilStub.RetornaPerfilDTO();
            
            AutenticarUsuario(perfilController, permissoes: new());
            Assert.Throws<AuthForbiddenException>(() => perfilController.CriarPerfil(perfil));
        }

        [Fact]
        public void CriarPerfil_QuandoTemPermissao_DeveRetornarOk()
        {
            var perfil = PerfilStub.RetornaPerfilDTO();
            
            var resposta = perfilController.CriarPerfil(perfil);
            var retorno =  (resposta as OkObjectResult)!.Value as PerfilModel;

            Assert.IsType<OkObjectResult>(resposta);
            Assert.NotNull(retorno);
            Assert.Equal(perfil.Nome, retorno.Nome);
        }

        [Fact]
        public async Task EditarPerfil_QuandoNaoExiste_DeveRetornarNotFound()
        {
            var perfil = PerfilStub.RetornaPerfilDTO();
            var resposta = await perfilController.EditarPerfil(Guid.NewGuid(), perfil);

            Assert.IsType<NotFoundObjectResult>(resposta);

            var retorno = (resposta as NotFoundObjectResult)!.Value as string;

            Assert.Equal("Perfil não encontrado", retorno);
        }

        [Fact]
        public async Task EditarPerfil_QuandoNaoTemPermissao_DeveBloquear()
        {
            var perfil = PerfilStub.RetornaPerfilDTO();

            AutenticarUsuario(perfilController, permissoes: new(){Permissao.PerfilCadastrar});

            var resposta = perfilController.CriarPerfil(perfil);
            var retorno =  (resposta as OkObjectResult)!.Value as PerfilModel;

            perfil.Nome = "Novo Nome";

            AutenticarUsuario(perfilController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await perfilController.EditarPerfil(retorno.Id, perfil));
        }

        [Fact]
        public async Task EditarPerfil_QuandoTemPermissao_DeveRetornarOk()
        {
            var perfil = PerfilStub.RetornaPerfilDTO();

            var resposta = perfilController.CriarPerfil(perfil);
            var retorno =  (resposta as OkObjectResult)!.Value as PerfilModel;

            perfil.Nome = "Novo Nome";

            var respostaEditar = await perfilController.EditarPerfil(retorno.Id, perfil);
            var perfilEditado = (respostaEditar as OkObjectResult)!.Value as PerfilModel;

            Assert.IsType<OkObjectResult>(resposta);
            Assert.NotNull(perfilEditado);
            Assert.NotEqual(retorno.Nome, perfilEditado.Nome);
        }

        [Fact]
        public async Task ExcluirPerfil_QuandoNaoTemPermissao_DeveBloquear()
        {
            AutenticarUsuario(perfilController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await perfilController.ExcluirPerfil(Guid.NewGuid()));
        }

        [Fact]
        public async Task ExcluirPerfil_QuandoTemPermissaoEPerfilNaoExiste_DeveLancarNotFound()
        {
            var resposta = await perfilController.ExcluirPerfil(Guid.NewGuid());

            Assert.IsType<NotFoundObjectResult>(resposta);
        }

        [Fact]
        public async Task ExcluirPerfil_QuandoTemPermissaoEPerfilBasico_DeveRetornarStatusCode400()
        {
            var perfil = PerfilStub.RetornaPerfil(tipo: TipoPerfil.Basico);
            perfil.Id = Guid.NewGuid();

            dbContext.Perfis.Add(perfil);
            dbContext.SaveChanges();

            var resposta = await perfilController.ExcluirPerfil(perfil.Id);
            var retorno = resposta as ObjectResult;

            Assert.IsType<ObjectResult>(resposta);
            Assert.Equal(400, retorno.StatusCode);
            Assert.Equal("Esse Perfil não pode ser excluído.", retorno.Value.ToString());
        }

        [Fact]
        public async Task ExcluirPerfil_QuandoTemPermissaoEPerfilAdministrador_DeveRetornarStatusCode400()
        {
            var perfil = PerfilStub.RetornaPerfil(tipo: TipoPerfil.Administrador);
            perfil.Id = Guid.NewGuid();

            dbContext.Perfis.Add(perfil);
            dbContext.SaveChanges();

            var resposta = await perfilController.ExcluirPerfil(perfil.Id);
            var retorno = resposta as ObjectResult;

            Assert.IsType<ObjectResult>(resposta);
            Assert.Equal(400, retorno.StatusCode);
            Assert.Equal("Esse Perfil não pode ser excluído.", retorno.Value.ToString());
        }

        [Fact]
        public async Task ExcluirPerfil_QuandoTemPermissaoEPerfilCustomizavel_DeveRetornarOk()
        {
            var perfil = PerfilStub.RetornaPerfil();
            perfil.Id = Guid.NewGuid();

            dbContext.Perfis.Add(perfil);
            dbContext.SaveChanges();

            var resposta = await perfilController.ExcluirPerfil(perfil.Id);
            
            Assert.IsType<OkObjectResult>(resposta);
        }

        [Fact]
        public async Task ListarPerfis_QuandoNaoTemPermissao_DeveBloquear()
        {
            AutenticarUsuario(perfilController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await perfilController.ListarPerfis(1, 20));
        }

        [Fact]
        public async Task ListarPerfis_QuandoTemPermissao_DeveRetornarOk()
        {
            var lista = PerfilStub.RetornaListaPerfilDTO(5);
            var administrador = PerfilStub.RetornaPerfil(tipo: TipoPerfil.Administrador);

            dbContext.Perfis.Add(administrador);
            dbContext.SaveChanges();

            lista.ForEach(p => perfilController.CriarPerfil(p));

            var resposta = await perfilController.ListarPerfis(1, 10);

            Assert.IsType<OkObjectResult>(resposta);

            var listaRetorno = (resposta as OkObjectResult)!.Value as List<PerfilModel>;

            Assert.NotEmpty(listaRetorno);
            Assert.Equal(6, listaRetorno.Count);
        }

        [Fact]
        public async Task ObterPorId_QuandoNaoTemPermissao_DeveRetornarBloquear()
        {
            AutenticarUsuario(perfilController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await perfilController.ObterPorId(Guid.NewGuid()));
        }

        
        [Fact]
        public async Task ObterPorId_QuandoNaoExiste_DeveRetornarNotFound()
        {
            AutenticarUsuario(perfilController, permissoes: new(){Permissao.PerfilVisualizar});

            var resposta = await perfilController.ObterPorId(Guid.NewGuid());

            Assert.IsType<NotFoundObjectResult>(resposta);

            var retorno = (resposta as NotFoundObjectResult)!.Value as string;

            Assert.Equal("Perfil não encontrado", retorno);
        }

        [Fact]
        public async Task ObterPorId_QuandoTemPermissao_DeveRetornarOk()
        {
            AutenticarUsuario(perfilController, permissoes: new(){Permissao.PerfilVisualizar,Permissao.PerfilCadastrar});
            var perfil = PerfilStub.RetornaPerfilDTO();
            var resposta = perfilController.CriarPerfil(perfil);
            var perfilCriado = (resposta as OkObjectResult)!.Value as PerfilModel;

            Assert.IsType<OkObjectResult>(resposta);

            var respostaObter = await perfilController.ObterPorId(perfilCriado.Id);

            Assert.IsType<OkObjectResult>(respostaObter);

            var retorno = (respostaObter as OkObjectResult)!.Value as PerfilModel;
            
            Assert.Equal(perfilCriado.Id, retorno.Id);
        }

        public new void Dispose()
        {
            dbContext.RemoveRange(dbContext.PerfilPermissoes);
            dbContext.RemoveRange(dbContext.Perfis);
            dbContext.SaveChanges();
        }
    }
}