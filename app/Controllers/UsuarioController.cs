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
        
        [HttpGet("usuario")]
        public IActionResult Obter([FromQuery] string email)
        { 
            
            UsuarioDnit usuario = usuarioService.Obter(email);
            Debug.WriteLine(usuario.email);

            bool verificar = usuarioService.validaLogin(usuario);
        
            if (verificar == true) return Ok(usuario);
            else
            {
                return Unauthorized();
            }

        }
    }
}
