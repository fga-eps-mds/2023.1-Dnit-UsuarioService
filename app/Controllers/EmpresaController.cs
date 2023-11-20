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
using api.Usuarios;

namespace app.Controller
{
    [ApiController]
    [Route("api/empresa")]
    public class EmpresaController : AppController
    {
        private readonly AuthService authService;
        private readonly IEmpresaService empresaService;
        private readonly IMapper mapper;

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
        }

        [Authorize]
        [HttpGet("{cnpj}")]
        public IActionResult VisualizarEmpresa(string cnpj)
        {
            authService.Require(Usuario, Permissao.EmpresaVisualizar);

            var empresa = empresaService.VisualizarEmpresa(cnpj);

            var result = mapper.Map<EmpresaModel>(empresa);
            return Ok(result);
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
            catch(InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }           
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ListarEmpresas(int pageIndex, int pageSize, string? nome = null, string? cnpj = null, string? ufs = "")
        {
            authService.Require(Usuario, Permissao.EmpresaVisualizar);
            
            var pagina = await empresaService.ListarEmpresas(pageIndex, pageSize, nome, cnpj, ufs);
            return Ok(pagina);
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
                var result = mapper.Map<EmpresaModel>(novaEmpresa);
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return UnprocessableEntity("Erro ao editar a empresa.");
            }

        }

        [Authorize]
        [HttpPut("adicionarUsuario")]
        public async Task<IActionResult> AdicionarUsuario(string cnpj, int usuarioid)
        {
            authService.Require(Usuario, Permissao.EmpresaGerenciarUsuarios);
            await empresaService.AdicionarUsuario(usuarioid, cnpj);
            return Ok("Usuário adicionado");
            
        }

        [Authorize]
        [HttpDelete("removerUsuario")]
        public async Task<IActionResult> RemoverUsuario(string cnpj, int usuarioid)
        {
            authService.Require(Usuario, Permissao.EmpresaGerenciarUsuarios);
            await empresaService.RemoverUsuario(usuarioid, cnpj);
            return Ok("Usuário removido");
            
       
        }

        [Authorize]
        [HttpGet("listarUsuarios/{cnpj}")]
        public async Task<IActionResult> ListarUsuarios(string cnpj, [FromQuery] PesquisaUsuarioFiltro filtro)
        {
            authService.Require(Usuario, Permissao.EmpresaVisualizarUsuarios);
            
            var pagina = await empresaService.ListarUsuarios(cnpj, filtro);


            return Ok(pagina);
            
        }
    }
}