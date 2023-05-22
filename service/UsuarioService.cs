using dominio;
using repositorio.Interfaces;
using service.Interfaces;
using System;
using System.Collections.Generic;

namespace service
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuarioRepositorio usuarioRepositorio;

        public UsuarioService(IUsuarioRepositorio usuarioRepositorio)
        {
            this.usuarioRepositorio = usuarioRepositorio;
        }

        public void Cadastrar(UsuarioDNIT usuario)
        {
            usuarioRepositorio.Cadastrar(usuario);
        }
    }
}
