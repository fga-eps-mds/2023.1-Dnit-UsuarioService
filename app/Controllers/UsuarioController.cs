using AutoMapper;
using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;
using System.Diagnostics;
using System.Web;
using repositorio.Interfaces;

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
            usuarioService.CadastrarUsuarioDnit(usuarioDTO);

            return Ok();
        }

        [HttpPost("cadastrarUsuarioTerceiro")]
        public IActionResult CadastrarUsuarioTerceiro([FromBody] UsuarioDTO usuarioDTO)
        {
            usuarioService.CadastrarUsuarioTerceiro(usuarioDTO);

            return Ok();
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
