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

        private Usuario? Obter(string email)
        {
            Usuario? usuario = usuarioRepositorio.ObterUsuario(email);

            if (usuario == null)
                throw new KeyNotFoundException();

            return usuario;
        }

        public bool ValidaLogin(UsuarioDTO usuarioDTO)
        {
            Usuario? usuarioBanco = Obter(usuarioDTO.Email);

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
            RedefinicaoSenha dadosRedefinicaoSenha = mapper.Map<RedefinicaoSenha>(redefinicaoSenhaDTO);

            string emailUsuario = usuarioRepositorio.ObterEmailRedefinicaoSenha(dadosRedefinicaoSenha.UuidAutenticacao) ?? throw new KeyNotFoundException();
            string senha = EncriptarSenha(dadosRedefinicaoSenha.Senha);

            usuarioRepositorio.TrocarSenha(emailUsuario, senha);

            emailService.EnviarEmail(emailUsuario, "Senha Atualizada", "A sua senha foi atualizada com sucesso.");

            usuarioRepositorio.RemoverUuidRedefinicaoSenha(dadosRedefinicaoSenha.UuidAutenticacao);
        }

        public void RecuperarSenha(UsuarioDTO usuarioDTO)
        {
            var usuarioEntrada = mapper.Map<UsuarioDnit>(usuarioDTO);
            Usuario usuarioBanco = Obter(usuarioEntrada.Email);

            string UuidAutenticacao = Guid.NewGuid().ToString();

            usuarioRepositorio.InserirDadosRecuperacao(UuidAutenticacao, usuarioBanco.Id);

            string mensagem = $"Recebemos uma solicitação para recuperar a sua senha.\n\n{GerarLinkDeRecuperacao(UuidAutenticacao)}";

            emailService.EnviarEmail(usuarioBanco.Email, "Link de Recuperação", mensagem);
        }
        private string GerarLinkDeRecuperacao(string UuidAutenticacao)
        {
            string baseUrl = "https://dnit.vercel.app/login";
            string link = $"{baseUrl}?token={UuidAutenticacao}";

            return link;
        }
    }
}