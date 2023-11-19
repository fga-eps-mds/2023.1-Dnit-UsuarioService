using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Abstractions;
using test.Fixtures;
using app.Repositorios.Interfaces;
using app.Entidades;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace test
{
    public class PerfilRepositorioTest : TestBed<Base>, IDisposable
    {
        private readonly IPerfilRepositorio repositorio;
        private readonly AppDbContext dbContext;

        public PerfilRepositorioTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            this.repositorio = fixture.GetService<IPerfilRepositorio>(testOutputHelper)!;
            this.dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
        }

        [Fact]
        public void RegistrarNovoPerfil_QuandoPerfilForPassado_DeveRetornarPerfilRegistrado()
        {
            Perfil perfil = Stub.PerfilStub.RetornaPerfil();

            var perfilRetornado = repositorio.RegistraPerfil(perfil);
            
            dbContext.SaveChanges();

            Assert.Equal(perfilRetornado.Nome, perfil.Nome);

            var perfilDb = dbContext.Perfis.Find(perfilRetornado.Id);
            Assert.NotNull(perfilDb);
        }

        [Fact]
        public void AdicionaPermissaoAoPerfil_QuandoPermissõesPassadas_DeveRetornarRelacaoCriada()
        {
            Perfil perfil = Stub.PerfilStub.RetornaPerfil();

            var perfilRetornado = repositorio.RegistraPerfil(perfil);

            PerfilPermissao perfilPermissao = repositorio.AdicionaPermissaoAoPerfil(perfilRetornado.Id, perfil.Permissoes.First());

            dbContext.SaveChanges();

            var perfilPermissaoDb = dbContext.PerfilPermissoes.Find(perfilPermissao.Id);

            var perfilatualizadoDb = dbContext.Perfis.Find(perfilRetornado.Id);

            Assert.Equal(perfilatualizadoDb.PerfilPermissoes[0].Permissao, perfilPermissaoDb.Permissao);
            Assert.NotNull(perfilatualizadoDb.Permissoes);
            Assert.NotNull(perfilPermissaoDb);
            Assert.Equal(perfilPermissaoDb.PerfilId, perfilRetornado.Id);
            Assert.Equal(perfilPermissaoDb.Permissao, perfil.Permissoes.First());            
        }

        [Fact]
        public void RemovePerfil_QuandoPerfilCadastrado_DeveRemoverPerfilDoBanco()
        {
            Perfil perfil = Stub.PerfilStub.RetornaPerfil();

            var perfilRetornado = repositorio.RegistraPerfil(perfil);

            repositorio.RemovePerfil(perfilRetornado);

            dbContext.SaveChanges();

            var perfilDb = dbContext.Perfis.Find(perfilRetornado.Id);

            Assert.Null(perfilDb);
        }

        [Fact]
        public void RemovePermissaoDoPerfil_QuandoPerfilTemUmaPermissao_DeveRemoverPermissao()
        {
            var perfil = Stub.PerfilStub.RetornaPerfil();

            var perfilRetornado = repositorio.RegistraPerfil(perfil);

            var perfilPermissao = repositorio.AdicionaPermissaoAoPerfil(perfilRetornado.Id, perfil.Permissoes.First());

            repositorio.RemovePermissaoDoPerfil(perfilPermissao);

            dbContext.SaveChanges();

            var perfilPermissaoDb = dbContext.PerfilPermissoes.Find(perfilPermissao.Id);

            Assert.Null(perfilPermissaoDb);
        }

        [Fact]
        public async Task ObterPerfilPorIdAsync_QuandoPerfilExiste_DeveRetornarPerfil()
        {
            var perfil = Stub.PerfilStub.RetornaPerfil();

            var perfilRetornado = repositorio.RegistraPerfil(perfil);

            dbContext.SaveChanges();

            var perfilRecuperado = await repositorio.ObterPerfilPorIdAsync(perfilRetornado.Id);

            Assert.NotNull(perfilRecuperado);
            Assert.Equal(perfilRecuperado.Nome, perfil.Nome);
        }

        [Fact]
        public async Task ObterPerfilPorIdAsync_QuandoPerfilNaoExiste_DeveRetornarNull()
        {
            var id = Guid.NewGuid();

            var perfilRecuperado = await repositorio.ObterPerfilPorIdAsync(id);

            Assert.Null(perfilRecuperado);
        }

        [Fact]
        public async Task ListarPerfis_QuandoMuitosPerfis_DeveRetornarUmaListaNaoVazia()
        {
            var lista = Stub.PerfilStub.RetornaListaDePerfis();
            List<string> nomeLista = new();

            lista.ForEach(p => nomeLista.Add(p.Nome));

            lista.ForEach(p => repositorio.RegistraPerfil(p));

            dbContext.SaveChanges();

            var listaRetornada = await repositorio.ListarPerfisAsync(1, 3);

            Assert.NotNull(listaRetornada);

            foreach (var item in listaRetornada)
            {
                Assert.Contains(item.Nome, nomeLista);
            }
        }

        public new void Dispose()
        {
            dbContext.RemoveRange(dbContext.PerfilPermissoes);
            dbContext.RemoveRange(dbContext.Perfis);
            dbContext.SaveChanges();
        }
    }
}