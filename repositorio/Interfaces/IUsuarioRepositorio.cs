using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        public UsuarioDnit ObterUsuario(string email);
        public void Cadastrar(UsuarioDnit usuario);
        public UsuarioDnit TrocarSenha(string senha, string email);
        public RedefinicaoSenha InserirDadosRecuperacao(string uuid, int idUsuario);
        public string? ObterEmailRedefinicaoSenha(string uuid);
        public void removerUuidRedefinicaoSenha(string uuid);
    }
}
