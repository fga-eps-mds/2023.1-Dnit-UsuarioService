using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Abstractions;
using test.Fixtures;
using app.Repositorios.Interfaces;
using app.Entidades;
using Xunit.Sdk;
using api.Perfis;
using System.Linq;
using System.Collections.Generic;
using api;
using api.Usuarios;
using System.Data.Common;
using test.Stub;
using System.Threading.Tasks;
using app.Repositorios;

namespace test
{
    public class EmpresaRepositorioTest : TestBed<Base>, IDisposable
    {
        private readonly IEmpresaRepositorio repositorio;

        private readonly IUsuarioRepositorio repositorio_usuario;
        private readonly AppDbContext dbContext;
      
        public EmpresaRepositorioTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {   

            repositorio = fixture.GetService<IEmpresaRepositorio>
            (testOutputHelper)!;

            repositorio_usuario = fixture.GetService<IUsuarioRepositorio>
            (testOutputHelper)!;
            
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
        }

        [Fact]
        public void DeletarEmpresa_QuandoEmpresaPassado_DeveRemoverDoBanco()
        {         
            Empresa empresa = Stub.EmpresaStub.RetornarEmpresa();

            repositorio.DeletarEmpresa(empresa);

            dbContext.SaveChanges();

            var empresaDb = dbContext.Empresa.Find(empresa.RazaoSocial);

            Assert.Null(empresaDb);
        }

        [Fact]
        public void AdicionarEmpresa_QuandoEmpresaPassado_DeveRetornarEmpresa()
        {         
            Empresa empresa = Stub.EmpresaStub.RetornarEmpresa();

            var empresaCadastrado = empresa;

            repositorio.CadastrarEmpresa(empresa);

            dbContext.SaveChanges();
            var empresaDb = dbContext.Empresa.FirstOrDefault(e => e.Cnpj == empresa.Cnpj);

            Assert.NotNull(empresaDb);
            Assert.Equal(empresaCadastrado.RazaoSocial,empresa.RazaoSocial );
        }

        [Fact]
        public async Task ObterPerfilPorIdAsync_QuandoPerfilExiste_DeveRetornarPerfil()
        {
            var empresa = EmpresaStub.RetornarEmpresa();

            repositorio.CadastrarEmpresa(empresa);

            dbContext.SaveChanges();

            var EmpresaRecuperado = await repositorio.ObterEmpresaPorCnpjAsync(empresa.Cnpj);

            Assert.NotNull(EmpresaRecuperado);
            Assert.Equal(EmpresaRecuperado.RazaoSocial, empresa.RazaoSocial);
        }

        [Fact]
        public void VisualizarEmpresa_QuandoColocadoCNPJ()
        {
            var empresa = dbContext.PopulaEmpresa(1).First();

            var empresaVisualiza = repositorio.VisualizarEmpresa(empresa.Cnpj);

            Assert.Equal(empresaVisualiza?.Cnpj,empresa.Cnpj);
        }

        [Fact]
        public async Task ListarEmpresas_QuandoColocadoTamanho()
        {
            var lista = EmpresaStub.RetornaListaDeEmpresas();
            var listaUFs = new List<UF>
            {
                UF.DF,
                UF.GO
            };
            
            List<string> nomeLista = new();

            lista.ForEach(p => nomeLista.Add(p.RazaoSocial));

            lista.ForEach(p => repositorio.CadastrarEmpresa(p));

            dbContext.SaveChanges();

            var listaRetornada = await repositorio.ListarEmpresas(1,3, listaUFs);
            Assert.NotNull(listaRetornada);

            foreach(var item in lista)
            {
                Assert.Contains(item.RazaoSocial, nomeLista);
            }
        }

        [Fact]
        public void AdicionarUsuario_QuandoPassado_DeveRetornarUsuario()
        {
            var empresa = dbContext.PopulaEmpresa(1)[0];
            var usuario = dbContext.PopulaUsuarios(1)[0];
            
            repositorio.AdicionarUsuario(usuario.Id, empresa.Cnpj);

            dbContext.SaveChanges();
            var empresaDb = dbContext.Empresa.FirstOrDefault(e => e.Usuarios.FirstOrDefault() == empresa.Usuarios[0]);

            Assert.NotNull(empresaDb);
        }

        public void Dispose()
        {
            dbContext.RemoveRange(dbContext.PerfilPermissoes);
            dbContext.RemoveRange(dbContext.Empresa);
            dbContext.SaveChanges();
        }
    }
}