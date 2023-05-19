using dominio;
using System.Collections.Generic;

namespace service.Interfaces
{
    public interface IUsuarioService
    {
        public UsuarioDnit Obter(string email);
        public bool validaLogin(UsuarioDnit primeiroUsuario);
    }
}
