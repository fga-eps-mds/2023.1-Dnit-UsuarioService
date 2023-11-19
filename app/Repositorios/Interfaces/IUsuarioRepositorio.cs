using api;
using api.Usuarios;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<ListaPaginada<Usuario>> ObterUsuariosAsync(PesquisaUsuarioFiltro filtro);
        Usuario? ObterUsuario(string? email = null, int? id = null, bool includePerfil = false);
        Usuario? ObterUsuarioPorEmail(string email);
        Task<Usuario?> ObterUsuarioAsync(int? id = null, string? email = null, bool includePerfil = false);
        UsuarioModel? TrocarSenha(string senha, string email);
        void InserirDadosRecuperacao(string uuid, int idUsuario);
        string? ObterEmailRedefinicaoSenha(string uuid);
        void RemoverUuidRedefinicaoSenha(string uuid);
        Task CadastrarUsuarioDnit(UsuarioDnit usuario);
        Task CadastrarUsuarioTerceiro(UsuarioTerceiro usuarioTerceiro);
    }
}
