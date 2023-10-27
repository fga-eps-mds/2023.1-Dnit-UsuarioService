using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Abstractions;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using test.Fixtures;
using test.Stub;
using auth;
using app.Services.Interfaces;
using app.Entidades;
using app.Controllers;
using api.Usuarios;
using api.Senhas;
using api;
using Microsoft.EntityFrameworkCore;

namespace test
{
    public class UsuarioControllerTest : TestBed<Base>, IDisposable
    {
        const int CREATED = 201;
        const int INTERNAL_SERVER_ERROR = 500;
        readonly UsuarioController controller;
        readonly AppDbContext dbContext;

        public UsuarioControllerTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
            controller = fixture.GetService<UsuarioController>(testOutputHelper)!;
            dbContext.PopulaUsuarios(5);
        }

        public async Task<(string Token, string TokenAtualizacao)> AutenticarUsuario(UsuarioDTO usuario)
        {
            var resultado = await controller.Logar(usuario);

            Assert.IsType<OkObjectResult>(resultado);

            var login = (resultado as OkObjectResult)!.Value as LoginModel;
            var token = login.Token.Split(" ")[1];

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            controller.AppUsuario = new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims));
            return (token, login.TokenAtualizacao);
        }

        [Fact]
        public async Task Logar_QuandoLoginForValidado_DeveRetornarOk()
        {
            var usuario = dbContext.PopulaUsuarios(1, true).First();

            var resultado = await controller.Logar(usuario);

            Assert.IsType<OkObjectResult>(resultado);

            var login = (resultado as OkObjectResult)!.Value as LoginModel;
            Assert.NotEmpty(login.Token);
            var token = login.Token.Split(" ");
            Assert.True(token[0] == "Bearer");
            Assert.NotEmpty(token[1]);
            Assert.NotEmpty(login.TokenAtualizacao);
        }

        [Fact]
        public async Task ListarPermissoes_QuandoTiverLogado_DeveRetornarPermissoes()
        {
            var usuario = dbContext.PopulaUsuarios(1, includePerfil: true).First();

            await AutenticarUsuario(usuario);
            var permissoes = await controller.ListarPermissoes();
            Assert.NotEmpty(permissoes);
        }

        [Fact]
        public async Task AtualizarToken_QuandoTiverValido_DeveRetornarNovoToken()
        {
            var usuario = dbContext.PopulaUsuarios(1, includePerfil: true).First();

            var login = await AutenticarUsuario(usuario);
            var novoLogin = await controller.AtualizarToken(new AtualizarTokenDto
            {
                Token = login.Token,
                TokenAtualizacao = login.TokenAtualizacao,
            });
            Assert.NotEmpty(novoLogin.Token);
            Assert.NotEmpty(novoLogin.TokenAtualizacao);
            Assert.NotEqual(novoLogin.TokenAtualizacao, login.TokenAtualizacao);
            Assert.NotEqual(novoLogin.Token, login.Token);
        }

        [Fact]
        public async Task AtualizarToken_QuandoTiverInvalido_DeveRetornarNovoToken()
        {
            var usuario = dbContext.PopulaUsuarios(1, includePerfil: true).First();

            var login = await AutenticarUsuario(usuario);
            var atualizarTokenDto = new AtualizarTokenDto
            {
                Token = login.Token,
                TokenAtualizacao = login.TokenAtualizacao + "aaaa",
            };

            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await controller.AtualizarToken(atualizarTokenDto));
        }

        [Fact]
        public async Task Logar_QuandoCredenciaisForemInvalidas_DeveRetornarUnauthorized()
        {
            var usuario = dbContext.PopulaUsuarios(1).First();
            usuario.Senha = "teste";

            var resultado = await controller.Logar(usuario);

            Assert.IsType<UnauthorizedResult>(resultado);
        }

        [Fact]
        public async Task Logar_QuandoUsuarioNaoExistir_DeveRetornarNotFound()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDTO = usuarioStub.RetornarUsuarioDnitDTO();

            var resultado = await controller.Logar(usuarioDTO);

            Assert.IsType<NotFoundResult>(resultado);
        }

        [Fact]
        public async Task CadastrarUsuarioDnit_QuandoUsuarioForCadastrado_DeveRetornarCreated()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDTO = usuarioStub.RetornarUsuarioDnitDTO();

            var resultado = await controller.CadastrarUsuarioDnit(usuarioDTO);

            var objeto = Assert.IsType<ObjectResult>(resultado);
            Assert.Equal(CREATED, objeto.StatusCode);
        }

        [Fact]
        public async Task CadastrarUsuarioDnit_QuandoUsuarioJaExistir_DeveRetornarConflict()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDTO = usuarioStub.RetornarUsuarioDnitDTO();

            Mock<IUsuarioService> usuarioServiceMock = new();
            var excecao = new Npgsql.PostgresException("", "", "", "23505");

            usuarioServiceMock.Setup(service => service.CadastrarUsuarioDnit(It.IsAny<UsuarioDTO>())).Throws(excecao);

            var controller = new UsuarioController(usuarioServiceMock.Object, null);

            var resultado = await controller.CadastrarUsuarioDnit(usuarioDTO);

            usuarioServiceMock.Verify(service => service.CadastrarUsuarioDnit(usuarioDTO), Times.Once);
            var objeto = Assert.IsType<ConflictObjectResult>(resultado);
        }

        [Fact]
        public void CadastrarUsuarioTerceiro_QuandoUsuarioForCadastrado_DeveRetornarCreated()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDTO = usuarioStub.RetornarUsuarioDnitDTO();

            Mock<IUsuarioService> usuarioServiceMock = new();

            var controller = new UsuarioController(usuarioServiceMock.Object, null);

            var resultado = controller.CadastrarUsuarioTerceiro(usuarioDTO);

            usuarioServiceMock.Verify(service => service.CadastrarUsuarioTerceiro(usuarioDTO), Times.Once);
            var objeto = Assert.IsType<ObjectResult>(resultado);

            Assert.Equal(CREATED, objeto.StatusCode);
        }

        [Fact]
        public void CadastrarUsuarioTerceiro_QuandoUsuarioJaExistir_DeveRetornarConflict()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDTO = usuarioStub.RetornarUsuarioDnitDTO();

            Mock<IUsuarioService> usuarioServiceMock = new();
            var excecao = new Npgsql.PostgresException("", "", "", "23505");

            usuarioServiceMock.Setup(service => service.CadastrarUsuarioTerceiro(It.IsAny<UsuarioDTO>())).Throws(excecao);

            var controller = new UsuarioController(usuarioServiceMock.Object, null);

            var resultado = controller.CadastrarUsuarioTerceiro(usuarioDTO);

            usuarioServiceMock.Verify(service => service.CadastrarUsuarioTerceiro(usuarioDTO), Times.Once);
            var objeto = Assert.IsType<ConflictObjectResult>(resultado);
        }

        [Fact]
        public void CadastrarUsuarioTerceiro_QuandoCadastroFalhar_DeveRetornarErroInterno()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDTO = usuarioStub.RetornarUsuarioDnitDTO();

            Mock<IUsuarioService> usuarioServiceMock = new();
            var excecao = new Exception("");

            usuarioServiceMock.Setup(service => service.CadastrarUsuarioTerceiro(It.IsAny<UsuarioDTO>())).Throws(excecao);

            var controller = new UsuarioController(usuarioServiceMock.Object, null);

            var resultado = controller.CadastrarUsuarioTerceiro(usuarioDTO);

            usuarioServiceMock.Verify(service => service.CadastrarUsuarioTerceiro(usuarioDTO), Times.Once);
            var objeto = Assert.IsType<ObjectResult>(resultado);

            Assert.Equal(INTERNAL_SERVER_ERROR, objeto.StatusCode);
        }

        [Fact]
        public async void RecuperarSenha_QuandoRecuperacaoForValidada_DeveRetornarOk()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDTO = usuarioStub.RetornarUsuarioDnitDTO();

            Mock<IUsuarioService> usuarioServiceMock = new();

            var controller = new UsuarioController(usuarioServiceMock.Object, null);

            var resultado = await controller.RecuperarSenhaAsync(usuarioDTO);

            usuarioServiceMock.Verify(service => service.RecuperarSenha(usuarioDTO), Times.Once);
            Assert.IsType<OkResult>(resultado);
        }

        [Fact]
        public async Task RecuperarSenha_QuandoUsuarioNaoExistir_DeveRetornarNotFoundAsync()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDTO = usuarioStub.RetornarUsuarioDnitDTO();

            Mock<IUsuarioService> usuarioServiceMock = new();
            usuarioServiceMock.Setup(service => service.RecuperarSenha(It.IsAny<UsuarioDTO>())).Throws(new KeyNotFoundException());

            var controller = new UsuarioController(usuarioServiceMock.Object, null);

            var resultado = await controller.RecuperarSenhaAsync(usuarioDTO);

            usuarioServiceMock.Verify(service => service.RecuperarSenha(usuarioDTO), Times.Once);
            Assert.IsType<NotFoundResult>(resultado);
        }

        [Fact]
        public async void RedefinirSenha_QuandoRedefinicaoForConcluida_DeveRetornarOk()
        {
            RedefinicaoSenhaStub redefinicaoSenhaStub = new();
            var redefinicaoSenhaDTO = redefinicaoSenhaStub.ObterRedefinicaoSenhaDTO();

            Mock<IUsuarioService> usuarioServiceMock = new();

            var controller = new UsuarioController(usuarioServiceMock.Object, null);

            var resultado = await controller.RedefinirSenhaAsync(redefinicaoSenhaDTO);

            usuarioServiceMock.Verify(service => service.TrocaSenha(redefinicaoSenhaDTO), Times.Once);
            Assert.IsType<OkResult>(resultado);
        }

        [Fact]
        public async void RedefinirSenha_QuandoUsuarioNaoExistir_DeveRetornarNotFound()
        {
            RedefinicaoSenhaStub redefinicaoSenhaStub = new();
            var redefinicaoSenhaDTO = redefinicaoSenhaStub.ObterRedefinicaoSenhaDTO();

            Mock<IUsuarioService> usuarioServiceMock = new();
            usuarioServiceMock.Setup(service => service.TrocaSenha(It.IsAny<RedefinicaoSenhaDTO>())).Throws(new KeyNotFoundException());

            var controller = new UsuarioController(usuarioServiceMock.Object, null);

            var resultado = await controller.RedefinirSenhaAsync(redefinicaoSenhaDTO);

            usuarioServiceMock.Verify(service => service.TrocaSenha(redefinicaoSenhaDTO), Times.Once);
            Assert.IsType<NotFoundResult>(resultado);
        }

        [Fact]
        public async void ObterUsuariosAsync_QuandoFiltroVazio_RetornaTodosUsuarios()
        {
            var filtro = new PesquisaUsuarioFiltro
            {
                ItemsPorPagina = 10,
            };
            var usuarios = await controller.ListarAsync(filtro);
            Assert.Equal(5, usuarios.Total);
        }

        [Fact]
        public async void ObterUsuariosAsync_QuandoFiltradoPorUf_RetornaUsuariosDaUfDada()
        {   
            var filtro = new PesquisaUsuarioFiltro
            {
                ItemsPorPagina = 100,
                UfLotacao = UF.DF,
            };
            var u = dbContext.Usuario.ToList();
            u[0].UfLotacao = UF.DF;
            u[1].UfLotacao = UF.DF;
            u[2].UfLotacao = UF.DF;
            u[3].UfLotacao = UF.AM;
            u[4].UfLotacao = UF.AM;
            dbContext.SaveChanges();

            var usuarios = await controller.ListarAsync(filtro);
            var lista = await controller.ListarAsync(filtro);
            Assert.Equal(UF.DF, lista.Items[0].UfLotacao);  
            Assert.Equal(UF.DF, lista.Items[1].UfLotacao);  
            Assert.Equal(UF.DF, lista.Items[2].UfLotacao);  
            Assert.Equal(3, lista.Items.Count);
        }

        // [Fact]
        // public async Task EditarPerfilUsuario_QuandoNaoTemPermissao_ErroDePermissao()
        // {
        //     // logarUsuarioSemPermissao() perfil basico ou customizável?
        //     var usuarioId = "";
        //     var novoPerfilId = "";
        //     var ex = await Assert.ThrowsAsync<Exception>(async () 
        //         => await controller.EditarPerfilUsuario(usuarioId, novoPerfilId));
            
        //     Assert.Equal("Sem permissão", ex.Message);
        // }

        // [Fact]
        // public async Task EditarPerfilUsuario_QuandoTemPermissao_NovoPerfilEhAtribuidoAoUsuario()
        // {
        //     // logarUsuarioComPermissao() perfil de adm
        //     var usuarioId = "";
        //     var novoPerfilId = "";
        //     await controller.EditarPerfilUsuario(usuarioId, novoPerfilId);

        //     var usuarioEditado = dbContext.Usuario.Find(usuarioId)!;
        //     Assert.Equal(novoPerfilId, usuarioEditado.PerfilId.ToString());
        // }


        // [Fact]
        // public async void ObterUsuariosAsync_QuandoFiltradoPorPerfilId_RetornarUsuariosDePerfilId()
        // {
        //     var filtro = new PesquisaUsuarioFiltro
        //     {
        //         ItemsPorPagina = 100,
        //         PerfilId = Guid.Supervisor,
        //     };
        //     var u = dbContext.Usuario.ToList();
        //     u[0].UfLotacao = UF.DF;
        //     u[1].UfLotacao = UF.DF;
        //     u[2].UfLotacao = UF.DF;
        //     u[3].UfLotacao = UF.AM;
        //     u[4].UfLotacao = UF.AM;
        //     dbContext.SaveChanges();

        //     var lista = await controller.ListarAsync(filtro);
        //     Assert.Equal(UF.DF, lista.Items[0].UfLotacao);  
        //     Assert.Equal(3, lista.Items.Count);
        // }

        public new void Dispose()
        {
            dbContext.RemoveRange(dbContext.PerfilPermissoes);
            dbContext.RemoveRange(dbContext.Perfis);
            dbContext.RemoveRange(dbContext.Usuario);
            dbContext.RemoveRange(dbContext.Empresa);
        }
    }
}
