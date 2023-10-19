using api.Usuarios;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IUsuarioRepositorio
    {
        public Usuario? ObterUsuario(string email);
        public UsuarioModel? TrocarSenha(string senha, string email);
        public void InserirDadosRecuperacao(string uuid, int idUsuario);
        public string? ObterEmailRedefinicaoSenha(string uuid);
        public void RemoverUuidRedefinicaoSenha(string uuid);
        public void CadastrarUsuarioDnit(UsuarioDnit usuario);
        public void CadastrarUsuarioTerceiro(UsuarioTerceiro usuarioTerceiro);
    }
}
