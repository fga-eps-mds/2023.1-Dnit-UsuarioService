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

        public PerfilController(IPerfilService perfilService, AuthService authService, IMapper mapper )
        {
            this.perfilService = perfilService;
            this.authService = authService;
            this.mapper = mapper;
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

            Perfil perfil = mapper.Map<Perfil>(perfilDTO);
            perfil.Id = id;

            try{
                Perfil novoPerfil = await perfilService.EditarPerfil(perfil, perfilDTO.Permissoes);
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
            catch(Exception)
            {
                return StatusCode(500, "Houve um erro interno no servidor.");
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
            catch(KeyNotFoundException){
                return NotFound("Perfil não encontrado");
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ListarPerfis(int pageIndex, int pageSize)
        {
            authService.Require(Usuario, Permissao.PerfilVisualizar);

            try
            {
                var pagina = await perfilService.ListarPerfisAsync(pageIndex, pageSize);

                List<PerfilModel> paginaRetorno = pagina.Select(p => mapper.Map<PerfilModel>(p)).ToList();

                return Ok(paginaRetorno);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message + "\n" + e.StackTrace + "\nHouve um erro interno no servidor.");
            }
        }
    }
}