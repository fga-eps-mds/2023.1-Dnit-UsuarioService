using app.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using app.Services;
using app.Services.Interfaces;
using Xunit;

namespace test
{
    public class ServicesConfigTest
    {
        [Fact]
        public void AddConfigServices_QuandoMetodoForChamado_DeveConfigurarInjecaoDeDependencia()
        {
            var services = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder().Build();

            ServicesConfig.AddConfigServices(services, configurationBuilder);

            Assert.Contains(services, descriptor =>
                descriptor.ServiceType == typeof(IUsuarioService) &&
                descriptor.ImplementationType == typeof(UsuarioService) &&
                descriptor.Lifetime == ServiceLifetime.Scoped);

            Assert.Contains(services, descriptor =>
                descriptor.ServiceType == typeof(IEmailService) &&
                descriptor.ImplementationType == typeof(EmailService) &&
                descriptor.Lifetime == ServiceLifetime.Scoped);
        }
    }
}
