using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;

namespace app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }

        [HttpGet("item")]
        public Item Obter([FromQuery] int id)
        {
            Item item = service.Obter(id);

            bool verificar = usuarioService.validaLogin();
        
            if (verificar == true) return Ok();
            else
            {
                return Unauthorized();
            }

        }
    }
}
