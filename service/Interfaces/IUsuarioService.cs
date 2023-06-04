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
        public bool ValidaLogin(UsuarioDTO usuarioDTO);
        public void TrocaSenha(RedefinicaoSenhaDTO redefinirSenhaDto);
        public void RecuperarSenha(UsuarioDTO usuarioDto);
        public void CadastrarUsuarioDnit(UsuarioDTO usuarioDTO);
        public void CadastrarUsuarioTerceiro(UsuarioDTO usuarioDTO);
    }
}
