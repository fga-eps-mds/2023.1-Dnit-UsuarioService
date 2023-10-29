using AutoMapper;
using api.Senhas;
using api.Usuarios;
using Microsoft.Extensions.Configuration;
using Moq;
using app.Repositorios.Interfaces;
using app.Services;
using app.Services.Interfaces;
using System.Collections.Generic;
using test.Stub;
using test.Fixtures;
using app.Entidades;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Microsoft.Extensions.Options;
using auth;
using System.Threading.Tasks;
using app.Configuracoes;

namespace test
{
    public class UsuarioServiceTest : TestBed<Base>, IDisposable
    {
        readonly AppDbContext dbContext;
        readonly Mock<IMapper> mapper;
        readonly Mock<IUsuarioRepositorio> usuarioRepositorio;
        readonly Mock<IPerfilRepositorio> perfilRepositorio;
        readonly Mock<IEmailService> emailService;
        readonly AuthService authService;
        readonly Mock<IOptions<AuthConfig>> authConfig;
        readonly IUsuarioService usuarioServiceMock;

        public UsuarioServiceTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;

            mapper = new Mock<IMapper>();
            usuarioRepositorio = new Mock<IUsuarioRepositorio>();
            perfilRepositorio = new Mock<IPerfilRepositorio>();
            emailService = new Mock<IEmailService>();
            var senhaConfig = new SenhaConfig();
            authConfig = new Mock<IOptions<AuthConfig>>();
            authService = new AuthService(authConfig.Object);
            
            usuarioServiceMock = new UsuarioService(
                usuarioRepositorio.Object, perfilRepositorio.Object, 
                mapper.Object, emailService.Object, 
                Options.Create(senhaConfig), dbContext, authService, authConfig.Object);
        }

        [Fact]
        public async Task CadastrarUsuarioDnit_QuandoUsuarioDnitForPassado_DeveCadastrarUsuarioDnitComSenhaEncriptografada()
        {
            UsuarioStub usuarioStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            string senhaAntesDaEncriptografia = usuarioDNIT.Senha;
            mapper.Setup(x => x.Map<UsuarioDnit>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);

            await usuarioServiceMock.CadastrarUsuarioDnit(usuarioStub.RetornarUsuarioDnitDTO());

            usuarioRepositorio.Verify(x => x.CadastrarUsuarioDnit(It.IsAny<UsuarioDnit>()), Times.Once);

            Assert.NotEqual(senhaAntesDaEncriptografia, usuarioDNIT.Senha);
        }

        [Fact]
        public void CadastrarUsuarioTerceiro_QuandoUsuarioTerceiroForPassado_DeveCadastrarUsuarioTerceiroComSenhaEncriptografada()
        {
            UsuarioStub usuarioStub = new();
            var usuarioTerceiro = usuarioStub.RetornarUsuarioTerceiro();

            var senhaAntesDaEncriptografia = usuarioTerceiro.Senha;

            mapper.Setup(x => x.Map<UsuarioTerceiro>(It.IsAny<UsuarioDTO>())).Returns(usuarioTerceiro);

            usuarioServiceMock.CadastrarUsuarioTerceiro(usuarioStub.RetornarUsuarioTerceiroDTO());

            usuarioRepositorio.Verify(x => x.CadastrarUsuarioTerceiro(It.IsAny<UsuarioTerceiro>()), Times.Once);

            Assert.NotEqual(senhaAntesDaEncriptografia, usuarioTerceiro.Senha);
        }

        [Fact]
        public async Task CadastrarUsuarioDnit_QuandoUsuarioDnitJaExistenteForPassado_DeveLancarExececaoFalandoQueEmailJaExiste()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            mapper.Setup(x => x.Map<UsuarioDnit>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);
            usuarioRepositorio.Setup(x => x.CadastrarUsuarioDnit(It.IsAny<UsuarioDnit>())).Throws(new InvalidOperationException("Email já cadastrado."));

            var cadastrarUsuario = async () => await usuarioServiceMock.CadastrarUsuarioDnit(usuarioStub.RetornarUsuarioDnitDTO());

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(cadastrarUsuario);
        }

        [Fact]
        public void CadastrarUsuarioTerceiro_QuandoUsuarioTerceiroJaExistenteForPassado_DeveLancarExececaoFalandoQueEmalJaExiste()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioTerceiro = usuarioStub.RetornarUsuarioTerceiro();

            mapper.Setup(x => x.Map<UsuarioTerceiro>(It.IsAny<UsuarioDTO>())).Returns(usuarioTerceiro);
            usuarioRepositorio.Setup(x => x.CadastrarUsuarioTerceiro(It.IsAny<UsuarioTerceiro>())).Throws(new InvalidOperationException("Email já cadastrado."));

            var cadastrarUsuario = () => usuarioServiceMock.CadastrarUsuarioTerceiro(usuarioStub.RetornarUsuarioTerceiroDTO());

            var exception = Assert.Throws<InvalidOperationException>(cadastrarUsuario);

            Assert.Equal("Email já cadastrado.", exception.Message);
        }

        [Fact]
        public void ValidaLogin_QuandoUsuarioCorretoForPassado_DeveRealizarLogin()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            var usuarioValidoLogin = usuarioStub.RetornarUsuarioValidoLogin();

            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(usuarioValidoLogin);

            Assert.True(usuarioServiceMock.ValidaLogin(usuarioDnitDTO));
        }

        [Fact]
        public void ValidaLogin_QuandoUsuarioInvalidoForPassado_NaoDeveRealizarLogin()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            var usuarioInvalidoLogin = usuarioStub.RetornarUsuarioInvalidoLogin();

            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(usuarioInvalidoLogin);

            Action validarLogin = () => usuarioServiceMock.ValidaLogin(usuarioDnitDTO);

            Assert.Throws<UnauthorizedAccessException>(validarLogin);
        }

        [Fact]
        public void ValidaLogin_QuandoUsuarioInexistenteForPassado_NaoDeveRealizarLogin()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();

            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(value: null);

            Action validarLogin = () => usuarioServiceMock.ValidaLogin(usuarioDnitDTO);

            Assert.Throws<KeyNotFoundException>(validarLogin);
        }

        [Fact]
        public async void RecuperarSenha_QuandoUsuarioExistir_DeveEnviarEmailDeRecuperacaoDeSenha()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            var usuarioRetorno = usuarioStub.RetornarUsuarioDnitBanco();

            mapper.Setup(x => x.Map<UsuarioDnit>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);

            usuarioRepositorio.Setup(x => x.InserirDadosRecuperacao(It.IsAny<string>(), It.IsAny<int>()));
            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(usuarioRetorno);

            await usuarioServiceMock.RecuperarSenha(usuarioDnitDTO);

            emailService.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task RecuperarSenha_QuandoUsuarioNaoExistir_DeveLancarException()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            mapper.Setup(x => x.Map<UsuarioDnit>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);

            usuarioRepositorio.Setup(x => x.InserirDadosRecuperacao(It.IsAny<string>(), It.IsAny<int>()));
            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(value: null);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await usuarioServiceMock.RecuperarSenha(usuarioDnitDTO));
        }

        [Fact]
        public void TrocaSenha_QuandoUuidForValido_DeveTrocarSenha()
        {
            var usuarioStub = new UsuarioStub();
            var redefinicaoSenhaStub = new RedefinicaoSenhaStub();
            var emailRedefinicaoSenha = "usuarioTeste@gmail.com";

            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();
            var redefinicaoSenha = redefinicaoSenhaStub.ObterRedefinicaoSenha();

            mapper.Setup(x => x.Map<UsuarioDnit>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);
            mapper.Setup(x => x.Map<RedefinicaoSenhaModel>(It.IsAny<RedefinicaoSenhaDTO>())).Returns(redefinicaoSenha);

            usuarioRepositorio.Setup(x => x.InserirDadosRecuperacao(It.IsAny<string>(), It.IsAny<int>()));
            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(value: null);

            usuarioRepositorio.Setup(x => x.ObterEmailRedefinicaoSenha(It.IsAny<string>())).Returns(emailRedefinicaoSenha);

            usuarioServiceMock.TrocaSenha(redefinicaoSenhaStub.ObterRedefinicaoSenhaDTO());

            emailService.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            usuarioRepositorio.Verify(x => x.RemoverUuidRedefinicaoSenha(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task TrocaSenha_QuandoUuidNaoForValido_DeveLancarException()
        {
            var usuarioStub = new UsuarioStub();
            var redefinicaoSenhaStub = new RedefinicaoSenhaStub();

            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();
            var redefinicaoSenha = redefinicaoSenhaStub.ObterRedefinicaoSenha();

            mapper.Setup(x => x.Map<UsuarioDnit>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);
            mapper.Setup(x => x.Map<RedefinicaoSenhaModel>(It.IsAny<RedefinicaoSenhaDTO>())).Returns(redefinicaoSenha);

            usuarioRepositorio.Setup(x => x.InserirDadosRecuperacao(It.IsAny<string>(), It.IsAny<int>()));
            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(value: null);

            usuarioRepositorio.Setup(x => x.ObterEmailRedefinicaoSenha(It.IsAny<string>())).Returns(value: null);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await usuarioServiceMock.TrocaSenha(redefinicaoSenhaStub.ObterRedefinicaoSenhaDTO()));

            emailService.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            usuarioRepositorio.Verify(x => x.RemoverUuidRedefinicaoSenha(It.IsAny<string>()), Times.Never);
        }
    }
}
