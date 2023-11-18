using api;
using api.Perfis;
using app.Entidades;
using app.Services;
using app.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app.Controllers
{
    [ApiController]
    [Route("api/perfil")]
    public class PerfilController : AppController
    {
        private readonly AuthService authService;
        private readonly IPerfilService perfilService;
        private readonly IMapper mapper;
        private readonly IPermissaoService permissaoService;

        public PerfilController(
            IPerfilService perfilService,
            AuthService authService,
            IMapper mapper,
            IPermissaoService permissaoService
        )
        {
            this.perfilService = perfilService;
            this.authService = authService;
            this.mapper = mapper;
            this.permissaoService = permissaoService;
        }

        [Authorize]
        [HttpPost()]
        public IActionResult CriarPerfil([FromBody] PerfilDTO perfilDTO)
        {
            authService.Require(Usuario, Permissao.PerfilCadastrar);

            var perfil = mapper.Map<Perfil>(perfilDTO);

            try{
                Perfil novoPerfil = perfilService.CriarPerfil(perfil, perfilDTO.Permissoes);
                return Ok(mapper.Map<PerfilModel>(novoPerfil));
            }
            catch(DbUpdateException)
            {
                return UnprocessableEntity("Este Perfil já existe");
            }
            catch(Exception)
            {
                return StatusCode(500, "Houve um erro interno no servidor.");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarPerfil(Guid id, [FromBody] PerfilDTO perfilDTO)
        {
            authService.Require(Usuario, Permissao.PerfilEditar);

            var perfil = mapper.Map<Perfil>(perfilDTO);
            perfil.Id = id;

            try{
                var novoPerfil = await perfilService.EditarPerfil(perfil, perfilDTO.Permissoes);
                return Ok(mapper.Map<PerfilModel>(novoPerfil));
            }
            catch(KeyNotFoundException)
            {
                return NotFound("Perfil não encontrado");
            }
            catch(DbUpdateException)
            {
                return UnprocessableEntity("Este Perfil já existe");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Houve um erro interno no servidor. {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirPerfil(Guid id)
        {
            authService.Require(Usuario, Permissao.PerfilRemover);

            try{
                await perfilService.ExcluirPerfil(id);
                return Ok("Perfil excluido");
            }
            catch(KeyNotFoundException ex){
                return NotFound(ex.Message);
            }
            catch(InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch(Exception)
            {
                return StatusCode(500, "Houve um erro interno no servidor.");
            }            
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListarPerfis(int pageIndex, int pageSize, string? nome = null)
        {
            authService.Require(Usuario, Permissao.PerfilVisualizar);

            try
            {
                var pagina = await perfilService.ListarPerfisAsync(pageIndex, pageSize, nome)!;
                pagina.ForEach(p => p.PermissoesSessao = p.Permissoes?.AsEnumerable().ToList(comInternas: false));
                List<PerfilModel> paginaRetorno = pagina.Select(p => mapper.Map<PerfilModel>(p)).ToList();

                return Ok(paginaRetorno);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message + "\n" + e.StackTrace + "\nHouve um erro interno no servidor.");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> ObterPorId(Guid id)
        {
            authService.Require(Usuario, Permissao.PerfilVisualizar);

            var perfil = await perfilService.ObterPorIdAsync(id);

            if (perfil == null)
            {
                return NotFound("Perfil não encontrado");
            }

            var perfilModel = mapper.Map<PerfilModel>(perfil);
            perfilModel.CategoriasPermissao = permissaoService.CategorizarPermissoes(perfil.Permissoes!.ToList(comInternas: false));

            return Ok(perfilModel);
        }
    }
}