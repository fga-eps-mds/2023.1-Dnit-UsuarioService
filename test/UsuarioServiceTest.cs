using AutoMapper;
using dominio;
using Moq;
using repositorio.Interfaces;
using service;
using service.Interfaces;
using System;
using System.Collections.Generic;
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

        [Fact]
        public void ValidaLogin_QuandoUsuarioCorretoForPassado_DeveRealizarLogin()
        {
            UsuarioStub usuarioStub = new();
            UsuarioDTO usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            Usuario usuarioValidoLogin = usuarioStub.RetornarUsuarioValidoLogin();

            Mock<IMapper> mapper = new();
            Mock<IUsuarioRepositorio> usuarioRepositorio = new();
            Mock<IEmailService> emailService = new();

            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(usuarioValidoLogin);

            IUsuarioService usuarioService = new UsuarioService(usuarioRepositorio.Object, mapper.Object, emailService.Object);

            Assert.True(usuarioService.ValidaLogin(usuarioDnitDTO));
        }

        [Fact]
        public void ValidaLogin_QuandoUsuarioInvalidoForPassado_NaoDeveRealizarLogin()
        {
            UsuarioStub usuarioStub = new();
            UsuarioDTO usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            Usuario usuarioInvalidoLogin = usuarioStub.RetornarUsuarioInvalidoLogin();

            Mock<IMapper> mapper = new();
            Mock<IUsuarioRepositorio> usuarioRepositorio = new();
            Mock<IEmailService> emailService = new();

            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(usuarioInvalidoLogin);

            IUsuarioService usuarioService = new UsuarioService(usuarioRepositorio.Object, mapper.Object, emailService.Object);

            Action validarLogin = () => usuarioService.ValidaLogin(usuarioDnitDTO);

            Assert.Throws<UnauthorizedAccessException>(validarLogin);
        }

        [Fact]
        public void ValidaLogin_QuandoUsuarioInexistenteForPassado_NaoDeveRealizarLogin()
        {
            UsuarioStub usuarioStub = new();
            UsuarioDTO usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            Usuario usuarioInvalidoLogin = usuarioStub.RetornarUsuarioInvalidoLogin();

            Mock<IMapper> mapper = new();
            Mock<IUsuarioRepositorio> usuarioRepositorio = new();
            Mock<IEmailService> emailService = new();

            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(value: null);

            IUsuarioService usuarioService = new UsuarioService(usuarioRepositorio.Object, mapper.Object, emailService.Object);

            Action validarLogin = () => usuarioService.ValidaLogin(usuarioDnitDTO);

            Assert.Throws<KeyNotFoundException>(validarLogin);
        }

        [Fact]
        public void RecuperarSenha_QuandoUsuarioExistir_DeveEnviarEmailDeRecuperacaoDeSenha()
        {
            UsuarioStub usuarioStub = new();
            UsuarioDTO usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            UsuarioDnit usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            Mock<IMapper> mapper = new();
            Mock<IUsuarioRepositorio> usuarioRepositorio = new();
            Mock<IEmailService> emailService = new();

            mapper.Setup(x => x.Map<UsuarioDnit>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);

            usuarioRepositorio.Setup(x => x.InserirDadosRecuperacao(It.IsAny<string>(), It.IsAny<int>()));
            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(usuarioDNIT);

            IUsuarioService usuarioService = new UsuarioService(usuarioRepositorio.Object, mapper.Object, emailService.Object);

            usuarioService.RecuperarSenha(usuarioDnitDTO);

            emailService.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void RecuperarSenha_QuandoUsuarioNaoExistir_DeveLancarException()
        {
            UsuarioStub usuarioStub = new();
            UsuarioDTO usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            UsuarioDnit usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            Mock<IMapper> mapper = new();
            Mock<IUsuarioRepositorio> usuarioRepositorio = new();
            Mock<IEmailService> emailService = new();

            mapper.Setup(x => x.Map<UsuarioDnit>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);

            usuarioRepositorio.Setup(x => x.InserirDadosRecuperacao(It.IsAny<string>(), It.IsAny<int>()));
            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(value: null);

            IUsuarioService usuarioService = new UsuarioService(usuarioRepositorio.Object, mapper.Object, emailService.Object);

            Action validarLogin = () => usuarioService.RecuperarSenha(usuarioDnitDTO);

            Assert.Throws<KeyNotFoundException>(validarLogin);
        }

        [Fact]
        public void TrocaSenha_QuandoUuidForValido_DeveTrocarSenha()
        {
            UsuarioStub usuarioStub = new();
            RedefinicaoSenhaStub redefinicaoSenhaStub = new();
            string emailRedefinicaoSenha = "usuarioTeste@gmail.com";

            UsuarioDTO usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            UsuarioDnit usuarioDNIT = usuarioStub.RetornarUsuarioDnit();
            RedefinicaoSenha redefinicaoSenha = redefinicaoSenhaStub.ObterRedefinicaoSenha();

            Mock<IMapper> mapper = new();
            Mock<IUsuarioRepositorio> usuarioRepositorio = new();
            Mock<IEmailService> emailService = new();

            mapper.Setup(x => x.Map<UsuarioDnit>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);
            mapper.Setup(x => x.Map<RedefinicaoSenha>(It.IsAny<RedefinicaoSenhaDTO>())).Returns(redefinicaoSenha);

            usuarioRepositorio.Setup(x => x.InserirDadosRecuperacao(It.IsAny<string>(), It.IsAny<int>()));
            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(value: null);

            usuarioRepositorio.Setup(x => x.ObterEmailRedefinicaoSenha(It.IsAny<string>())).Returns(emailRedefinicaoSenha);

            IUsuarioService usuarioService = new UsuarioService(usuarioRepositorio.Object, mapper.Object, emailService.Object);

            usuarioService.TrocaSenha(redefinicaoSenhaStub.ObterRedefinicaoSenhaDTO());

            emailService.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            usuarioRepositorio.Verify(x => x.RemoverUuidRedefinicaoSenha(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void TrocaSenha_QuandoUuidNaoForValido_DeveLancarException()
        {
            UsuarioStub usuarioStub = new();
            RedefinicaoSenhaStub redefinicaoSenhaStub = new();

            UsuarioDTO usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            UsuarioDnit usuarioDNIT = usuarioStub.RetornarUsuarioDnit();
            RedefinicaoSenha redefinicaoSenha = redefinicaoSenhaStub.ObterRedefinicaoSenha();

            Mock<IMapper> mapper = new();
            Mock<IUsuarioRepositorio> usuarioRepositorio = new();
            Mock<IEmailService> emailService = new();

            mapper.Setup(x => x.Map<UsuarioDnit>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);
            mapper.Setup(x => x.Map<RedefinicaoSenha>(It.IsAny<RedefinicaoSenhaDTO>())).Returns(redefinicaoSenha);

            usuarioRepositorio.Setup(x => x.InserirDadosRecuperacao(It.IsAny<string>(), It.IsAny<int>()));
            usuarioRepositorio.Setup(x => x.ObterUsuario(It.IsAny<string>())).Returns(value: null);

            usuarioRepositorio.Setup(x => x.ObterEmailRedefinicaoSenha(It.IsAny<string>())).Returns(value: null);

            IUsuarioService usuarioService = new UsuarioService(usuarioRepositorio.Object, mapper.Object, emailService.Object);

            Action trocarSenha = () => usuarioService.TrocaSenha(redefinicaoSenhaStub.ObterRedefinicaoSenhaDTO());

            Assert.Throws<KeyNotFoundException>(trocarSenha);

            emailService.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            usuarioRepositorio.Verify(x => x.RemoverUuidRedefinicaoSenha(It.IsAny<string>()), Times.Never);
        }
    }
}