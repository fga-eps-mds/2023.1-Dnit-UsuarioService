using dominio;
using repositorio.Interfaces;
using service.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Globalization;

namespace service
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuarioRepositorio usuarioRepositorio;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;

        public UsuarioService(IUsuarioRepositorio usuarioRepositorio, IMapper mapper, IEmailService emailService)
        {
            this.usuarioRepositorio = usuarioRepositorio;
            this.mapper = mapper;
            this.emailService = emailService;
        }

        public void CadastrarUsuarioDnit(UsuarioDTO usuarioDTO)
        {
            var usuario = mapper.Map<UsuarioDnit>(usuarioDTO);

            usuario.Senha = EncriptarSenha(usuario.Senha);

            usuarioRepositorio.CadastrarUsuarioDnit(usuario);
        }

        public string EncriptarSenha(string senha)
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

        public UsuarioDnit? Obter(string email)
        {
            UsuarioDnit? usuario = usuarioRepositorio.ObterUsuario(email);

            if (usuario == null) throw new KeyNotFoundException();

            return usuario;
        }

        public bool ValidaLogin(UsuarioDTO usuarioDTO)
        {
            var usuarioEntrada = mapper.Map<UsuarioDnit>(usuarioDTO);

            UsuarioDnit usuarioBanco = Obter(usuarioEntrada.Email);

            return ValidaSenha(usuarioEntrada, usuarioBanco);
        }

        private bool ValidaSenha(UsuarioDnit usuarioEntrada, UsuarioDnit usuarioBanco)
        {
            if (BCryptNet.Verify(usuarioEntrada.Senha, usuarioBanco.Senha))
                return true;

            throw new UnauthorizedAccessException();
        }
        public string GerarLinkDeRecuperacao(string UuidAutenticacao)
        {
            string baseUrl = "https://dnit.vercel.app/login";
            string link = $"{baseUrl}?token={UuidAutenticacao}";

            return link;
        }

        public void TrocaSenha(RedefinicaoSenhaDTO redefinicaoSenhaDto)
        {
            var dadosRedefinicaoSenha = mapper.Map<RedefinicaoSenha>(redefinicaoSenhaDto);

            string emailUsuario = usuarioRepositorio.ObterEmailRedefinicaoSenha(dadosRedefinicaoSenha.UuidAutenticacao) ?? throw new KeyNotFoundException();
            string senha = EncriptarSenha(dadosRedefinicaoSenha.Senha);

            usuarioRepositorio.TrocarSenha(emailUsuario, senha);

            emailService.EnviarEmail(emailUsuario, "Senha Atualizada", "A sua senha foi atualizada com sucesso.");

            usuarioRepositorio.RemoverUuidRedefinicaoSenha(dadosRedefinicaoSenha.UuidAutenticacao);
        }

        public void RecuperarSenha(UsuarioDTO usuarioDto)
        {
            var usuarioEntrada = mapper.Map<UsuarioDnit>(usuarioDto);
            UsuarioDnit usuarioBanco = Obter(usuarioEntrada.Email);

            string UuidAutenticacao = Guid.NewGuid().ToString();

            usuarioRepositorio.InserirDadosRecuperacao(UuidAutenticacao, usuarioBanco.Id);

            string mensagem = $"Recebemos uma solicitação para recuperar a sua senha.\n\n{GerarLinkDeRecuperacao(UuidAutenticacao)}";
            emailService.EnviarEmail(usuarioBanco.Email, "Link de Recuperação", mensagem);
        }
    }
}