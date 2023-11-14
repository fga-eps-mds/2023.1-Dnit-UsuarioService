using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Abstractions;
using test.Fixtures;
using test.Stub;
using app.Services.Interfaces;
using app.Entidades;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using api;
using app.Repositorios.Interfaces;
using System.Collections.Generic;

namespace test
{
    public class PerfilServiceTest : TestBed<Base>, IDisposable
    {
        private readonly IPerfilService perfilService;
        private readonly IPerfilRepositorio perfilRepositorio;
        private readonly IMapper mapper;
        private readonly AppDbContext dbContext;

        public PerfilServiceTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            perfilService = fixture.GetService<IPerfilService>(testOutputHelper)!;
            perfilRepositorio = fixture.GetService<IPerfilRepositorio>(testOutputHelper)!;
            mapper = fixture.GetService<IMapper>(testOutputHelper)!;
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
        }

        [Fact]
        public async Task CriarPerfil_QuandoPerfilPassado_DeveRetornarPerfilRegistrado()
        {
            var perfilDTO = PerfilStub.RetornaPerfilDTO();

            var perfil = mapper.Map<Perfil>(perfilDTO);

            var perfilRetorno = perfilService.CriarPerfil(perfil, perfilDTO.Permissoes);

            var perfilDb = await perfilRepositorio.ObterPerfilPorIdAsync(perfilRetorno.Id);

            Assert.NotNull(perfilDb);
            Assert.Equal(perfilDTO.Nome, perfilDb.Nome);
            Assert.Equal(1, perfilDb.Permissoes.Count());
        }

        [Fact]
        public async Task EditarPerfil_QuandoNomeJaCadastrado_DeveRetornarPerfilAtualizado()
        {
            var perfilDTO = PerfilStub.RetornaPerfilDTO();
            var perfilDTOEdicao = PerfilStub.RetornaPerfilDTO("Novo Nome");
            perfilDTOEdicao.Permissoes.Add(Permissao.RodoviaCadastrar);

            var perfil = mapper.Map<Perfil>(perfilDTO);
            var perfilEdicao = mapper.Map<Perfil>(perfilDTOEdicao);

            var perfilRetorno = perfilService.CriarPerfil(perfil, perfilDTO.Permissoes);

            perfilEdicao.Id = perfilRetorno.Id;
            var perfilRetornoEditado = await perfilService.EditarPerfil(perfilEdicao, perfilDTOEdicao.Permissoes);

            var perfilDb = await perfilRepositorio.ObterPerfilPorIdAsync(perfilRetorno.Id);

            Assert.NotNull(perfilRetorno);
            Assert.NotNull(perfilRetornoEditado);
            Assert.NotNull(perfilDb);
            Assert.Equal(perfilRetorno.Id, perfilDb.Id);
            Assert.Equal(perfilRetornoEditado, perfilDb);
            Assert.NotEqual(perfilDb.Nome, perfilDTO.Nome);
            Assert.NotEqual(perfilDb.Permissoes.Count(), perfilDTO.Permissoes.Count());
        }

        [Fact]
        public async Task EditarPerfil_QuandoPerfilNaoExiste_DeveLancarKeyNotFoundException()
        {
            var perfilDTO = PerfilStub.RetornaPerfilDTO();
            var perfil = mapper.Map<Perfil>(perfilDTO);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await perfilService.EditarPerfil(perfil, perfilDTO.Permissoes));
        }

        [Fact]
        public async Task ExcluirPerfil_QuandoPerfilExiste_DeveExcluirPerfilDoDB()
        {
            var perfilDTO = PerfilStub.RetornaPerfilDTO();
            var perfil = mapper.Map<Perfil>(perfilDTO);

            var perfilRetorno = perfilService.CriarPerfil(perfil, perfilDTO.Permissoes);

            await perfilService.ExcluirPerfil(perfilRetorno.Id);

            var perfilDb = await perfilRepositorio.ObterPerfilPorIdAsync(perfilRetorno.Id);

            Assert.Null(perfilDb);
        }

        [Fact]
        public async Task ExcluirPerfil_QuandoTemUsuariosComEssePerfilExcluido_PerfisDosUsuariosSaoDefinidosComoBasico()
        {
            var perfilParaExcluir = new Perfil { Nome = "Será excluído", Tipo = TipoPerfil.Customizavel };
            var perfilParaManter = new Perfil { Nome = "Para manter", Tipo = TipoPerfil.Customizavel };
            var perfilBasico = new Perfil { Nome = "Básico", Tipo = TipoPerfil.Basico };
            dbContext.Perfis.Add(perfilParaExcluir);
            dbContext.Perfis.Add(perfilParaManter);
            dbContext.Perfis.Add(perfilBasico);
            dbContext.SaveChanges();

            dbContext.PopulaUsuarios(5);
            var usuarios = dbContext.Usuario.ToList();
            usuarios[0].PerfilId = perfilParaExcluir.Id;
            usuarios[1].PerfilId = perfilParaExcluir.Id;
            usuarios[2].PerfilId = perfilParaExcluir.Id;
            usuarios[3].PerfilId = perfilParaManter.Id;
            usuarios[4].PerfilId = perfilParaManter.Id;
            dbContext.SaveChanges();

            await perfilService.ExcluirPerfil(perfilParaExcluir.Id);

            var usuariosComPerfilBasico = dbContext.Usuario
                .Include(u => u.Perfil)
                .Where(u => u.PerfilId == perfilBasico.Id)
                .Count();
            Assert.Equal(3, usuariosComPerfilBasico);
            Assert.Null(dbContext.Perfis.Find(perfilParaExcluir.Id));

            var usuariosComPerfilManter = dbContext.Usuario
                .Include(u => u.Perfil)
                .Where(u => u.PerfilId == perfilParaManter.Id)
                .Count();
            Assert.Equal(2, usuariosComPerfilManter);
        }

        [Fact]
        public async Task ExcluirPerfil_QuandoPerfilNaoExiste_DeveLancarKeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await perfilService.ExcluirPerfil(Guid.NewGuid()));
        }

        [Fact]
        public async Task ListarPerfis_QuandoNaoExistir_DeveRetornarListaVazia()
        {
            var lista = await perfilService.ListarPerfisAsync(1, 20);

            Assert.Empty(lista);
        }

        [Fact]
        public async Task ListarPerfis_QuandoExistir_DeveRetornarListaDePerfis()
        {
            var lista = PerfilStub.RetornaListaDePerfis(5);

            lista.ForEach(p => perfilService.CriarPerfil(p, p.Permissoes.ToList()));

            var listaRetorno = await perfilService.ListarPerfisAsync(1, 5);

            Assert.Equal(5, listaRetorno.Count());
        }

        public void Dispose()
        {
            dbContext.RemoveRange(dbContext.PerfilPermissoes);
            dbContext.RemoveRange(dbContext.Perfis);
            dbContext.RemoveRange(dbContext.Usuario);
            dbContext.SaveChanges();
        }
    }
}