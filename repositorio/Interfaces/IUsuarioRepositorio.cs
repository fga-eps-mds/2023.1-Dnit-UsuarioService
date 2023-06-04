using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        public Usuario? ObterUsuario(string email);
        public UsuarioDnit TrocarSenha(string senha, string email);
        public RedefinicaoSenha InserirDadosRecuperacao(string uuid, int idUsuario);
        public string? ObterEmailRedefinicaoSenha(string uuid);
        public void RemoverUuidRedefinicaoSenha(string uuid);
        public void CadastrarUsuarioDnit(UsuarioDnit usuario);
        public void CadastrarUsuarioTerceiro(UsuarioTerceiro usuarioTerceiro);
    }
}
