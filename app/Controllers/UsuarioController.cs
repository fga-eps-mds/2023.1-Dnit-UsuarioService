using api.Usuarios;
using api.Senhas;
using Microsoft.AspNetCore.Mvc;
using app.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace app.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;
        private readonly IConfiguration configuration;

        public UsuarioController(
            IUsuarioService usuarioService,
            IConfiguration configuration
        )
        {
            this.usuarioService = usuarioService;
            this.configuration = configuration;
        }

        [HttpGet("login/teste")]
        public IResult LoginTeste(string username, string password)
        {
            if (username != "joydip" || password != "joydip123")
            {
                return Results.Unauthorized();
            }

            var configuracaoAutenticaco = configuration.GetSection("Autenticacao");

            var issuer = configuracaoAutenticaco["Issuer"];
            var audience = configuracaoAutenticaco["Audience"];
            var key = Encoding.ASCII.GetBytes(configuracaoAutenticaco["Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Email, "email@gmail.com"),
                    new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(configuracaoAutenticaco["ExpireMinutes"]!)),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return Results.Ok(stringToken);
        }

        [HttpGet("autenticacao/teste")]
        [Authorize]
        public int TesteRotaAutenticada()
        {
            return 42;
        }

        [HttpPost("login")]
        public IActionResult Logar([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                bool verificar = usuarioService.ValidaLogin(usuarioDTO);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("cadastrarUsuarioDnit")]
        public async Task<IActionResult> CadastrarUsuarioDnit([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                await usuarioService.CadastrarUsuarioDnit(usuarioDTO);

                return StatusCode(201, new NoContentResult());
            }
            catch (Npgsql.PostgresException ex)
            {
                if (ex.SqlState == "23505")
                {
                    return Conflict("Usu�rio j� cadastrado.");
                }

                return StatusCode(500, "Houve um erro interno no servidor.");
            }
        }

        [HttpPost("cadastrarUsuarioTerceiro")]
        public IActionResult CadastrarUsuarioTerceiro([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                usuarioService.CadastrarUsuarioTerceiro(usuarioDTO);

                return StatusCode(201, new NoContentResult());
            }
            catch (Npgsql.PostgresException ex)
            {
                if (ex.SqlState == "23505")
                {
                    return Conflict("Usu�rio j� cadastrado.");
                }

                return StatusCode(500, "Houve um erro interno no servidor.");
            }
        }

        [HttpPut("recuperarSenha")]
        public async Task<IActionResult> RecuperarSenhaAsync([FromBody] UsuarioDTO usuarioDto)
        {
            try
            {
                await usuarioService.RecuperarSenha(usuarioDto);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("redefinirSenha")]
        public async Task<IActionResult> RedefinirSenhaAsync([FromBody] RedefinicaoSenhaDTO redefinirSenhaDto)
        {
            try
            {
                await usuarioService.TrocaSenha(redefinirSenhaDto);

                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
