using dominio;
using repositorio.Interfaces;
using service.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System;
using BCryptNet = BCrypt.Net.BCrypt;

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
            var primeiroUsuario = mapper.Map<UsuarioDnit>(usuarioDTO);

            UsuarioDnit segundoUsuario = Obter(primeiroUsuario);

            if (ValidaEmail(primeiroUsuario, segundoUsuario) && ValidaSenha(primeiroUsuario, segundoUsuario)) return true;
            else return false;
        }

        private bool ValidaEmail(UsuarioDnit primeiroUsuario, UsuarioDnit segundoUsuario)
        {
            if (segundoUsuario.Email == primeiroUsuario.Email) return true;
            else return false;
        }

        private bool ValidaSenha(UsuarioDnit primeiroUsuario, UsuarioDnit segundoUsuario)
        {
            if (segundoUsuario.Senha == primeiroUsuario.Senha) return true;
            else throw new UnauthorizedAccessException();
        }
    }
}