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

        public UsuarioService(IUsuarioRepositorio usuarioRepositorio, IMapper mapper)
        {
            this.usuarioRepositorio = usuarioRepositorio;
            this.mapper = mapper;
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

        public RedefinicaoSenha TrocaSenha(RedefinicaoSenhaDTO redefinicaoSenhaDto)
        {
            var dadosRedefinicaoSenha = mapper.Map<RedefinicaoSenha>(redefinicaoSenhaDto);
            
            dadosRedefinicaoSenha.Senha = EncriptarSenha(dadosRedefinicaoSenha.Senha);

            usuarioRepositorio.TrocarSenha(dadosRedefinicaoSenha.Email, dadosRedefinicaoSenha.Senha);

            return dadosRedefinicaoSenha;
        }



        public void EnviarEmail(string emailDestinatario, string assunto, string corpo)
        {

            MailMessage mensagem = new MailMessage();

            string emailRemetente = "email@gmail.com";
            string senhaRemetente = "senha";

            mensagem.From = new MailAddress(emailRemetente);
            mensagem.Subject = assunto;
            mensagem.To.Add(new MailAddress(emailDestinatario));
            mensagem.Body = corpo;

            var clienteSmtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(emailRemetente, senhaRemetente),
                EnableSsl = true,

            };
            clienteSmtp.Send(mensagem);
        }

        public bool ValidaRedefinicaoDeSenha(RedefinicaoSenhaDTO redefinicaoSenhaDto)
        {
            var dadosRedefinicao = mapper.Map<RedefinicaoSenhaDTO>(redefinicaoSenhaDto);
            var usuarioBanco = Obter(dadosRedefinicao.Email);
            var dadosRedefinicaoBanco = usuarioRepositorio.ObterDadosRedefinicaoSenha(usuarioBanco.Id);

            if (dadosRedefinicao.Uuid == dadosRedefinicaoBanco.Uuid) return true;
            else throw new UnauthorizedAccessException();
        }

        public void RecuperarSenha(UsuarioDTO usuarioDto)
        {
            var usuarioEntrada = mapper.Map<UsuarioDnit>(usuarioDto);
            UsuarioDnit usuarioBanco = Obter(usuarioEntrada.Email);

            string Uuid = Guid.NewGuid().ToString();
            EnviarEmail(usuarioBanco.Email, "Link de recuperação", GerarLinkDeRecuperacao(Uuid));

            Console.WriteLine(usuarioBanco.Id);
            usuarioRepositorio.InserirDadosRecuperacao(Uuid, usuarioBanco.Id);

        }
    }
}   