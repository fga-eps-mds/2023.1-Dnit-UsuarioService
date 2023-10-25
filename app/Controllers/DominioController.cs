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
        private readonly IPermissaoService permissaoService;
        private readonly IMapper mapper;
        private readonly AuthService authService;


        public DominioController
        (
            IUnidadeFederativaRepositorio unidadeFederativaRepositorio, 
            IMapper mapper, 
            IPermissaoService permissaoService,
            AuthService authService
        )
        {
            this.unidadeFederativaRepositorio = unidadeFederativaRepositorio;
            this.permissaoService = permissaoService;
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
        public List<CategoriaPermissaoModel> ObterListaDePermissoes()
        {
            authService.Require(Usuario, Permissao.PerfilVisualizar);
            
            return permissaoService.CategorizarPermissoes(Enum.GetValues<Permissao>().ToList());
        }
    }
}
