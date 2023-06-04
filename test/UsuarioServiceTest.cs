using AutoMapper;
using dominio;
using Moq;
using repositorio.Interfaces;
using service;
using service.Interfaces;
using System;
using test.Stub;
using Xunit;

namespace test
{
    public class UsuarioServiceTest
    {
        [Fact]
        public void CadastrarUsuarioDnit_QuandoUsuarioDnitForPassado_DeveCadastrarUsuarioDnitComSenhaEncriptografada()
        {
            UsuarioStub usuarioStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            string senhaAntesDaEncriptografia = usuarioDNIT.Senha;

            Mock<IMapper> mapper = new();
            Mock<IUsuarioRepositorio> usuarioRepositorio = new();
            Mock<IEmailService> emailService = new();

            mapper.Setup(x => x.Map<UsuarioDnit>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);

            IUsuarioService usuarioService = new UsuarioService(usuarioRepositorio.Object, mapper.Object, emailService.Object);

            usuarioService.CadastrarUsuarioDnit(usuarioStub.RetornarUsuarioDnitDTO());

            usuarioRepositorio.Verify(x => x.CadastrarUsuarioDnit(It.IsAny<UsuarioDnit>()), Times.Once);

            Assert.NotEqual(senhaAntesDaEncriptografia, usuarioDNIT.Senha);
        }

        [Fact]
        public void CadastrarUsuarioTerceiro_QuandoUsuarioTerceiroForPassado_DeveCadastrarUsuarioTerceiroComSenhaEncriptografada()
        {
            UsuarioStub usuarioStub = new();
            var usuarioTerceiro = usuarioStub.RetornarUsuarioTerceiro();

            string senhaAntesDaEncriptografia = usuarioTerceiro.Senha;

            Mock<IMapper> mapper = new();
            Mock<IUsuarioRepositorio> usuarioRepositorio = new();
            Mock<IEmailService> emailService = new();

            mapper.Setup(x => x.Map<UsuarioTerceiro>(It.IsAny<UsuarioDTO>())).Returns(usuarioTerceiro);

            IUsuarioService usuarioService = new UsuarioService(usuarioRepositorio.Object, mapper.Object, emailService.Object);

            usuarioService.CadastrarUsuarioTerceiro(usuarioStub.RetornarUsuarioTerceiroDTO());

            usuarioRepositorio.Verify(x => x.CadastrarUsuarioTerceiro(It.IsAny<UsuarioTerceiro>()), Times.Once);

            Assert.NotEqual(senhaAntesDaEncriptografia, usuarioTerceiro.Senha);
        }

        [Fact]
        public void CadastrarUsuarioDnit_QuandoUsuarioDnitJáExistenteForPassado_DeveLançarExececaoFalandoQueEmailJaExiste()
        {
            UsuarioStub usuarioStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            string senhaAntesDaEncriptografia = usuarioDNIT.Senha;

            Mock<IMapper> mapper = new();
            Mock<IUsuarioRepositorio> usuarioRepositorio = new();
            Mock<IEmailService> emailService = new();

            mapper.Setup(x => x.Map<UsuarioDnit>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);
            usuarioRepositorio.Setup(x => x.CadastrarUsuarioDnit(It.IsAny<UsuarioDnit>())).Throws(new InvalidOperationException("Email já cadastrado."));

            IUsuarioService usuarioService = new UsuarioService(usuarioRepositorio.Object, mapper.Object, emailService.Object);

            Action cadastrarUsuario = () => usuarioService.CadastrarUsuarioDnit(usuarioStub.RetornarUsuarioDnitDTO());

            Exception exception = Assert.Throws<InvalidOperationException>(cadastrarUsuario);

            Assert.Equal("Email já cadastrado.", exception.Message);
        }

        [Fact]
        public void CadastrarUsuarioTerceiro_QuandoUsuarioTerceiroJáExistenteForPassado_DeveLançarExececaoFalandoQueEmalJaExiste()
        {
            UsuarioStub usuarioStub = new();
            var usuarioTerceiro = usuarioStub.RetornarUsuarioTerceiro();

            string senhaAntesDaEncriptografia = usuarioTerceiro.Senha;

            Mock<IMapper> mapper = new();
            Mock<IUsuarioRepositorio> usuarioRepositorio = new();
            Mock<IEmailService> emailService = new();

            mapper.Setup(x => x.Map<UsuarioTerceiro>(It.IsAny<UsuarioDTO>())).Returns(usuarioTerceiro);
            usuarioRepositorio.Setup(x => x.CadastrarUsuarioTerceiro(It.IsAny<UsuarioTerceiro>())).Throws(new InvalidOperationException("Email já cadastrado."));

            IUsuarioService usuarioService = new UsuarioService(usuarioRepositorio.Object, mapper.Object, emailService.Object);

            Action cadastrarUsuario = () => usuarioService.CadastrarUsuarioTerceiro(usuarioStub.RetornarUsuarioTerceiroDTO());

            Exception exception = Assert.Throws<InvalidOperationException>(cadastrarUsuario);

            Assert.Equal("Email já cadastrado.", exception.Message);
        }
    }
}