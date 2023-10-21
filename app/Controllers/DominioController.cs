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

        private readonly IPermissaoRepositorio permissaoRepositorio;

        private readonly IMapper mapper;

        public DominioController(IUnidadeFederativaRepositorio unidadeFederativaRepositorio, IMapper mapper, IPermissaoRepositorio permissaoRepositorio)
        {
            this.unidadeFederativaRepositorio = unidadeFederativaRepositorio;
            this.permissaoRepositorio = permissaoRepositorio;
            this.mapper = mapper;
        }

        [HttpGet("unidadeFederativa")]
        public IActionResult ObterLista()
        {
            IEnumerable<UfModel> listaUnidadeFederativa = unidadeFederativaRepositorio.ObterDominio();

            return  new OkObjectResult(listaUnidadeFederativa);
        }

        [HttpGet("permissoes")]
        public IActionResult ObterListaDePermissoes()
        {
            var categorias = permissaoRepositorio.ObterCategorias();

            List<PermissaoModel> lista = new();
            foreach(var categoria in categorias)
            {
                PermissaoModel model = new ()
                {
                    Categoria = categoria,
                    Permisoes = permissaoRepositorio.ObterPermissoesPortCategoria(categoria)
                };
                lista.Add(model);
            }
            return Ok(lista);
            /* try
            {
                
            }
            catch (Exception)
            {
                return StatusCode(500, "Houve um erro interno no servidor.");
            } */            
        }
    }
}
