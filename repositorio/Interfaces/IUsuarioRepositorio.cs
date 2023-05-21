using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        public void Cadastrar(UsuarioDNIT usuario);
    }
}