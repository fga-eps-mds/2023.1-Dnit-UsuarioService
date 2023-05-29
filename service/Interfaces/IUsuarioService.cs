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
        public UsuarioDnit Obter(string email);
        public bool ValidaLogin(UsuarioDTO usuarioDTO);
        public void Cadastrar(UsuarioDTO usuarioDTO);
        public RedefinicaoSenha TrocaSenha(RedefinicaoSenhaDTO redefinirSenhaDto);
        public bool ValidaRedefinicaoDeSenha(RedefinicaoSenhaDTO redefinicaoSenhaDto);
        public void RecuperarSenha(UsuarioDTO usuarioDto);
    }
}
