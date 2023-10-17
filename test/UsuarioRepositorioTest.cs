using app.Repositorios;
using app.Repositorios.Interfaces;
using api.Usuarios;
using test.Stub;
using test.Fixtures;
using app.Entidades;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using System.Linq;
using AutoMapper;

namespace test
{
    public class UsuarioRepositorioTest : TestBed<Base>, IDisposable
    {
        IUsuarioRepositorio repositorio;
        AppDbContext dbContext;
        IMapper mapper;

        public UsuarioRepositorioTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
            repositorio = fixture.GetService<IUsuarioRepositorio>(testOutputHelper)!;
            mapper = fixture.GetService<IMapper>(testOutputHelper)!;
        }

        [Fact]
        public async void ObterUsuario_QuandoEmailForPassado_DeveRetornarUsuarioCorrespondente()
        {
            UsuarioStub usuarioStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            repositorio.CadastrarUsuarioDnit(usuarioDNIT);
            await dbContext.SaveChangesAsync();

            var usuarioObtido = repositorio.ObterUsuario("usuarioteste@gmail.com");

            Assert.Equal(usuarioDNIT.Email, usuarioObtido?.Email);
            Assert.Equal(usuarioDNIT.Senha, usuarioObtido?.Senha);
            Assert.Equal(usuarioDNIT.Nome, usuarioObtido?.Nome);
        }

        [Fact]
        public async void CadastrarUsuarioDnit_QuandoUsuarioForPassado_DeveCadastrarUsuarioComDadosPassados()
        {
            UsuarioStub usuarioStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            repositorio.CadastrarUsuarioDnit(usuarioDNIT);
            await dbContext.SaveChangesAsync();

            var usuarioObtido = dbContext.Usuario.Where(u => u.Email == usuarioDNIT.Email).FirstOrDefault();
            UsuarioDTO usuarioObtidoDTO = mapper.Map<UsuarioDTO>(usuarioObtido);

            Assert.Equal(usuarioDNIT.Email, usuarioObtidoDTO.Email);
            Assert.Equal(usuarioDNIT.Senha, usuarioObtidoDTO.Senha);
            Assert.Equal(usuarioDNIT.Nome, usuarioObtidoDTO.Nome);
            Assert.Equal(usuarioDNIT.UfLotacao, usuarioObtidoDTO.UfLotacao);
        }

        [Fact]
        public async void TrocarSenha_QuandoNovaSenhaForPassada_DeveAtualizarSenhaDoUsuario()
        {
            UsuarioStub usuarioStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            repositorio.CadastrarUsuarioDnit(usuarioDNIT);
            await dbContext.SaveChangesAsync();

            string novaSenha = "NovaSenha";

            repositorio.TrocarSenha(usuarioDNIT.Email, novaSenha);
            await dbContext.SaveChangesAsync();

            var usuarioObtido = repositorio.ObterUsuario(usuarioDNIT.Email);

            Assert.Equal(novaSenha, usuarioObtido?.Senha);
        }

        [Fact]
        public async void ObterEmailRedefinicaoSenha_QuandoUuidForPassado_DeveRetornarEmailCorrespondente()
        {
            UsuarioStub usuarioStub = new();
            RedefinicaoSenhaStub redefinicaoSenhaStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();
            var redefinicaoSenha = redefinicaoSenhaStub.ObterRedefinicaoSenha();

            repositorio.CadastrarUsuarioDnit(usuarioDNIT);
            await dbContext.SaveChangesAsync();

            var usuarioObtido = repositorio.ObterUsuario(usuarioDNIT.Email);

            repositorio.InserirDadosRecuperacao(redefinicaoSenha.UuidAutenticacao, usuarioObtido!.Id);
            await dbContext.SaveChangesAsync();

            var email = repositorio.ObterEmailRedefinicaoSenha(redefinicaoSenha.UuidAutenticacao);

            Assert.Equal(usuarioDNIT.Email, email);
        }

        [Fact]
        public async void RemoverUuidRedefinicaoSenha_QuandoUuidForPassado_DeveRemoverUuidDoBanco()
        {
            UsuarioStub usuarioStub = new();
            RedefinicaoSenhaStub redefinicaoSenhaStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();
            var redefinicaoSenha = redefinicaoSenhaStub.ObterRedefinicaoSenha();

            repositorio.CadastrarUsuarioDnit(usuarioDNIT);
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
        public async void CadastrarUsuarioTerceiro_QuandoUsuarioForPassado_DeveCadastrarUsuarioComDadosPassados()
        {
            UsuarioStub usuarioStub = new();
            var usuarioTerceiro = usuarioStub.RetornarUsuarioTerceiro();

            var empresa = new Empresa
            {
                Cnpj = usuarioTerceiro.CNPJ,
                RazaoSocial = "Empresa1"  
            };

            dbContext.Empresa.Add(empresa);
            await dbContext.SaveChangesAsync();

            repositorio.CadastrarUsuarioTerceiro(usuarioTerceiro);
            await dbContext.SaveChangesAsync();

            var usuarioObtido = repositorio.ObterUsuario(usuarioTerceiro.Email);
            UsuarioDTO usuarioObtidoDTO = mapper.Map<UsuarioDTO>(usuarioObtido);

            Assert.Equal(usuarioTerceiro.Email, usuarioObtidoDTO.Email);
            Assert.Equal(usuarioTerceiro.Senha, usuarioObtidoDTO.Senha);
            Assert.Equal(usuarioTerceiro.Nome, usuarioObtidoDTO.Nome);
            Assert.Equal(usuarioTerceiro.CNPJ, usuarioObtidoDTO.CNPJ);
        }

        public void Dispose()
        {
            dbContext.RemoveRange(dbContext.Usuario);
            dbContext.RemoveRange(dbContext.Empresa);
            dbContext.SaveChanges();
        }
    }
}
