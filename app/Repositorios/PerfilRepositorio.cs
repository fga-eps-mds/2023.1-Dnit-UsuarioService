using api;
using app.Entidades;
using app.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace app.Repositorios
{
    public class PerfilRepositorio : IPerfilRepositorio
    {
        private AppDbContext dbContext;

        public PerfilRepositorio(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Perfil RegistraPerfil(Perfil perfil)
        {
            perfil.Id = Guid.NewGuid();

            dbContext.Add(perfil);
            
            return perfil;
        }

        public PerfilPermissao AdicionaPermissaoAoPerfil(Guid perfilId, Permissao permissao)
        {
            var  novoPerfilPermissao = new PerfilPermissao
            {
                Id = Guid.NewGuid(),
                PerfilId = perfilId,
                Permissao = permissao
            };
            
            dbContext.Add(novoPerfilPermissao);

            return novoPerfilPermissao;
        }

        public void RemovePerfil(Perfil perfil)
        {            
            dbContext.Perfis.Remove(perfil);
        }

        public void RemovePermissaoDoPerfil(PerfilPermissao perfilPermissao)
        {
            dbContext.PerfilPermissoes.Remove(perfilPermissao);
        }

        public async Task<Perfil?> ObterPerfilPorIdAsync(Guid id)
        {
            var query = dbContext.Perfis.AsQueryable();

            query = query.Include(p => p.PerfilPermissoes);

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Perfil>> ListarPerfisAsync(int pageIndex, int pageSize)
        {
            var query = dbContext.Perfis.AsQueryable();

            query = query.Include(p => p.PerfilPermissoes);
            query = query.Include(p => p.Usuarios);

            return await query
                .OrderBy(p => p.Nome)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}