using app.Repositorios;
using app.Repositorios.Interfaces;

namespace app.DI
{
    public static class RepositoriosConfig
    {
        public static void AddConfigRepositorios(this IServiceCollection services)
        {
            services.AddScoped<IUnidadeFederativaRepositorio, UnidadeFederativaRepositorio>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<IPerfilRepositorio, PerfilRepositorio>();
            services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();
        }
    }
}
