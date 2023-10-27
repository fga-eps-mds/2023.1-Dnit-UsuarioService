using api.Usuarios;
using api.Senhas;
using Microsoft.AspNetCore.Mvc;
using app.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using app.Services;
using api;
using System.Data.Common;

namespace app.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : AppController
    {
        private readonly IUsuarioService usuarioService;
        private readonly AuthService authService;

        public UsuarioController(
            IUsuarioService usuarioService,
            AuthService authService
        )
        {
            this.usuarioService = usuarioService;
            this.authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Logar([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                var resultado = await usuarioService.AutenticarUsuarioAsync(usuarioDTO.Email, usuarioDTO.Senha);
                return Ok(resultado);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("permissoes")]
        [Authorize]
        public async Task<List<Permissao>> ListarPermissoes()
        {
            var userId = authService.GetUserId(Usuario);
            return await usuarioService.ListarPermissoesAsync(userId);
        }

        [HttpPost("atualizarToken")]
        public async Task<LoginModel> AtualizarToken([FromBody] AtualizarTokenDto atualizarTokenDto)
        {
            return await usuarioService.AtualizarTokenAsync(atualizarTokenDto);
        }


        [HttpPost("cadastrarUsuarioDnit")]
        public async Task<IActionResult> CadastrarUsuarioDnit([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                await usuarioService.CadastrarUsuarioDnit(usuarioDTO);

                return StatusCode(201, new NoContentResult());
            }
            catch (DbException)
            {
                return Conflict("Usuário já cadastrado.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Houve um erro interno no servidor.");
            }
        }

        [HttpPost("cadastrarUsuarioTerceiro")]
        public IActionResult CadastrarUsuarioTerceiro([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                usuarioService.CadastrarUsuarioTerceiro(usuarioDTO);

                return StatusCode(201, new NoContentResult());
            }
            catch (DbException)
            {
                return Conflict("Usuário já cadastrado.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Houve um erro interno no servidor.");
            }
        }

        [HttpPut("recuperarSenha")]
        public async Task<IActionResult> RecuperarSenhaAsync([FromBody] UsuarioDTO usuarioDto)
        {
            try
            {
                await usuarioService.RecuperarSenha(usuarioDto);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("redefinirSenha")]
        public async Task<IActionResult> RedefinirSenhaAsync([FromBody] RedefinicaoSenhaDTO redefinirSenhaDto)
        {
            try
            {
                await usuarioService.TrocaSenha(redefinirSenhaDto);

                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet()]
        public async Task<ListaPaginada<UsuarioModelNovo>> ListarAsync([FromQuery] PesquisaUsuarioFiltro filtro)
        {
            // authService.Require(Usuario, Permissao.UsuarioVisualizar);
            return await usuarioService.ObterUsuariosAsync(filtro);
        }

        [HttpPatch("{id}/perfil")]
        public async Task EditarPerfilUsuario([FromRoute] int id, [FromBody] string novoPerfilId)
        {
            authService.Require(Usuario, Permissao.PerfilEditar);
            await usuarioService.EditarUsuarioPerfil(id, novoPerfilId);
        }
    }
}
