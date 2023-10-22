using api.Usuarios;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IUsuarioRepositorio
    {
        UsuarioModel? ObterUsuario(string email);
        Task<Usuario?> ObterUsuarioAsync(int? id = null, string? email = null, bool includePerfil = false);
        UsuarioModel? TrocarSenha(string senha, string email);
        void InserirDadosRecuperacao(string uuid, int idUsuario);
        string? ObterEmailRedefinicaoSenha(string uuid);
        void RemoverUuidRedefinicaoSenha(string uuid);
        Task CadastrarUsuarioDnit(UsuarioDnit usuario);
        Task CadastrarUsuarioTerceiro(UsuarioTerceiro usuarioTerceiro);
    }
}
