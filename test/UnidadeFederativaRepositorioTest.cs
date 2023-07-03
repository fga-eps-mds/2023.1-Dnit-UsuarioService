using Microsoft.Data.Sqlite;
using repositorio;
using repositorio.Interfaces;
using Dapper;
using Xunit;
using System.Linq;

namespace test
{
    public class UnidadeFederativaRepositorioTest
    {
        IUnidadeFederativaRepositorio repositorio;
        SqliteConnection connection;

        public UnidadeFederativaRepositorioTest()
        {
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            repositorio = new UnidadeFederativaRepositorio(contexto => new Contexto(connection));

            string sql = @"
                CREATE TABLE public.unidade_federativa (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    sigla TEXT,
                    descricao TEXT
                );

                INSERT INTO public.unidade_federativa(sigla, descricao)
                VALUES ('DF', 'Distrito Federal'), ('GO', 'Goiás');
            ";

            connection.Execute(sql);
        }

        [Fact]
        public void ObterDominio_QuandoHouverUFsCadastradas_DeveRetornarListaDeUFs()
        {
            var dominios = repositorio.ObterDominio();

            Assert.Equal("Distrito Federal", dominios.ElementAt(0).Descricao);
            Assert.Equal("DF", dominios.ElementAt(0).Sigla);

            Assert.Equal("Goiás", dominios.ElementAt(1).Descricao);
            Assert.Equal("GO", dominios.ElementAt(1).Sigla);

            Assert.Equal(2, dominios.Count());
        }
        [Fact]
        public void ObterUnidadeFederativa_QuandoNaoHouverUFsCadastradas_DeveRetornarListaVazia()
        {
            string sql = "DELETE FROM public.unidade_federativa";
            connection.Execute(sql);

            var dominios = repositorio.ObterDominio();

            Assert.Empty(dominios);
        }
    }
}
