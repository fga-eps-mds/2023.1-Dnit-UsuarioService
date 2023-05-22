using dominio;
using System.Collections.Generic;

namespace service.Interfaces
{
    public interface IUsuarioService
    {
        public UsuarioDnit Obter(UsuarioDnit usuarioDnit);
        public bool ValidaLogin(UsuarioDTO usuarioDTO);
    }
}
