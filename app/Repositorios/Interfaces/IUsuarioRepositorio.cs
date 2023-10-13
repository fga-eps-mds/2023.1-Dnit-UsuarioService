using api.Usuarios;
using api.Senhas;

namespace app.Repositorios.Interfaces
{
    public interface IUsuarioRepositorio
    {
        public UsuarioModel? ObterUsuario(string email);
        public UsuarioDnit TrocarSenha(string senha, string email);
        public RedefinicaoSenhaModel InserirDadosRecuperacao(string uuid, int idUsuario);
        public string? ObterEmailRedefinicaoSenha(string uuid);
        public void RemoverUuidRedefinicaoSenha(string uuid);
        public void CadastrarUsuarioDnit(UsuarioDnit usuario);
        public void CadastrarUsuarioTerceiro(UsuarioTerceiro usuarioTerceiro);
    }
}
