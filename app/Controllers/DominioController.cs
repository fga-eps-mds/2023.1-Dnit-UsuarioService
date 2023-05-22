using dominio;
using Microsoft.AspNetCore.Mvc;
using repositorio;
using repositorio.Interfaces;
using service;
using service.Interfaces;

namespace app.Controllers
{
    [ApiController]
    [Route("api/dominio")]
    public class DominioController : ControllerBase
    {
        private readonly IUnidadeFederativaRepositorio unidadeFederativaRepositorio;

        public DominioController(IUnidadeFederativaRepositorio unidadeFederativaRepositorio)
        {
            this.unidadeFederativaRepositorio = unidadeFederativaRepositorio;
        }



        [HttpGet("unidadeFederativa")]
        public IActionResult ObterLista()
        {
            IEnumerable<UnidadeFederativa> listaUnidadeFederativa = unidadeFederativaRepositorio.ObterDominio();

            return  new OkObjectResult(listaUnidadeFederativa);
        }


    }
}
