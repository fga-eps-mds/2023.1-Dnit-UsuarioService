using api;
using app.Entidades;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace app.Services
{
    public class AutenticacaoService
    {
        private readonly IConfiguration configuration;

        public AutenticacaoService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public (string Token, DateTime ExpiraEm) GerarToken(Usuario usuario)
        {
            var configuracaoAutenticaco = configuration.GetSection("Autenticacao");

            var issuer = configuracaoAutenticaco["Issuer"];
            var audience = configuracaoAutenticaco["Audience"];
            var key = Encoding.ASCII.GetBytes(configuracaoAutenticaco["Key"]!);
            var expiraEm = DateTime.UtcNow.AddMinutes(int.Parse(configuracaoAutenticaco["ExpireMinutes"]!));
            var temPermissoes = usuario?.Perfil?.Permissoes?.Any() ?? false;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", usuario.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Nome),
                    new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("permissions", temPermissoes ? string.Join(',', usuario.Perfil!.Permissoes!.Select(p => p.ToString())) : "")
                }),
                Expires = expiraEm,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return (stringToken, expiraEm);
        }
    }
}
