using api;
using Microsoft.AspNetCore.Mvc;
using app.Repositorios.Interfaces;
using app.Services.Interfaces;
using app.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using app.Services;

namespace app.Controllers
{
    [ApiController]
    [Route("api/dominio")]
    public class DominioController : AppController
    {
        private readonly IUnidadeFederativaRepositorio unidadeFederativaRepositorio;
        private readonly IPermissaoService PermissaoService;
        private readonly IMapper mapper;
        private readonly AuthService authService;


        public DominioController
        (
            IUnidadeFederativaRepositorio unidadeFederativaRepositorio, 
            IMapper mapper, 
            IPermissaoService PermissaoService,
            AuthService authService
        )
        {
            this.unidadeFederativaRepositorio = unidadeFederativaRepositorio;
            this.PermissaoService = PermissaoService;
            this.authService = authService;
            this.mapper = mapper;
        }

        [HttpGet("unidadeFederativa")]
        public IActionResult ObterLista()
        {
            IEnumerable<UfModel> listaUnidadeFederativa = unidadeFederativaRepositorio.ObterDominio();

            return  new OkObjectResult(listaUnidadeFederativa);
        }

        [Authorize]
        [HttpGet("permissoes")]
        public IActionResult ObterListaDePermissoes()
        {
            authService.Require(Usuario, Permissao.PerfilVisualizar);
            
            var categorias = PermissaoService.ObterCategorias();
            
            var lista = categorias.ConvertAll(c => new CategoriaPermissaoModel
            {
                Categoria = c,
                Permissoes = PermissaoService.ObterPermissoesPortCategoria(c)
            });
                        
            return Ok(lista);
        }
    }
}
