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

        [HttpPost("cadastrar")]
        public IActionResult Cadastrar([FromBody] UsuarioDTO usuarioDTO)
        {
            usuarioService.Cadastrar(usuarioDTO);

            return Ok();
        }

        [HttpPut("Recuperar Senha")]
        public IActionResult RecuperarSenha([FromBody] UsuarioDTO usuarioDto)
        {
            try
            {
                usuarioService.RecuperarSenha(usuarioDto);
           //     usuarioService.TrocaSenha(usuarioDto);
                return Ok();
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
        }

        //  [HttpPut("Validar token de recuperação")]
        // public IActionResult ValidarTokenDeRecuperacao([FromBody] RedefinicaoSenhaDTO redefinirSenhaDto)
        // {
        //     usuarioService.ValidaRedefinicaoDeSenha(redefinirSenhaDto);
        // }

        [HttpPut("Redefinir Senha")]
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
