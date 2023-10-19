using app.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace auth
{
    public static class AuthStartup
    {
        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthConfig>(configuration.GetSection("Auth"));

            services.AddSingleton<AuthService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var configuracaoAutenticaco = configuration.GetSection("Auth");
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuracaoAutenticaco["Issuer"],
                    ValidAudience = configuracaoAutenticaco["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracaoAutenticaco["Key"]!)),
                    ValidateIssuer = bool.Parse(configuracaoAutenticaco["ValidateIssuer"] ?? "false"),
                    ValidateAudience = bool.Parse(configuracaoAutenticaco["ValidateAudience"] ?? "false"),
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = bool.Parse(configuracaoAutenticaco["ValidateIssuerSigningKey"] ?? "false")
                };
            });

            services.AddAuthorization();
        }
    }
}
