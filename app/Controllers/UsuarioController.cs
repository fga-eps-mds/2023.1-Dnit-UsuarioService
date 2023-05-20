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
        
        [HttpPost]
        public IActionResult Obter([FromBody] UsuarioDTO usuarioDTO)
        {
            bool verificar = usuarioService.validaLogin(usuarioDTO);
        
            if (verificar == true) return Ok();
            else
            {
                return Unauthorized();
            }

        }
    }
}
