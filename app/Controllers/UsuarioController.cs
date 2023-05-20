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

        [HttpPost("usuarioDnit")]
        public IActionResult Cadastrar([FromBody] UsuarioDNIT usuarioDNIT)
        {
            usuarioService.Cadastrar(usuarioDNIT);

            return Ok();
            //return item;
        }
    }
}