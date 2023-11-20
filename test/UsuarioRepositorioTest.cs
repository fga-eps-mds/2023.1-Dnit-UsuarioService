using app.Repositorios.Interfaces;
using test.Stub;
using test.Fixtures;
using app.Entidades;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using System.Linq;
using System.Threading.Tasks;

namespace test
{
    public class UsuarioRepositorioTest : TestBed<Base>, IDisposable
    {
        readonly IUsuarioRepositorio repositorio;
        readonly AppDbContext dbContext;

        public UsuarioRepositorioTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
            repositorio = fixture.GetService<IUsuarioRepositorio>(testOutputHelper)!;
        }

        [Fact]
        public async Task ObterUsuario_QuandoEmailForPassado_DeveRetornarUsuarioCorrespondente()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            await repositorio.CadastrarUsuarioDnit(usuarioDNIT);
            await dbContext.SaveChangesAsync();

            var usuarioObtido = repositorio.ObterUsuario("usuarioteste@gmail.com");

            Assert.Equal(usuarioDNIT.Email, usuarioObtido?.Email);
            Assert.Equal(usuarioDNIT.Senha, usuarioObtido?.Senha);
            Assert.Equal(usuarioDNIT.Nome, usuarioObtido?.Nome);
        }

        [Fact]
        public async Task CadastrarUsuarioDnit_QuandoUsuarioForPassado_DeveCadastrarUsuarioComDadosPassados()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            await repositorio.CadastrarUsuarioDnit(usuarioDNIT);
            await dbContext.SaveChangesAsync();

            var usuarioObtido = dbContext.Usuario.Where(u => u.Email == usuarioDNIT.Email).FirstOrDefault()!;

            Assert.Equal(usuarioDNIT.Email, usuarioObtido.Email);
            Assert.Equal(usuarioDNIT.Senha, usuarioObtido.Senha);
            Assert.Equal(usuarioDNIT.Nome, usuarioObtido.Nome);
            Assert.Equal(usuarioDNIT.UfLotacao, usuarioObtido.UfLotacao);
        }

        [Fact]
        public async Task TrocarSenha_QuandoNovaSenhaForPassada_DeveAtualizarSenhaDoUsuario()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            await repositorio.CadastrarUsuarioDnit(usuarioDNIT);
            await dbContext.SaveChangesAsync();

            string novaSenha = "NovaSenha";

            repositorio.TrocarSenha(usuarioDNIT.Email, novaSenha);
            await dbContext.SaveChangesAsync();

            var usuarioObtido = repositorio.ObterUsuario(usuarioDNIT.Email);

            Assert.Equal(novaSenha, usuarioObtido?.Senha);
        }

        [Fact]
        public async Task ObterEmailRedefinicaoSenha_QuandoUuidForPassado_DeveRetornarEmailCorrespondente()
        {
            var usuarioStub = new UsuarioStub();
            var redefinicaoSenhaStub = new RedefinicaoSenhaStub();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();
            var redefinicaoSenha = redefinicaoSenhaStub.ObterRedefinicaoSenha();

            await repositorio.CadastrarUsuarioDnit(usuarioDNIT);
            await dbContext.SaveChangesAsync();

            var usuarioObtido = repositorio.ObterUsuario(usuarioDNIT.Email);

            repositorio.InserirDadosRecuperacao(redefinicaoSenha.UuidAutenticacao, usuarioObtido!.Id);
            await dbContext.SaveChangesAsync();

            var email = repositorio.ObterEmailRedefinicaoSenha(redefinicaoSenha.UuidAutenticacao);

            Assert.Equal(usuarioDNIT.Email, email);
        }

        [Fact]
        public async Task RemoverUuidRedefinicaoSenha_QuandoUuidForPassado_DeveRemoverUuidDoBanco()
        {
            var usuarioStub = new UsuarioStub();
            var redefinicaoSenhaStub = new RedefinicaoSenhaStub();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();
            var redefinicaoSenha = redefinicaoSenhaStub.ObterRedefinicaoSenha();

            await repositorio.CadastrarUsuarioDnit(usuarioDNIT);
            await dbContext.SaveChangesAsync();

            var usuarioObtido = repositorio.ObterUsuario(usuarioDNIT.Email);

            repositorio.InserirDadosRecuperacao(redefinicaoSenha.UuidAutenticacao, usuarioObtido!.Id);
            await dbContext.SaveChangesAsync();

            repositorio.RemoverUuidRedefinicaoSenha(redefinicaoSenha.UuidAutenticacao);
            await dbContext.SaveChangesAsync();

            var email = repositorio.ObterEmailRedefinicaoSenha(redefinicaoSenha.UuidAutenticacao);

            Assert.Null(email);
        }


        [Fact]
        public async Task CadastrarUsuarioTerceiro_QuandoUsuarioForPassado_DeveCadastrarUsuarioComDadosPassados()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioTerceiro = usuarioStub.RetornarUsuarioTerceiro();

            var empresa = new Empresa
            {
                Cnpj = usuarioTerceiro.CNPJ,
                RazaoSocial = "Empresa1"
            };

            dbContext.Empresa.Add(empresa);
            await dbContext.SaveChangesAsync();

            await repositorio.CadastrarUsuarioTerceiro(usuarioTerceiro);
            await dbContext.SaveChangesAsync();

            var usuarioObtido = repositorio.ObterUsuario(usuarioTerceiro.Email)!;

            Assert.Equal(usuarioTerceiro.Email, usuarioObtido.Email);
            Assert.Equal(usuarioTerceiro.Senha, usuarioObtido.Senha);
            Assert.Equal(usuarioTerceiro.Nome, usuarioObtido.Nome);
            Assert.Equal(usuarioTerceiro.CNPJ, usuarioObtido?.Empresa?.Cnpj);
        }

        public new void Dispose()
        {
            dbContext.RemoveRange(dbContext.Usuario);
            dbContext.RemoveRange(dbContext.Empresa);
            dbContext.SaveChanges();
        }
    }
}
