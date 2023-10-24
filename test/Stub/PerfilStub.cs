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

        public static Perfil RetornaPerfil(string nome = "PerfilTeste", TipoPerfil tipo = TipoPerfil.Customizavel)
        {
            return new Perfil
            {
                Nome = nome,
                PerfilPermissoes = new List<PerfilPermissao> 
                {
                    RetornaPerfilPermissao(), RetornaPerfilPermissao(Permissao.PerfilEditar)
                },
                Tipo = tipo
            };
        }

        public static List<Perfil> RetornaListaDePerfis(int n = 4)
        {
            var lista = new List<Perfil>();

            for(int i = 0; i < n; i++)
            {
                lista.Add(RetornaPerfil("PerfilTeste_" + i.ToString()));
            }

            return lista;
        }

        public static PerfilDTO RetornaPerfilDTO(string nome = "Perfil Teste")
        {
            return new PerfilDTO
            {
                Nome = nome,
                Permissoes = new List<Permissao>(){Permissao.PerfilCadastrar}
            };
        }

        public static List<PerfilDTO> RetornaListaPerfilDTO(int n = 4)
        {
            var lista = new List<PerfilDTO>();

            for (int i = 0; i < n; i++)
            {
                lista.Add(RetornaPerfilDTO("Perfil" + i.ToString()));               
            }

            return lista;
        }
    }
}