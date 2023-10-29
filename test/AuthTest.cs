using api;
using app.Services;
using auth;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace test
{
    public class AuthenticationTest
    {

        AuthService authService;
        AuthConfig authConfig;

        public AuthenticationTest()
        {
            authConfig = new AuthConfig()
            {
                Enabled = true,
                Key = "teste secreta teste secreta teste secreta teste secreta teste secreta teste secreta",
                ExpireMinutes = 5,
                RefreshTokenExpireMinutes = 5,
            };

            authService = new AuthService(Options.Create(authConfig));
        }

        [Fact]
        public void GenerateRefreshToken_QuandoForValido_DeveTokenRetornarComAExpiracao()
        {
            var (refreshToken, expiracao) = authService.GenerateRefreshToken();

            Assert.NotEmpty(refreshToken);
            Assert.True(DateTime.UtcNow < expiracao);
        }

        [Fact]
        public void GenerateToken_QuandoForValido_DeveRetornarTokenComAExpiracao()
        {
            var (token, expiracao) = ObterTokenValido();

            Assert.NotEmpty(token);
            Assert.True(DateTime.UtcNow < expiracao);
        }

        [Fact]
        public void GetUserId_QuandoForValido_DeveRetornarId()
        {
            var id = 1;
            var (token, _) = ObterTokenValido(id);

            var usuarioId = authService.GetUserId(token);
            
            Assert.Equal(id, usuarioId);
        }

        [Fact]
        public void GetUserId_QuandoForClaim_DeveRetornarId()
        {
            var id = 1;
            var (token, _) = ObterTokenValido(id);
            var identity = ObterClaim(token);

            var usuarioId = authService.GetUserId(identity);

            Assert.Equal(id, usuarioId);
        }

        [Fact]
        public void GetUserId_QuandoForInvalido_DeveLancarExcecao()
        {
            var id = 1;
            var (token, _) = ObterTokenValido(id);
            token = Convert.ToBase64String(Encoding.ASCII.GetBytes('0' + token));
            Assert.Throws<ArgumentException>(() => authService.GetUserId(token));
        }

        [Fact]
        public void HasPermission_QuandoTiverPermissao_DeveRetornarTrue()
        {
            var (token, _) = ObterTokenValido(permissoes: new() { Permissao.RodoviaCadastrar });
            Assert.True(authService.HasPermission(ObterClaim(token), Permissao.RodoviaCadastrar));
        }

        [Fact]
        public void HasPermission_QuandoNaoTiverPermissao_DeveRetornarFalse()
        {
            var (token, _) = ObterTokenValido(permissoes: new() { Permissao.RodoviaCadastrar });
            Assert.False(authService.HasPermission(ObterClaim(token), Permissao.PerfilVisualizar));
        }

        [Fact]
        public void Require_QuandoTiverPermissao_DevePassar()
        {
            var (token, _) = ObterTokenValido(permissoes: new() { Permissao.RodoviaCadastrar });
            authService.Require(ObterClaim(token), Permissao.RodoviaCadastrar);
            Assert.True(true);
        }

        [Fact]
        public void Require_QuandoTiverDesabilitado_DevePassar()
        {
            authConfig.Enabled = false;
            var (token, _) = ObterTokenValido(permissoes: new() { Permissao.RodoviaCadastrar });
            authService.Require(ObterClaim(token), Permissao.PerfilVisualizar);
            Assert.True(true);
        }

        [Fact]
        public void Require_QuandoNaoTiverPermissao_DeveLancarExcecao()
        {
            var (token, _) = ObterTokenValido(permissoes: new() { Permissao.RodoviaCadastrar });
            Assert.Throws<AuthForbiddenException>(() => authService.Require(ObterClaim(token), Permissao.PerfilVisualizar));
        }

        private ClaimsPrincipal ObterClaim(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims));
        }

        private (string, DateTime) ObterTokenValido(int id = 1, List<Permissao>? permissoes = null)
        {
            var usuario = new AuthUserModel<Permissao>()
            {
                Id = id,
                Name = "Test",
                Permissions = permissoes ?? new() { Permissao.EscolaCadastrar },
            };
            return authService.GenerateToken(usuario);
        }
    }
}
