using api.Usuarios;
using api.Senhas;

namespace app.Repositorios.Interfaces
{
    public interface IUsuarioRepositorio
    {
        public UsuarioModel? ObterUsuario(string email);
        public UsuarioModel? TrocarSenha(string senha, string email);
        public void InserirDadosRecuperacao(string uuid, int idUsuario);
        public string? ObterEmailRedefinicaoSenha(string uuid);
        public void RemoverUuidRedefinicaoSenha(string uuid);
        public void CadastrarUsuarioDnit(UsuarioDnit usuario);
        public void CadastrarUsuarioTerceiro(UsuarioTerceiro usuarioTerceiro);
    }
}
