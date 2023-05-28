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

            usuario.Senha = EncriptarSenha(usuario);

            usuarioRepositorio.Cadastrar(usuario);
        }

        public string EncriptarSenha(UsuarioDnit usuarioDnit)
        {
            string salt = BCryptNet.GenerateSalt();

            return BCryptNet.HashPassword(usuarioDnit.Senha, salt);
        }

        public UsuarioDnit? Obter(UsuarioDnit usuarioDnit)
        {
            UsuarioDnit? usuario = usuarioRepositorio.ObterUsuario(usuarioDnit.Email);

            if (usuario == null) throw new KeyNotFoundException();

            return usuario;
        }

        public bool ValidaLogin(UsuarioDTO usuarioDTO)
        {
            var usuarioEntrada = mapper.Map<UsuarioDnit>(usuarioDTO);

            UsuarioDnit usuarioBanco = Obter(usuarioEntrada);

            return ValidaSenha(usuarioEntrada, usuarioBanco);
        }

        private bool ValidaSenha(UsuarioDnit usuarioEntrada, UsuarioDnit usuarioBanco)
        {
            if (BCryptNet.Verify(usuarioEntrada.Senha, usuarioBanco.Senha))
                return true;

            throw new UnauthorizedAccessException();
        }

        public UsuarioDnit TrocaSenha(UsuarioDTO usuarioDto)
        {
            var usuarioBanco = mapper.Map<UsuarioDnit>(usuarioDto);

            UsuarioDnit? usuario = Obter(usuarioBanco);

            usuarioBanco.Senha = EncriptarSenha(usuario);

            usuarioRepositorio.TrocarSenha(usuarioBanco.Email, usuarioBanco.Senha);

            enviarEmail("email", "Link de recuperação", GerarLinkDeRecuperacao());

            return usuario;
        }

        public string GerarLinkDeRecuperacao()
        {
            string token = Guid.NewGuid().ToString();
            string baseUrl = "https://dnit.vercel.app/login";
            string link = $"{baseUrl}?token={token}";
            
            return link;
        }


        public void enviarEmail(string emailDestinatario, string assunto, string corpo)
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
    }
}   