using System.Collections.Generic;
using System.ComponentModel;
using api.Perfis;
using app.Entidades;
using api;

namespace test.Stub
{
    public static class PerfilStub
    {

        public static PerfilPermissao RetornaPerfilPermissao(Permissao permissao = Permissao.EscolaCadastrar)
        {
            return new PerfilPermissao
            {
                Id = Guid.NewGuid(),
                PerfilId = Guid.NewGuid(),
                Permissao = permissao
            };
        }

        public static Perfil RetornaPerfil(string nome = "PerfilTeste")
        {
            return new Perfil
            {
                Nome = nome,
                PerfilPermissoes = new List<PerfilPermissao> 
                {
                    RetornaPerfilPermissao(), RetornaPerfilPermissao(Permissao.PerfilEditar)
                }
            };
        }

        public static List<Perfil> RetornaListaDePerfis(int n = 4)
        {
            List<Perfil> lista = new();

            for(int i = 0; i < 4; i++)
            {
                lista.Add(RetornaPerfil("PerfilTeste_" + i.ToString()));
            }

            return lista;
        }
    }
}