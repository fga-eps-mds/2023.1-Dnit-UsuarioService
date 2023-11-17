using api;
using api.Empresa;
using app.Controller;
using app.Entidades;
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
        public async void CriarEmpresa_QuandoTemPermissao_DeveRetornarOk()
        {
            var empresa = EmpresaStub.RetornarEmpresaDTO();
            
            var resposta = await empresaController.CadastrarEmpresa(empresa);
            Assert.IsType<OkResult>(resposta);
            
           
        }

        [Fact]
        public async void EditarEmpresa_QuandoNaoTemPermissao_DeveBloquear()
        {
            var empresa = EmpresaStub.RetornarEmpresaDTO();

            AutenticarUsuario(empresaController, permissoes: new(){Permissao.EmpresaCadastrar});

            var resposta = empresaController.CadastrarEmpresa(empresa);
            

            empresa.RazaoSocial = "Nova Razao";

            AutenticarUsuario(empresaController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await empresaController.EditarEmpresa(empresa.Cnpj, empresa));
        }

        [Fact]
        public async void EditarEmpresa_QuandoTemPermissao_DeveRetornarOk()
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
        public async void ExcluirEmpresa_QuandoNaoTemPermissao_DeveBloquear()
        {
            AutenticarUsuario(empresaController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await empresaController.ExcluirEmpresa("1234567"));
        }

        [Fact]
        public async void ExcluirEmpresa_QuandoTemPermissaoEEmpresaNaoExiste_DeveLancarNotFound()
        {
            var resposta = await empresaController.ExcluirEmpresa("123");

            Assert.IsType<NotFoundObjectResult>(resposta);
        }

        [Fact]
        public async void ExcluirEmpresa_QuandoTemPermissao_DeveRetornarOk()
        {
            var empresa= EmpresaStub.RetornarEmpresa();
            empresa.Cnpj = "123456";

            dbContext.Empresa.Add(empresa);
            dbContext.SaveChanges();

            var resposta = await empresaController.ExcluirEmpresa(empresa.Cnpj);
            
            Assert.IsType<OkObjectResult>(resposta);
        }
        public void Dispose()
        {
            dbContext.RemoveRange(dbContext.PerfilPermissoes);
            dbContext.RemoveRange(dbContext.Empresa);
            dbContext.SaveChanges();
        }

        [Fact]
        public async void ListarEmpresas_QuandoNaoTemPermissao_DeveBloquear()
        {
            AutenticarUsuario(empresaController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await empresaController.ListarEmpresas(1, 20));
        }

        [Fact]
        public async void ListarEmpresas_QuandoTemPermissao_DeveRetornarOk()
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
        public async void ListarEmpresas_QuandoNaoTemPermissao_DeveRetornarErro()
        {
            AutenticarUsuario(empresaController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => await empresaController.ListarEmpresas(1,5));
            
        }

        [Fact]
        public async void VisualizarEmpresa_QuandoNaoTemPermissao_DeveRetornarErro()
        {
            AutenticarUsuario(empresaController, permissoes: new());
            await Assert.ThrowsAsync<AuthForbiddenException>(async () => empresaController.VisualizarEmpresa("123445"));
            
        }

        [Fact]
        public async void VisualizarEmpresa_QuandoTemPermissao_DeveRetornarEmpresa()
        {
            
            var empresa = EmpresaStub.RetornarEmpresaDTO();
            var resposta = empresaController.CadastrarEmpresa(empresa);

            

            var respostaVisualizar = empresaController.VisualizarEmpresa(empresa.Cnpj);

            var empresaEditado = (respostaVisualizar as OkObjectResult)!.Value as EmpresaModel;

            Assert.IsType<OkObjectResult>(respostaVisualizar);
            Assert.NotNull(empresaEditado);
        }

    }
}