using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        public UsuarioDnit ObterUsuario(string email);
        public void CadastrarUsuarioDnit(UsuarioDnit usuario);

        public void CadastrarUsuarioTerceiro(UsuarioTerceiro usuarioTerceiro);
    }
}
