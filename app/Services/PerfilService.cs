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
        private readonly IMapper mapper;

        public PerfilService(IPerfilRepositorio perfilRepositorio, AppDbContext dbContext, IMapper mapper)
        {
            this.perfilRepositorio = perfilRepositorio;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public Perfil CriarPerfil(Perfil perfil, List<Permissao> permissoes)
        {
            Perfil novoPerfil = perfilRepositorio.RegistraPerfil(perfil);

            foreach(var permissao in permissoes)
            {
                var novoPermissaoPerfil = perfilRepositorio.AdicionaPermissaoAoPerfil(novoPerfil.Id, permissao);
                novoPerfil.PerfilPermissoes!.Add(novoPermissaoPerfil);
            }

            dbContext.SaveChanges();

            return novoPerfil;
        }

        public async Task<Perfil> EditarPerfil(Perfil perfil, List<Permissao> permissoes)
        {    
            Perfil perfilDb = await perfilRepositorio.ObterPerfilPorIdAsync(perfil.Id) ?? 
                throw new KeyNotFoundException("Perfil não encontrado");

            if(perfilDb.Nome != perfil.Nome)
            {
                perfilDb.Nome = perfil.Nome;
            }

            List<Permissao> permissoesRegistradas = perfilDb.Permissoes!.ToList();

            foreach(var permissao in permissoesRegistradas)
            {
                if(!permissoes.Contains(permissao))
                {
                    var permissaoRemovida = perfilDb.PerfilPermissoes!.Where(p => p.Permissao == permissao).FirstOrDefault();
                    perfilRepositorio.RemovePermissaoDoPerfil(permissaoRemovida!);
                    perfilDb.PerfilPermissoes!.Remove(permissaoRemovida!);
                }
                else
                {
                    permissoes.Remove(permissao);
                }
            }

            foreach(var permissao in permissoes)
            {
                var novoPerfilPermissao = perfilRepositorio.AdicionaPermissaoAoPerfil(perfil.Id, permissao);
                perfilDb.PerfilPermissoes!.Add(novoPerfilPermissao);
            }

            dbContext.SaveChanges();

            return perfilDb; 
        }

        public async Task ExcluirPerfil(Guid id)
        {   
            var perfilDb = await perfilRepositorio.ObterPerfilPorIdAsync(id) ??
                throw new KeyNotFoundException("Perfil não encontrado");

            foreach(var perfilPermissao in perfilDb.PerfilPermissoes!)
            {
                perfilRepositorio.RemovePermissaoDoPerfil(perfilPermissao);
            }

            perfilRepositorio.RemovePerfil(perfilDb);

            dbContext.SaveChanges();
        }

        public async Task<List<Perfil>> ListarPerfisAsync(int pageIndex, int pageSize)
        {
            return await perfilRepositorio.ListarPerfisAsync(pageIndex, pageSize);
        }
    }
}