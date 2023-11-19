using api;
using app.Entidades;
using app.Repositorios.Interfaces;
using app.Services.Interfaces;
using AutoMapper;

namespace app.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IPerfilRepositorio perfilRepositorio;
        private readonly AppDbContext dbContext;

        public PerfilService(IPerfilRepositorio perfilRepositorio, AppDbContext dbContext)
        {
            this.perfilRepositorio = perfilRepositorio;
            this.dbContext = dbContext;
        }

        public Perfil CriarPerfil(Perfil perfil, List<Permissao> permissoes)
        {
            var novoPerfil = perfilRepositorio.RegistraPerfil(perfil);

            foreach (var permissao in permissoes)
            {
                perfilRepositorio.AdicionaPermissaoAoPerfil(novoPerfil.Id, permissao);
            }

            dbContext.SaveChanges();

            return novoPerfil;
        }

        public async Task<Perfil> EditarPerfil(Perfil perfil, List<Permissao> permissoes)
        {
            var perfilDb = await perfilRepositorio.ObterPerfilPorIdAsync(perfil.Id) ??
                throw new KeyNotFoundException("Perfil não encontrado");

            perfilDb.Nome = perfil.Nome;

            var permissoesDeletadas = perfilDb.PerfilPermissoes!.Where(p => !permissoes.Contains(p.Permissao)).ToList();
            var permissoesNovas = permissoes.Where(p => !perfilDb.PerfilPermissoes!.Exists(pr => pr.Permissao == p)).ToList();

            foreach (var permissao in permissoesDeletadas)
            {
                perfilRepositorio.RemovePermissaoDoPerfil(permissao);
                perfilDb.PerfilPermissoes!.Remove(permissao);
            }

            foreach (var permissao in permissoesNovas)
            {
                perfilRepositorio.AdicionaPermissaoAoPerfil(perfil.Id, permissao);
            }

            await dbContext.SaveChangesAsync();

            return perfilDb;
        }

        public async Task ExcluirPerfil(Guid id)
        {
            var perfilParaExcluir = await perfilRepositorio.ObterPerfilPorIdAsync(id) ??
                throw new KeyNotFoundException("Perfil não encontrado");

            if (perfilParaExcluir.Tipo == TipoPerfil.Basico || perfilParaExcluir.Tipo == TipoPerfil.Administrador)
            {
                throw new InvalidOperationException("Esse Perfil não pode ser excluído.");
            }

            foreach (var perfilPermissao in perfilParaExcluir.PerfilPermissoes!)
            {
                perfilRepositorio.RemovePermissaoDoPerfil(perfilPermissao);
            }

            perfilRepositorio.RemovePerfil(perfilParaExcluir);
            dbContext.SaveChanges();
        }

        public async Task<List<Perfil>> ListarPerfisAsync(int pageIndex, int pageSize, string? nome = null)
        {
            var perfis = await perfilRepositorio.ListarPerfisAsync(pageIndex, pageSize, nome);
            var administrador = perfis.FirstOrDefault(p => p.Tipo == TipoPerfil.Administrador);

            if (administrador != null)
            {
                PreencherPermissoesAdministrador(administrador, false);
            }
            return perfis;
        }

        public async Task<Perfil?> ObterPorIdAsync(Guid id, bool comInternas = false)
        {
            var perfil = await perfilRepositorio.ObterPerfilPorIdAsync(id);

            if (perfil != null && perfil.Tipo == TipoPerfil.Administrador)
            {
                PreencherPermissoesAdministrador(perfil, comInternas);
            }

            return perfil;
        }

        private void PreencherPermissoesAdministrador(Perfil perfil, bool comInternas = false)
        {
            perfil.PermissoesSessao = Enum.GetValues<Permissao>().ToList(comInternas);
        }
    }
}