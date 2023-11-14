using api.Usuarios;
using api.Senhas;
using api;

namespace app.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<LoginModel> AutenticarUsuarioAsync(string email, string senha);
        bool ValidaLogin(UsuarioDTO usuarioDTO);
        Task TrocaSenha(RedefinicaoSenhaDTO redefinirSenhaDto);
        Task RecuperarSenha(UsuarioDTO usuarioDto);
        Task CadastrarUsuarioDnit(UsuarioDTO usuarioDTO);
        void CadastrarUsuarioTerceiro(UsuarioDTO usuarioDTO);
        Task<LoginModel> AtualizarTokenAsync(AtualizarTokenDto atualizarTokenDto);
        Task<List<Permissao>> ListarPermissoesAsync(int userId);
        Task<ListaPaginada<UsuarioModel>> ObterUsuariosAsync(PesquisaUsuarioFiltro filtro);
        Task EditarUsuarioPerfil(int usuarioId ,string novoPerfilId, api.UF novaUF, int novoMunicipio);
    }
}
