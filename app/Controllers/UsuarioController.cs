using AutoMapper;
using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;
using System.Diagnostics;
using System.Web;

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
        public IActionResult Obter([FromBody] UsuarioDTO usuarioDTO)
        {
            try{
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
    }
}
