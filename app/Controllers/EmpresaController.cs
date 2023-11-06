using app.Entidades;
using app.Services;
using app.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api;
using api.Perfis;
using api.Empresa;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace app.Controller
{
    [ApiController]
    [Route("api/empresa")]
    public class EmpresaController : AppController
    {
        private readonly AuthService authService;
        private readonly IEmpresaService empresaService;
        private readonly IMapper mapper;
        private readonly IPermissaoService permissaoService;

        public EmpresaController(
            IEmpresaService empresaService,
            AuthService authService,
            IMapper mapper,
            IPermissaoService permissaoService
        )
        {
            this.empresaService = empresaService;
            this.authService = authService;
            this.mapper = mapper;
            this.permissaoService = permissaoService;
        }

        //[Authorize]
        [HttpPost()]
        public async Task<IActionResult> CadastrarEmpresa([FromBody] EmpresaDTO empresaDTO)
        {
            //authService.Require(Usuario, Permissao.EmpresaCadastrar);

            var empresa = mapper.Map<Empresa>(empresaDTO);

            try{
                await empresaService.CadastrarEmpresa(empresa);
                return Ok();
            }
            catch(DbUpdateException)
            {
                return UnprocessableEntity("Esta Empresa já existe");
            }
            catch(Exception)
            {
                return StatusCode(500, "Houve um erro interno no servidor.");
            }
        }

        [HttpGet("{cnpj}")]
        public IActionResult VisualizarEmpresa(string cnpj)
        {
            var empresa = empresaService.VisualizarEmpresa(cnpj);

            if (empresa != null)
            {
                return Ok(empresa);
            }

            return StatusCode(404, "A empresa não existe.");
        }

        [HttpDelete("{cnpj}")]
        public async Task<IActionResult> ExcluirEmpresa(string cnpj)
        {
            authService.Require(Usuario, Permissao.PerfilRemover);

            try{
                await empresaService.DeletarEmpresa(cnpj);
                return Ok("Empresa excluida");
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
        
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> ListarEmpresas(int pageIndex, int pageSize, string? nome = null)
        {

            try
            {
                var pagina = await empresaService.ListarEmpresas(pageIndex, pageSize);

                List<Empresa> paginaRetorno = pagina.Select(p => mapper.Map<Empresa>(p)).ToList();

                return Ok(paginaRetorno);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message + "\n" + e.StackTrace + "\nHouve um erro interno no servidor.");
            }
        }
        
        // [Authorize]
        [HttpPut("{cnpj}")]
        public async Task<IActionResult> EditarEmpresa(string cnpj, [FromBody] EmpresaDTO empresaDTO)
        {

            var empresa = mapper.Map<Empresa>(empresaDTO);
            empresa.Cnpj = cnpj;

            try
            {
                var novaEmpresa = await empresaService.EditarEmpresa(cnpj, empresa);
                return Ok(novaEmpresa);
            }
            catch (DbUpdateException)
            {
                return UnprocessableEntity("Este Perfil já existe");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message + "\n" + e.StackTrace + "\nHouve um erro interno no servidor.");
            }
        }
        
    }
}