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

        public void CadastrarUsuarioDnit(UsuarioDTO usuarioDTO)
        {
            var usuario = mapper.Map<UsuarioDnit>(usuarioDTO);

            usuario.Senha = EncriptarSenha(usuario);

            usuarioRepositorio.CadastrarUsuarioDnit(usuario);
        }

        public void CadastrarUsuarioTerceiro(UsuarioDTO usuarioDTO)
        {
            var usuario = mapper.Map<UsuarioTerceiro>(usuarioDTO);

            usuario.Senha = EncriptarSenha(usuario);

            usuarioRepositorio.CadastrarUsuarioTerceiro(usuario);
        }

        public string EncriptarSenha(Usuario usuario)
        {
            string salt = BCryptNet.GenerateSalt();

            return BCryptNet.HashPassword(usuario.Senha, salt);
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
    }
}