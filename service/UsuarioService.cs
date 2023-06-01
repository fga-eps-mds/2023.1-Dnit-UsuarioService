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

        public void Cadastrar(UsuarioDTO usuarioDTO)
        {
            var usuario = mapper.Map<UsuarioDnit>(usuarioDTO);

            usuario.Senha = EncriptarSenha(usuario.Senha);

            usuarioRepositorio.Cadastrar(usuario);
        }

        public string EncriptarSenha(string senha)
        {
            string salt = BCryptNet.GenerateSalt();

            return BCryptNet.HashPassword(senha, salt);
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
        public string GerarLinkDeRecuperacao(string Uuid)
        {
            string baseUrl = "https://dnit.vercel.app/login";
            string link = $"{baseUrl}?token={Uuid}";

            return link;
        }

        public void TrocaSenha(RedefinicaoSenhaDTO redefinicaoSenhaDto)
        {
            var dadosRedefinicaoSenha = mapper.Map<RedefinicaoSenha>(redefinicaoSenhaDto);

            string emailUsuario = usuarioRepositorio.ObterEmailRedefinicaoSenha(dadosRedefinicaoSenha.Uuid) ?? throw new KeyNotFoundException();
            string senha = EncriptarSenha(dadosRedefinicaoSenha.Senha);

            usuarioRepositorio.TrocarSenha(emailUsuario, senha);

            emailService.EnviarEmail(emailUsuario, "Senha Atualizada", "A sua senha foi atualizada com sucesso.");

            usuarioRepositorio.removerUuidRedefinicaoSenha(dadosRedefinicaoSenha.Uuid);
        }

        /*public bool ValidaRedefinicaoDeSenha(RedefinicaoSenhaDTO redefinicaoSenhaDto)
        {
            var dadosRedefinicao = mapper.Map<RedefinicaoSenhaDTO>(redefinicaoSenhaDto);
            var usuarioBanco = Obter(dadosRedefinicao.Email);
            var dadosRedefinicaoBanco = usuarioRepositorio.ObterDadosRedefinicaoSenha(usuarioBanco.Id);

            if (dadosRedefinicao.Uuid == dadosRedefinicaoBanco.Uuid) return true;
            else throw new UnauthorizedAccessException();
        }*/

        public void RecuperarSenha(UsuarioDTO usuarioDto)
        {
            var usuarioEntrada = mapper.Map<UsuarioDnit>(usuarioDto);
            UsuarioDnit usuarioBanco = Obter(usuarioEntrada.Email);

            string Uuid = Guid.NewGuid().ToString();

            usuarioRepositorio.InserirDadosRecuperacao(Uuid, usuarioBanco.Id);

            string mensagem = $"Recebemos uma solicitação para recuperar a sua senha.\n\n{GerarLinkDeRecuperacao(Uuid)}";
            emailService.EnviarEmail(usuarioBanco.Email, "Link de Recuperação", mensagem);
        }

        public int? ObterIdRedefinicaoSenha(string uuid)
        {
            throw new NotImplementedException();
        }
    }
}