using api.Usuarios;
using api.Senhas;
using app.Repositorios.Interfaces;
using app.Services.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System;
using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Configuration;

namespace app.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuarioRepositorio usuarioRepositorio;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public UsuarioService(IUsuarioRepositorio usuarioRepositorio, IMapper mapper, IEmailService emailService, IConfiguration configuration)
        {
            this.usuarioRepositorio = usuarioRepositorio;
            this.mapper = mapper;
            this.emailService = emailService;
            this.configuration = configuration;
        }

        public void CadastrarUsuarioDnit(UsuarioDTO usuarioDTO)
        {
            var usuario = mapper.Map<UsuarioDnit>(usuarioDTO);

            usuario.Senha = EncriptarSenha(usuario.Senha);

            usuarioRepositorio.CadastrarUsuarioDnit(usuario);
        }

        private string EncriptarSenha(string senha)
        {
            string salt = BCryptNet.GenerateSalt();

            return BCryptNet.HashPassword(senha, salt);
        }

        public void CadastrarUsuarioTerceiro(UsuarioDTO usuarioDTO)
        {
            var usuario = mapper.Map<UsuarioTerceiro>(usuarioDTO);

            usuario.Senha = EncriptarSenha(usuario.Senha);

            usuarioRepositorio.CadastrarUsuarioTerceiro(usuario);
        }

        private UsuarioModel? Obter(string email)
        {
            UsuarioModel? usuario = usuarioRepositorio.ObterUsuario(email);

            if (usuario == null)
                throw new KeyNotFoundException();

            return usuario;
        }

        public bool ValidaLogin(UsuarioDTO usuarioDTO)
        {
            UsuarioModel? usuarioBanco = Obter(usuarioDTO.Email);

            return ValidaSenha(usuarioDTO.Senha, usuarioBanco.Senha);
        }

        private bool ValidaSenha(string senhaUsuarioEntrada, string senhaUsuarioBanco)
        {
            if (BCryptNet.Verify(senhaUsuarioEntrada, senhaUsuarioBanco))
                return true;

            throw new UnauthorizedAccessException();
        }

        public void TrocaSenha(RedefinicaoSenhaDTO redefinicaoSenhaDTO)
        {
            RedefinicaoSenhaModel dadosRedefinicaoSenha = mapper.Map<RedefinicaoSenhaModel>(redefinicaoSenhaDTO);

            string emailUsuario = usuarioRepositorio.ObterEmailRedefinicaoSenha(dadosRedefinicaoSenha.UuidAutenticacao) ?? throw new KeyNotFoundException();
            string senha = EncriptarSenha(dadosRedefinicaoSenha.Senha);

            usuarioRepositorio.TrocarSenha(emailUsuario, senha);

            emailService.EnviarEmail(emailUsuario, "Senha Atualizada", "A sua senha foi atualizada com sucesso.");

            usuarioRepositorio.RemoverUuidRedefinicaoSenha(dadosRedefinicaoSenha.UuidAutenticacao);
        }

        public void RecuperarSenha(UsuarioDTO usuarioDTO)
        {
            var usuarioEntrada = mapper.Map<UsuarioDnit>(usuarioDTO);
            UsuarioModel usuarioBanco = Obter(usuarioEntrada.Email);

            string UuidAutenticacao = Guid.NewGuid().ToString();

            usuarioRepositorio.InserirDadosRecuperacao(UuidAutenticacao, usuarioBanco.Id);

            string mensagem = "Olá!!\n\n" +
                              "Recebemos uma solicitação para redefinir a sua senha.\n\n" +
                              "Clique no link abaixo para ser direcionado a página de Redefinição de Senha.\n\n" +
                              $"{GerarLinkDeRecuperacao(UuidAutenticacao)}";

            emailService.EnviarEmail(usuarioBanco.Email, "Link de Recuperação", mensagem);
        }
        private string GerarLinkDeRecuperacao(string UuidAutenticacao)
        {
            var baseUrl = configuration["RedefinirSenhaUrl"];

            string link = $"{baseUrl}?token={UuidAutenticacao}";

            return link;
        }
    }
}
