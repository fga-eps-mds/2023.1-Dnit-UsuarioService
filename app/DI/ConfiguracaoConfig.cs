using app.Repositorios.Interfaces;
using app.Repositorios;
using app.Configuracoes;

namespace app.DI
{
    public static class ConfiguracaoConfig
    {
        public static void AddConfiguracoes(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SenhaConfig>(configuration.GetSection("Senha"));
        }
    }
}
