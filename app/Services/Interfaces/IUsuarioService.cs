using api.Usuarios;
using api.Senhas;
using api;

namespace app.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<LoginModel> AutenticarUsuarioAsync(string email, string senha);
        bool ValidaLogin(UsuarioDTONovo usuarioDTO);
        Task TrocaSenha(RedefinicaoSenhaDTO redefinirSenhaDto);
        Task RecuperarSenha(UsuarioDTONovo usuarioDto);
        Task CadastrarUsuarioDnit(UsuarioDTONovo usuarioDTO);
        void CadastrarUsuarioTerceiro(UsuarioDTONovo usuarioDTO);
        Task<LoginModel> AtualizarTokenAsync(AtualizarTokenDto atualizarTokenDto);
        Task<List<Permissao>> ListarPermissoesAsync(int userId);
        Task<ListaPaginada<UsuarioModelNovo>> ObterUsuariosAsync(PesquisaUsuarioFiltro filtro);
        Task EditarUsuarioPerfil(int usuarioId ,string novoPerfilId);
    }
}
