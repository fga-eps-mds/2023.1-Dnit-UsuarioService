using api;
using Microsoft.AspNetCore.Mvc;
using app.Repositorios.Interfaces;
using app.Services;

namespace app.Controllers
{
    [ApiController]
    [Route("api/dominio")]
    public class DominioController : AppController
    {
        private readonly IUnidadeFederativaRepositorio unidadeFederativaRepositorio;

        public DominioController(IUnidadeFederativaRepositorio unidadeFederativaRepositorio)
        {
            this.unidadeFederativaRepositorio = unidadeFederativaRepositorio;
        }



        [HttpGet("unidadeFederativa")]
        public IActionResult ObterLista()
        {
            IEnumerable<UfModel> listaUnidadeFederativa = unidadeFederativaRepositorio.ObterDominio();

            return  new OkObjectResult(listaUnidadeFederativa);
        }


    }
}
