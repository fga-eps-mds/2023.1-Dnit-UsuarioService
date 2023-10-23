using app.Entidades;
using app.Services;
using app.Services.Interfaces;
using auth;
using Microsoft.EntityFrameworkCore;

namespace app.DI
{
    public static class ServicesConfig
    {
        public static void AddConfigServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSql")));
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<AuthService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddAuth(configuration);
        }

    }
}
