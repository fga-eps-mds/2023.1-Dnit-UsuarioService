using app.Entidades;
using app.Services;
using Microsoft.EntityFrameworkCore;
using app.Services.Interfaces;

namespace app.DI
{
    public static class ServicesConfig
    {
        public static void AddConfigServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSql")));
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
