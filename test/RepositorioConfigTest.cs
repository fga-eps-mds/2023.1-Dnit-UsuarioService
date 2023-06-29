using app.DI;
using Microsoft.Extensions.DependencyInjection;
using repositorio.Interfaces;
using repositorio;
using Xunit;

namespace test
{
    public class RepositorioConfigTest
    {
        public class RepositoriosConfigTests
        {
            [Fact]
            public void AddConfigRepositorios_QuandoMetodoForChamado_DeveConfigurarInjecaoDeDependencia()
            {
                var services = new ServiceCollection();

                RepositoriosConfig.AddConfigRepositorios(services);

                Assert.Contains(services, descriptor =>
                    descriptor.ServiceType == typeof(IUnidadeFederativaRepositorio) &&
                    descriptor.ImplementationType == typeof(UnidadeFederativaRepositorio) &&
                    descriptor.Lifetime == ServiceLifetime.Scoped);

                Assert.Contains(services, descriptor =>
                    descriptor.ServiceType == typeof(IUsuarioRepositorio) &&
                    descriptor.ImplementationType == typeof(UsuarioRepositorio) &&
                    descriptor.Lifetime == ServiceLifetime.Scoped);
            }
        }
    }
}
