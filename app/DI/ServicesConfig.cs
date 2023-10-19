using app.Entidades;
using app.Services;
using Microsoft.EntityFrameworkCore;
using app.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Runtime.CompilerServices;

namespace app.DI
{
    public static class ServicesConfig
    {
        public static void AddConfigServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSql")));
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<AutenticacaoService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddAppAuthorization(configuration);
        }

        public static void AddAppAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var configuracaoAutenticaco = configuration.GetSection("Autenticacao");
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuracaoAutenticaco["Issuer"],
                    ValidAudience = configuracaoAutenticaco["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuracaoAutenticaco["Key"]!)),
                    ValidateIssuer = bool.Parse(configuracaoAutenticaco["ValidateIssuer"]!),
                    ValidateAudience = bool.Parse(configuracaoAutenticaco["ValidateAudience"]!),
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = bool.Parse(configuracaoAutenticaco["ValidateIssuerSigningKey"]!)
                };
            });

            services.AddAuthorization();
        }
    }
}
