using dominio;
using repositorio.Interfaces;
using repositorio.Contexto;
using service.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System;
using System.Collections.Generic;

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

            usuarioRepositorio.Cadastrar(usuario);
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
        
        public UsuarioDnit TrocaSenha(UsuarioDTO usuarioDto)
        {
            var primeiroUsuario = mapper.Map<UsuarioDnit>(usuarioDto);

            UsuarioDnit? usuario = usuarioRepositorio.ObterUsuario(primeiroUsuario.Email);

            if (usuario == null) throw new KeyNotFoundException();

            usuarioRepositorio.TrocarSenha(primeiroUsuario.Email, primeiroUsuario.Senha);
            
            return usuario;
        }
    }
}