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
            var mode = Environment.GetEnvironmentVariable("MODE");
            var connectionString = mode == "container" ? "PostgreSqlDocker" : "PostgreSql";

            services.AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(configuration.GetConnectionString(connectionString)));
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPerfilService, PerfilService>();
            services.AddScoped<IPermissaoService, PermissaoService>();

            services.AddControllers(o => o.Filters.Add(typeof(HandleExceptionFilter)));

            services.AddAuth(configuration);
        }
    }
}
