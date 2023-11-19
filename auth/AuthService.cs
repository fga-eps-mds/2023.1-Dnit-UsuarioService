using auth;
using EnumsNET;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace app.Services
{
    public class AuthService
    {
        private readonly AuthConfig authConfig;
        private const string CLAIM_PERMISSIONS = "permissions";
        private const string CLAIM_ID = "id";
        private const string CLAIM_ADMIN = "admin";
        private const string CLAIM_APIKEY = "apiKey";
        private const char PERMISSIONS_SEPARATOR = ',';

        public AuthService(IOptions<AuthConfig> authConfig)
        {
            this.authConfig = authConfig.Value;
        }

        public void Require<TPermission>(ClaimsPrincipal user, TPermission permission) where TPermission : struct
        {
            if (!authConfig.Enabled)
                return;
            
            if (!HasPermission(user, permission))
                throw new AuthForbiddenException($"O usuário não tem a permissão: {Enums.AsStringUnsafe(permission, EnumFormat.Description)} ({permission})");
        }

        public bool HasPermission<TPermission>(ClaimsPrincipal user, TPermission permission) where TPermission : struct
        {
            var isAdmin = user.Claims.FirstOrDefault(c => c.Type == CLAIM_ADMIN)?.Value;
            if (!string.IsNullOrWhiteSpace(isAdmin) && bool.Parse(isAdmin)) return true;

            var permissionsText = user.Claims.FirstOrDefault(c => c.Type == CLAIM_PERMISSIONS)?.Value;
            return DecodePermissions<TPermission>(permissionsText)?.Any(p => permission.Equals(p)) ?? false;
        }

        public int GetUserId(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var id = tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(c => c.Type == CLAIM_ID);
            if (id != null)
            {
                return int.Parse(id.Value);
            }
            throw new AuthForbiddenException("Token inválido");
        }

        public int GetUserId(ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.First(c => c.Type == CLAIM_ID).Value);
        }

        public (string Token, DateTime ExpiresAt) GenerateToken<TPermission>(AuthUserModel<TPermission> user, bool apiKey = false) where TPermission : struct
        {
            var issuer = authConfig.Issuer;
            var audience = authConfig.Audience;
            var key = Encoding.ASCII.GetBytes(authConfig.Key);
            var expiraEm = DateTime.UtcNow.AddMinutes(!apiKey ? authConfig.ExpireMinutes : authConfig.ApiKeyExpireMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(CLAIM_ID, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(CLAIM_PERMISSIONS, EncodePermissions(user.Permissions)),
                    new Claim(CLAIM_ADMIN, user.Administrador.ToString()),
                    new Claim(CLAIM_APIKEY, apiKey.ToString())
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

        public (string RefreshToken, DateTime ExpiresAt) GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return (Convert.ToBase64String(randomNumber), DateTime.UtcNow.AddMinutes(authConfig.RefreshTokenExpireMinutes));
        }

        private string EncodePermissions<TPermission>(List<TPermission>? permissions) where TPermission : struct
        {
            var hasPermissions = permissions?.Any() ?? false;
            return hasPermissions ? string.Join(PERMISSIONS_SEPARATOR, permissions!.Select(p => ToLong(p))) : "";
        }

        private IEnumerable<TPermission?>? DecodePermissions<TPermission>(string? permissionsText) where TPermission : struct
        {
            var permissionValues = ((TPermission[])Enum.GetValues(typeof(TPermission))).ToDictionary(p => ToLong(p));
            
            var permissions = permissionsText?.Split(PERMISSIONS_SEPARATOR);
#pragma warning disable S2589 // Boolean expressions should not be gratuitous
            return permissions?
                .Where(p => !string.IsNullOrWhiteSpace(p))?
                .Select(long.Parse)?
                .Where(p => p > 0)?
                .Select(p => {
                    TPermission result;
                    return permissionValues.TryGetValue(p, out result) ? result : (TPermission?)null;
                })?
                .Where(p => p.HasValue)?
                .Select(p => (TPermission?)Convert.ChangeType(p, typeof(TPermission)))?
                .Where(p => p != null);
#pragma warning restore S2589 // Boolean expressions should not be gratuitous
        }

        private long ToLong<TPermission>(TPermission p) where TPermission : struct {
            return (long)Convert.ChangeType(p, typeof(long));
        }
    }
}
