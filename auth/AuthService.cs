using auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace app.Services
{
    public class AuthService
    {
        private readonly AuthConfig authConfig;

        public AuthService(IOptions<AuthConfig> authConfig)
        {
            this.authConfig = authConfig.Value;
        }

        public (string Token, DateTime ExpiresAt) GenerateToken<TEnum>(AuthUserModel<TEnum> user) where TEnum : struct
        {
            var issuer = authConfig.Issuer;
            var audience = authConfig.Audience;
            var key = Encoding.ASCII.GetBytes(authConfig.Key);
            var expiraEm = DateTime.UtcNow.AddMinutes(authConfig.ExpireMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("permissions", EncodePermissions(user.Permissions))
                }),
                Expires = expiraEm,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return (stringToken, expiraEm);
        }

        private string EncodePermissions<TEnum>(List<TEnum>? permissions) where TEnum : struct
        {
            var hasPermissions = permissions?.Any() ?? false;
            return hasPermissions ? string.Join(',', permissions!.Select(p => p.ToLong())) : "";
        }
    }
}
