using app.DI;
using dominio.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using repositorio.Contexto;
using System.Threading.Tasks;
using Xunit;

namespace test
{
    public class ContextoConfigTest
    {
        [Fact]
        public void AddContexto_QuandoMetodoForChamado_DeveRegistrarServicoCorretamente()
        {
            var services = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder().Build();

            services.AddContexto(configurationBuilder);

            var contexto = services.BuildServiceProvider().GetService<ContextoPostgresql>();

            Assert.NotNull(contexto);
        }

        [Fact]
        public async Task ObterConnectionString_QuandoMetodoForChamado_DeveRetornarStringDeConexao()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var contexto = ContextoBancoDeDados.Postgresql;

            var connectionString = await ObterConnectionString(configuration, contexto);

            Assert.NotNull(connectionString);
        }

        private async Task<string> ObterConnectionString(IConfiguration configuration, ContextoBancoDeDados contexto)
        {
            var metodo = typeof(ContextoConfig).GetMethod("ObterConnectionString", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            return await (Task<string>)metodo.Invoke(null, new object[] { configuration, contexto });
        }
    }
}
