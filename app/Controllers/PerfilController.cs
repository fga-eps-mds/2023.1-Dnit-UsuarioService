using api.Perfis;
using app.Entidades;
using app.Services;
using app.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app.Controllers
{
    [ApiController]
    [Route("api/perfil")]
    public class PerfilController : ControllerBase
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

        [HttpPost()]
        public IActionResult CriarPerfil([FromBody] PerfilDTO perfilDTO)
        {
            //verificar se o user tem permissão

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

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarPerfil(Guid id, [FromBody] PerfilDTO perfilDTO)
        {
            //verificar se o user tem permissão
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirPerfil(Guid id)
        {
            //verificar se o user tem permissao
            try{
                await perfilService.ExcluirPerfil(id);
                return Ok("Perfil excluido");
            }
            catch(KeyNotFoundException){
                return NotFound("Perfil não encontrado");
            }
            catch(Exception)
            {
                return StatusCode(500, "Houve um erro interno no servidor.");
            }            
        }

        [HttpGet()]
        public async Task<IActionResult> ListarPerfis(int pageIndex, int pageSize)
        {
            try
            {
                var pagina = await perfilService.ListarPerfisAsync(pageIndex, pageSize);

                List<PerfilModel> paginaRetorno = pagina.Select(p => mapper.Map<PerfilModel>(p)).ToList();

                return Ok(paginaRetorno);
            }
            catch(Exception)
            {
                return StatusCode(500, "Houve um erro interno no servidor.");
            }
        }
    }
}