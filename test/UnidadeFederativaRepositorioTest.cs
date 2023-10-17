using app.Repositorios;
using app.Repositorios.Interfaces;
using System.Linq;
using test.Fixtures;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using System.Collections.Generic;

namespace test
{
    public class UnidadeFederativaRepositorioTest : TestBed<Base>, IDisposable
    {
        IUnidadeFederativaRepositorio repositorio;

        public UnidadeFederativaRepositorioTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {            
            repositorio = fixture.GetService<IUnidadeFederativaRepositorio>(testOutputHelper)!;
        }

        [Fact]
        public void ObterDominio_QuandoHouverUFsCadastradas_DeveRetornarListaDeUFs()
        {
            var dominios = repositorio.ObterDominio().ToList();

            Assert.Equal("Acre", dominios.ElementAt(0).Nome);
            Assert.Equal("AC", dominios.ElementAt(0).Sigla);

            Assert.Equal("Alagoas", dominios.ElementAt(1).Nome);
            Assert.Equal("AL", dominios.ElementAt(1).Sigla);            

            Assert.Equal("Amazonas", dominios.ElementAt(2).Nome);
            Assert.Equal("AM", dominios.ElementAt(2).Sigla);

            Assert.Equal("Amapá", dominios.ElementAt(3).Nome);
            Assert.Equal("AP", dominios.ElementAt(3).Sigla);

            Assert.Equal("Bahia", dominios.ElementAt(4).Nome);
            Assert.Equal("BA", dominios.ElementAt(4).Sigla);

            Assert.Equal("Ceará", dominios.ElementAt(5).Nome);
            Assert.Equal("CE", dominios.ElementAt(5).Sigla);

            Assert.Equal("Distrito Federal", dominios.ElementAt(6).Nome);
            Assert.Equal("DF", dominios.ElementAt(6).Sigla);

            Assert.Equal(27, dominios.Count());
        }
    }
}
