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

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CadastrarEmpresa([FromBody] EmpresaDTO empresaDTO)
        {
            authService.Require(Usuario, Permissao.EmpresaCadastrar);

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

        [Authorize]
        [HttpGet("{cnpj}")]
        public IActionResult VisualizarEmpresa(string cnpj)
        {
            authService.Require(Usuario, Permissao.EmpresaVisualizar);

            var empresa = empresaService.VisualizarEmpresa(cnpj);

            if (empresa != null)
            {
                return Ok(empresa);
            }

            return StatusCode(404, "A empresa não existe.");
        }

        [Authorize]
        [HttpDelete("{cnpj}")]
        public async Task<IActionResult> ExcluirEmpresa(string cnpj)
        {
            authService.Require(Usuario, Permissao.EmpresaRemover);

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
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ListarEmpresas(int pageIndex, int pageSize, string? nome = null)
        {
            authService.Require(Usuario, Permissao.EmpresaVisualizar);
            try
            {
                var pagina = await empresaService.ListarEmpresas(pageIndex, pageSize, nome);

                List<Empresa> paginaRetorno = pagina.Select(p => mapper.Map<Empresa>(p)).ToList();

                return Ok(paginaRetorno);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message + "\n" + e.StackTrace + "\nHouve um erro interno no servidor.");
            }
        }
        
        [Authorize]
        [HttpPut("{cnpj}")]
        public async Task<IActionResult> EditarEmpresa(string cnpj, [FromBody] EmpresaDTO empresaDTO)
        {
            authService.Require(Usuario, Permissao.EmpresaEditar);

            var empresa = mapper.Map<Empresa>(empresaDTO);
            empresa.Cnpj = cnpj;

            try
            {
                var novaEmpresa = await empresaService.EditarEmpresa(cnpj, empresa);
                return Ok(novaEmpresa);
            }
            catch (DbUpdateException)
            {
                return UnprocessableEntity("Erro ao editar a empresa.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message + "\n" + e.StackTrace + "\nHouve um erro interno no servidor.");
            }
        }

        [Authorize]
        [HttpPut("adicionarUsuario")]
        public async Task<IActionResult> AdicionarUsuario(string cnpj, int usuarioid)
        {
            authService.Require(Usuario, Permissao.EmpresaGerenciarUsuarios);

            try
            {
                await empresaService.AdicionarUsuario(usuarioid, cnpj);
                return Ok("Usuário adicionado");
            }
            catch (DbUpdateException)
            {
                return UnprocessableEntity("Erro ao adicionar usuário.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message + "\n" + e.StackTrace + "\nHouve um erro interno no servidor.");
            }
        }

        [Authorize]
        [HttpDelete("removerUsuario")]
        public async Task<IActionResult> RemoverUsuario(string cnpj, int usuarioid)
        {
            authService.Require(Usuario, Permissao.EmpresaGerenciarUsuarios);

            try
            {
                await empresaService.RemoverUsuario(usuarioid, cnpj);
                return Ok("Usuário removido");
            }
            catch (DbUpdateException)
            {
                return UnprocessableEntity("Erro ao remover usuário.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message + "\n" + e.StackTrace + "\nHouve um erro interno no servidor.");
            }
        }

        [Authorize]
        [HttpGet("listarUsuarios/{cnpj}")]
        public async Task<IActionResult> ListarUsuarios(string cnpj, int pageIndex, int pageSize, string? nome = null)
        {
            authService.Require(Usuario, Permissao.EmpresaVisualizarUsuarios);

            try
            {
                var pagina = await empresaService.ListarUsuarios(cnpj, pageIndex, pageSize, nome);

                return Ok(pagina);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message + "\n" + e.StackTrace + "\nHouve um erro interno no servidor.");
            }
        }
    }
}