using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;
using System.Collections.Generic;

namespace service.Interfaces
{
    public interface IUsuarioService
    {
        public UsuarioDnit Obter(UsuarioDnit usuarioDnit);
        public bool ValidaLogin(UsuarioDTO usuarioDTO);
        public void Cadastrar(UsuarioDTO usuarioDTO);
        public UsuarioDnit TrocaSenha(UsuarioDTO usuarioDto);

    }
}
