using app.Entidades;
using api;
using Microsoft.AspNetCore.Mvc;
using app.Repositorios;
using app.Repositorios.Interfaces;
using app.Services;
using app.Services.Interfaces;
using AutoMapper;

namespace app.Controllers
{
    [ApiController]
    [Route("api/dominio")]
    public class DominioController : ControllerBase
    {
        private readonly IUnidadeFederativaRepositorio unidadeFederativaRepositorio;

        private readonly IMapper mapper;

        public DominioController(IUnidadeFederativaRepositorio unidadeFederativaRepositorio, IMapper mapper)
        {
            this.unidadeFederativaRepositorio = unidadeFederativaRepositorio;
            this.mapper = mapper;
        }

        [HttpGet("unidadeFederativa")]
        public IActionResult ObterLista()
        {
            IEnumerable<UfModel> listaUnidadeFederativa = unidadeFederativaRepositorio.ObterDominio();

            return  new OkObjectResult(listaUnidadeFederativa);
        }

        [HttpGet("permissoes")]
        public IActionResult ObterListaDePermissoes(int pageIndex, int pageSize)
        {
            var lista = Enum.GetValues<Permissao>()
                .Select(p => mapper.Map<PermissaoModel>(p))
                .OrderBy(p => p.Categoria)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
                
            return Ok(lista);
        }
        


    }
}
