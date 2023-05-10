using dominio.Enums;
using repositorio.Contexto;
using static repositorio.Contexto.ResolverContexto;

namespace app.DI
{
    public static class ContextoConfig
    {
        public static void AddContexto(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionPostgres = ObterConnectionString(configuration, ContextoBancoDeDados.Postgresql).Result;

            services.AddScoped(contexto => new ContextoPostgresql(connectionPostgres));

            services.AddTransient<ResolverContextoDelegate>(serviceProvider => contextos =>
            {
                return contextos switch
                {
                    ContextoBancoDeDados.Postgresql => serviceProvider.GetService<ContextoPostgresql>(),
                    _ => throw new NotImplementedException()
                };
            });
        }

        private static async Task<string> ObterConnectionString(IConfiguration configuration, ContextoBancoDeDados contexto)
        {
            string conn = contexto switch
            {
                ContextoBancoDeDados.Postgresql => "Postgresql",
                _ => throw new NotImplementedException(),
            };

            string connection = configuration.GetConnectionString(conn);

            return connection;
        }
    }
}
