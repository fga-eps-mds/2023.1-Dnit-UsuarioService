using api.Usuarios;
using api.Senhas;

namespace app.Services.Interfaces
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
