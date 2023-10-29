using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Abstractions;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using test.Fixtures;
using test.Stub;
using auth;
using app.Services.Interfaces;
using app.Entidades;
using app.Controllers;
using api.Usuarios;
using api.Senhas;
using api;
using app.Services;

namespace test
{
    public class EditarUsuarioPerfilTest : AuthTest, IDisposable
    {
        readonly UsuarioController controller;
        readonly AppDbContext dbContext;

        public EditarUsuarioPerfilTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
            controller = fixture.GetService<UsuarioController>(testOutputHelper)!;
            dbContext.PopulaUsuarios(5);
        }

        [Fact]
        public async void ObterUsuariosAsync_QuandoNãoTemPermissaoVisualizar_RetornaErroDePermissao()
        {
            AutenticarUsuario(controller, permissoes: new());
            var ex = await Assert.ThrowsAsync<AuthForbiddenException>(async () =>
                await controller.ListarAsync(new PesquisaUsuarioFiltro()));

            Assert.Contains("não tem a permissão: Visualizar Usuário", ex.Message);
        }

        [Fact]
        public async void ObterUsuariosAsync_QuandoFiltroVazio_RetornaTodosUsuarios()
        {
            AutenticarUsuario(controller, permissoes: new() { Permissao.UsuarioVisualizar });
            var filtro = new PesquisaUsuarioFiltro
            {
                ItemsPorPagina = 10,
            };
            var usuarios = await controller.ListarAsync(filtro);
            Assert.Equal(5, usuarios.Total);
        }

        [Fact]
        public async void ObterUsuariosAsync_QuandoFiltradoPorUf_RetornaUsuariosDaUfDada()
        {
            AutenticarUsuario(controller, permissoes: new() { Permissao.UsuarioVisualizar });
            var filtro = new PesquisaUsuarioFiltro
            {
                UfLotacao = UF.DF,
            };
            var u = dbContext.Usuario.ToList();
            u[0].UfLotacao = UF.DF;
            u[1].UfLotacao = UF.DF;
            u[2].UfLotacao = UF.DF;
            u[3].UfLotacao = UF.AM;
            u[4].UfLotacao = UF.AM;
            dbContext.SaveChanges();

            var lista = await controller.ListarAsync(filtro);

            Assert.Equal(UF.DF, lista.Items[0].UfLotacao);
            Assert.Equal(UF.DF, lista.Items[1].UfLotacao);
            Assert.Equal(UF.DF, lista.Items[2].UfLotacao);
            Assert.Equal(3, lista.Items.Count);
        }

        [Fact]
        public async void ObterUsuariosAsync_QuandoFiltradoPorMunicipio_RetornaUsuariosDoMunicipioDado()
        {
            AutenticarUsuario(controller, permissoes: new() { Permissao.UsuarioVisualizar });
            var m = new Municipio { Id = 1, Nome = "Municipio", Uf = UF.DF };
            dbContext.Municipio.Add(m);
            var filtro = new PesquisaUsuarioFiltro
            {
                MunicipioId = m.Id,
            };
            var u = dbContext.Usuario.ToList();
            u[0].MunicipioId = 1;
            u[1].MunicipioId = 1;
            u[2].MunicipioId = 2;
            u[3].MunicipioId = 2;
            u[4].MunicipioId = 2;
            dbContext.SaveChanges();

            var lista = await controller.ListarAsync(filtro);

            Assert.Equal(1, lista.Items[0].Municipio!.Id);
            Assert.Equal(1, lista.Items[1].Municipio!.Id);
            Assert.Equal(2, lista.Items.Count);
        }

        [Fact]
        public async void ObterUsuariosAsync_QuandoFiltradoPorNome_RetornaUsuariosComNomeDado()
        {
            AutenticarUsuario(controller, permissoes: new() { Permissao.UsuarioVisualizar });
            var m = new Municipio { Id = 1, Nome = "Municipio", Uf = UF.DF };
            dbContext.Municipio.Add(m);
            var filtro = new PesquisaUsuarioFiltro
            {
                Nome = "Silva"
            };
            var u = dbContext.Usuario.ToList();
            u[0].Nome = "Anderson Silva";
            u[1].Nome = "Silvania Almeida";
            u[2].Nome = "Silvio Santos";
            u[3].Nome = "Marcos";
            u[4].Nome = "Bianca";
            dbContext.SaveChanges();

            var lista = await controller.ListarAsync(filtro);

            Assert.Equal(2, lista.Items.Count);
        }

        [Fact]
        public async void ObterUsuariosAsync_QuandoFiltradoPorPerfil_RetornaUsuariosComPerfilDado()
        {
            AutenticarUsuario(controller, permissoes: new() { Permissao.UsuarioVisualizar });
            var filtro = new PesquisaUsuarioFiltro
            {
                PerfilId = Guid.NewGuid(),
            };
            var u = dbContext.Usuario.ToList();
            u[0].PerfilId = filtro.PerfilId;
            u[1].PerfilId = filtro.PerfilId;
            u[2].PerfilId = filtro.PerfilId;
            dbContext.SaveChanges();

            var lista = await controller.ListarAsync(filtro);

            Assert.Equal(3, lista.Items.Count);
        }

        [Fact]
        public async Task EditarPerfilUsuario_QuandoNaoTemPermissao_ErroDePermissao()
        {
            AutenticarUsuario(controller, permissoes: new());
            var dto = new EditarPerfilUsuarioDTO { NovoPerfilId = "id" };
            var ex = await Assert.ThrowsAsync<AuthForbiddenException>(async ()
                => await controller.EditarPerfilUsuario(1, dto));

            Assert.Contains("não tem a permissão: Editar Perfil", ex.Message);
        }

        [Fact]
        public async Task EditarPerfilUsuario_QuandoUsuarioNaoExiste_RetornaNaoEncontrado()
        {
            AutenticarUsuario(controller, permissoes: new() { Permissao.UsuarioPerfilEditar });
            var dto = new EditarPerfilUsuarioDTO { NovoPerfilId = "id" };
            var ex = await Assert.ThrowsAsync<ApiException>(async ()
                => await controller.EditarPerfilUsuario(-1, dto));

            Assert.Equal(ErrorCodes.UsuarioNaoEncontrado, ex.Error.Code);
        }

        [Fact]
        public async Task EditarPerfilUsuario_QuandoPerfilNaoExiste_RetornaPerfilNaoEncontrado()
        {
            AutenticarUsuario(controller, permissoes: new() { Permissao.UsuarioPerfilEditar });
            var usuarioId = dbContext.Usuario.First().Id;
            var dto = new EditarPerfilUsuarioDTO { NovoPerfilId = Guid.NewGuid().ToString() };
            var excecao = await Assert.ThrowsAsync<ApiException>(async ()
                => await controller.EditarPerfilUsuario(usuarioId, dto));

            Assert.Equal(ErrorCodes.PermissaoNaoEncontrada, excecao.Error.Code);
        }

        [Fact]
        public async Task EditarPerfilUsuario_QuandoTemPermissao_DeveAlterarPerfilDoUsuario()
        {
            AutenticarUsuario(controller, permissoes: new() { Permissao.UsuarioPerfilEditar });
            var novoPerfilParaUsuario = new Perfil
            {
                Id = Guid.NewGuid(),
                // Se remover esses parâmetros, os testes acusam que já foi inserido uma row duplicada
                Nome = "Teste",
                Tipo = TipoPerfil.Administrador
            };
            dbContext.Perfis.Add(novoPerfilParaUsuario);
            dbContext.SaveChanges();
            var usuario = dbContext.Usuario.First();
            var dto = new EditarPerfilUsuarioDTO { NovoPerfilId = novoPerfilParaUsuario.Id.ToString() };

            await controller.EditarPerfilUsuario(usuario.Id, dto);

            var usuarioEditado = dbContext.Usuario.Find(usuario.Id)!;

            Assert.Equal(novoPerfilParaUsuario.Id, usuarioEditado.PerfilId);
        }

        public new void Dispose()
        {
            dbContext.RemoveRange(dbContext.PerfilPermissoes);
            dbContext.RemoveRange(dbContext.Perfis);
            dbContext.RemoveRange(dbContext.Usuario);
            dbContext.RemoveRange(dbContext.Municipio);
        }
    }
}
