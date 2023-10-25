using api;
using api.Perfis;
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
        private readonly IMapper mapper;

        public PerfilService(IPerfilRepositorio perfilRepositorio, AppDbContext dbContext, IMapper mapper)
        {
            this.perfilRepositorio = perfilRepositorio;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public Perfil CriarPerfil(Perfil perfil, List<Permissao> permissoes)
        {
            var novoPerfil = perfilRepositorio.RegistraPerfil(perfil);

            foreach(var permissao in permissoes)
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
                if (permissao.Perfil.Tipo == TipoPerfil.Basico && permissao.Permissao.ToString().StartsWith("Perfil")) {
                    throw new UnauthorizedAccessException("A edição das permissões sobre o perfil básico esta desabilitada no momento. Isso será possível após a implementação da atribuição dos usuários aos perfis. Isso está desabilitado porque se nenhum usuário tiver acesso a edição de perfil, não será possível ativá-los depois.");
                }
                perfilRepositorio.RemovePermissaoDoPerfil(permissao);
                perfilDb.PerfilPermissoes!.Remove(permissao);
            }

            foreach(var permissao in permissoesNovas)
            {
                perfilRepositorio.AdicionaPermissaoAoPerfil(perfil.Id, permissao);
            }

            await dbContext.SaveChangesAsync();

            return perfilDb; 
        }

        public async Task ExcluirPerfil(Guid id)
        {   
            var perfilDb = await perfilRepositorio.ObterPerfilPorIdAsync(id) ??
                throw new KeyNotFoundException("Perfil não encontrado");

            if (perfilDb.Tipo == TipoPerfil.Basico || perfilDb.Tipo == TipoPerfil.Administrador)
            {
                throw new InvalidOperationException("Esse Perfil não pode ser excluído.");
            }

            foreach(var perfilPermissao in perfilDb.PerfilPermissoes!)
            {
                perfilRepositorio.RemovePermissaoDoPerfil(perfilPermissao);
            }

            perfilRepositorio.RemovePerfil(perfilDb);

            dbContext.SaveChanges();
        }

        public async Task<List<Perfil>> ListarPerfisAsync(int pageIndex, int pageSize, string? nome = null)
        {
            var perfis = await perfilRepositorio.ListarPerfisAsync(pageIndex, pageSize, nome);
            var administrador = perfis.FirstOrDefault(p => p.Tipo == TipoPerfil.Administrador);

            if (administrador != null)
            {
                PreencherPermissoesAdministrador(administrador);
            }
            return perfis;
        }

        public async Task<Perfil?> ObterPorIdAsync(Guid id)
        {
            var perfil = await perfilRepositorio.ObterPerfilPorIdAsync(id);
            
            if(perfil != null && perfil.Tipo == TipoPerfil.Administrador)
            {
                PreencherPermissoesAdministrador(perfil);
            }

            return perfil;
        }

        private void PreencherPermissoesAdministrador(Perfil perfil)
        {
            perfil.PermissoesSessao = Enum.GetValues<Permissao>().ToList();
        }
    }
}