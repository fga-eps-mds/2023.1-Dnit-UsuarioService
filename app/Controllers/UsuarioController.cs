using api.Usuarios;
using api.Senhas;
using Microsoft.AspNetCore.Mvc;
using app.Services.Interfaces;

namespace app.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }
        
        [HttpPost("login")]
        public IActionResult Logar([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                bool verificar = usuarioService.ValidaLogin(usuarioDTO);
                return Ok();
            }
            catch(UnauthorizedAccessException){
                return Unauthorized();
            }
            catch(KeyNotFoundException){
                return NotFound();
            }
        }

        [HttpPost("cadastrarUsuarioDnit")]
        public IActionResult CadastrarUsuarioDnit([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                usuarioService.CadastrarUsuarioDnit(usuarioDTO);

                return StatusCode(201, new NoContentResult());
            }
            catch (Npgsql.PostgresException ex)
            {
                if (ex.SqlState == "23505") {
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
        public IActionResult RecuperarSenha([FromBody] UsuarioDTO usuarioDto)
        {
            try
            {
                usuarioService.RecuperarSenha(usuarioDto);
                return Ok();
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("redefinirSenha")]
        public IActionResult RedefinirSenha([FromBody] RedefinicaoSenhaDTO redefinirSenhaDto)
        {
            try
            {
                usuarioService.TrocaSenha(redefinirSenhaDto);
                return Ok();
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
