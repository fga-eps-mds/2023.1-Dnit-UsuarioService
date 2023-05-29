using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        public UsuarioDnit ObterUsuario(string email);
        public void Cadastrar(UsuarioDnit usuario);
        public UsuarioDnit TrocarSenha(string senha, string email);
        public RedefinicaoSenha ObterDadosRedefinicaoSenha(int id);
        public RedefinicaoSenha InserirDadosRecuperacao(string uuid, int idUsuario);
        public int? ObterIdRedefinicaoSenha(string uuid);


    }
}
