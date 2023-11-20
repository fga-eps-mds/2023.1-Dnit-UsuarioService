using System.Linq;
using System.Threading.Tasks;
using api;
using api.Empresa;
using api.Usuarios;
using app.Controller;
using app.Entidades;
using app.Services;
using auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Fixtures;
using test.Stub;
using Xunit.Abstractions;

namespace test
{
    public class EmpresaControllerTest : AuthTest, IDisposable
    {
        readonly AppDbContext dbContext;
        readonly EmpresaController empresaController;

        public EmpresaControllerTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
            empresaController = fixture.GetService<EmpresaController>(testOutputHelper)!;

            AutenticarUsuario(empresaController);            
        }
        
        [Fact]
        public void CriarEmpresa_QuandoNaoTemPermissao_DeveBloquear()
        {
            var empresa = EmpresaStub.RetornarEmpresaDTO();
            
            AutenticarUsuario(empresaController, permissoes: new());
            Assert.ThrowsAsync<AuthForbiddenException>(() => empresaController.CadastrarEmpresa(empresa));
        }     

        [Fact]
        public async Task CriarEmpresa_QuandoTemPermissao_DeveRetornarOk()
        {
            var empresa = EmpresaStub.RetornarEmpresaDTO();
            
            var resposta = await empresaController.CadastrarEmpresa(empresa);
            Assert.IsType<OkResult>(resposta);           
        }

        [Fact]
        public async Task EditarEmpresa_QuandoNaoTemPermissao_DeveBloquear()
        {
            var empresa = EmpresaStub.RetornarEmpresaDTO();

            AutenticarUsuario(empresaController, permissoes: new(){Permissao.EmpresaCadastrar});

            var resposta = empresaController.CadastrarEmpresa(empresa);            

            empresa.RazaoSocial = "Nova Razao";

            AutenticarUsuario(empresaController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await empresaController.EditarEmpresa(empresa.Cnpj, empresa));
        }

        [Fact]
        public async Task EditarEmpresa_QuandoTemPermissao_DeveRetornarOk()
        {
            var empresa = EmpresaStub.RetornarEmpresaDTO();
            var resposta = empresaController.CadastrarEmpresa(empresa);

            empresa.RazaoSocial = "Nova Razao";

            var respostaEditar = await empresaController.EditarEmpresa(empresa.Cnpj, empresa);

            var empresaEditado = (respostaEditar as OkObjectResult)!.Value as EmpresaModel;

            Assert.IsType<OkObjectResult>(respostaEditar);
            Assert.NotNull(empresaEditado);

        }

        [Fact]
        public async Task ExcluirEmpresa_QuandoNaoTemPermissao_DeveBloquear()
        {
            AutenticarUsuario(empresaController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await empresaController.ExcluirEmpresa("1234567"));
        }

        [Fact]
        public async Task ExcluirEmpresa_QuandoTemPermissaoEEmpresaNaoExiste_DeveLancarNotFound()
        {
            await Assert.ThrowsAsync<ApiException>(async() => await empresaController.ExcluirEmpresa("123"));
        }

        [Fact]
        public async Task ExcluirEmpresa_QuandoTemPermissao_DeveRetornarOk()
        {
            var empresa= EmpresaStub.RetornarEmpresa();
            empresa.Cnpj = "123456";

            dbContext.Empresa.Add(empresa);
            dbContext.SaveChanges();

            var resposta = await empresaController.ExcluirEmpresa(empresa.Cnpj);
            
            Assert.IsType<OkObjectResult>(resposta);
        }

        [Fact]
        public async Task ListarEmpresas_QuandoNaoTemPermissao_DeveBloquear()
        {
            AutenticarUsuario(empresaController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await empresaController.ListarEmpresas(1, 20));
        }

        [Fact]
        public async Task ListarEmpresas_QuandoTemPermissao_DeveRetornarOk()
        {
            var lista = EmpresaStub.RetornaListaEmpresaDTO(5);
            var administrador = PerfilStub.RetornaPerfil(tipo: TipoPerfil.Administrador);

            dbContext.Perfis.Add(administrador);
            dbContext.SaveChanges();

            lista.ForEach(async p => await empresaController.CadastrarEmpresa(p));

            var resposta = await empresaController.ListarEmpresas(1, 10);

            Assert.IsType<OkObjectResult>(resposta);
            Assert.NotNull(resposta);            
        }

        [Fact]
        public async Task ListarEmpresas_QuandoNaoTemPermissao_DeveRetornarErro()
        {
            AutenticarUsuario(empresaController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await empresaController.ListarEmpresas(1,5));
        }

        [Fact]
        public async Task VisualizarEmpresa_QuandoNaoTemPermissao_DeveRetornarErro()
        {
            AutenticarUsuario(empresaController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => empresaController.VisualizarEmpresa("123445"));
        }

        [Fact]
        public async Task VisualizarEmpresa_QuandoTemPermissao_DeveRetornarEmpresa()
        {
            
            var empresa = EmpresaStub.RetornarEmpresaDTO();
            var resposta = empresaController.CadastrarEmpresa(empresa);           

            var respostaVisualizar = empresaController.VisualizarEmpresa(empresa.Cnpj);

            var empresaEditado = (respostaVisualizar as OkObjectResult)!.Value as EmpresaModel;

            Assert.IsType<OkObjectResult>(respostaVisualizar);
            Assert.NotNull(empresaEditado);
        }
        [Fact]
        public async Task AdicionarUsuario_QuandoTemPermissao_DeveRetornarOk()
        {
            var empresa = dbContext.PopulaEmpresa(1).First();
            var usuario = dbContext.PopulaUsuarios(1).First();
            var resposta = await empresaController.AdicionarUsuario(empresa.Cnpj, usuario.Id);
            Assert.IsType<OkObjectResult>(resposta);
        }

        [Fact] 
        public async Task AdicionarUsuario_QuandoNaoTemPermissao_DeveRetornarErro()
        {
            var empresa = dbContext.PopulaEmpresa(1).First();
            var usuario = dbContext.PopulaUsuarios(1).First();
            AutenticarUsuario(empresaController, permissoes: new());

            await Assert.ThrowsAsync<AuthForbiddenException>(() => empresaController.AdicionarUsuario(empresa.Cnpj, usuario.Id));  
        }

        [Fact]
        public async Task ListarUsuarios_QuandoColocadoCnpj()
        {
            var usuario = dbContext.PopulaUsuarios(1).First();
            var empresa = dbContext.PopulaEmpresa(1).First();
            
            await empresaController.AdicionarUsuario(empresa.Cnpj, usuario.Id);
            await dbContext.SaveChangesAsync();
            var lista = await empresaController.ListarUsuarios(empresa.Cnpj, new PesquisaUsuarioFiltro());

            Assert.NotNull(lista);            
        }
        
        [Fact]
        public async Task ExcluirUsuario_QuandoTemPermissao_DeveRetornarOk()
        {
            var empresa = dbContext.PopulaEmpresa(1).First();
            var usuario = dbContext.PopulaUsuarios(1).First();

            var resposta = await empresaController.RemoverUsuario(empresa.Cnpj, usuario.Id);
            Assert.IsType<OkObjectResult>(resposta);
        }

        public new void Dispose()
        {
            dbContext.RemoveRange(dbContext.PerfilPermissoes);
            dbContext.RemoveRange(dbContext.Empresa);
            dbContext.SaveChanges();
        }
    }
}