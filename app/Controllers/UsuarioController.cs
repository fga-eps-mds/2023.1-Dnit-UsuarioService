using api.Usuarios;
using api.Senhas;
using Microsoft.AspNetCore.Mvc;
using app.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace app.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;

        public UsuarioController(
            IUsuarioService usuarioService
        )
        {
            this.usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Logar([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                var resultado = await usuarioService.AutenticarUsuarioAsync(usuarioDTO.Email, usuarioDTO.Senha);
                return Ok(resultado);
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
            catch (DbUpdateException)
            {
                return Conflict("Usuário já cadastrado.");
            }
            catch (Exception)
            {
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
            catch (DbUpdateException)
            {
                return Conflict("Usuário já cadastrado.");
            }
            catch (Exception)
            {
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
