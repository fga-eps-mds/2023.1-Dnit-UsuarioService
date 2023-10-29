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
            var novoPerfilPermissao = new PerfilPermissao
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
            DefinirPerfilBasicoParaUsuariosComPerfilParaExcluir(perfil.Id);
            dbContext.Perfis.Remove(perfil);
        }

        private void DefinirPerfilBasicoParaUsuariosComPerfilParaExcluir(Guid perfilParaExcluirId)
        {
            var usuariosComPerfilParaExcluir = dbContext.Usuario.Where(u => u.PerfilId == perfilParaExcluirId);
            if (usuariosComPerfilParaExcluir.Any())
            {
                var perfilBasico = dbContext.Perfis.Where(p => p.Tipo == TipoPerfil.Basico).First();
                foreach (var u in usuariosComPerfilParaExcluir)
                    u.PerfilId = perfilBasico.Id;
            }
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

        public async Task<List<Perfil>> ListarPerfisAsync(int pageIndex, int pageSize, string? nome = null)
        {
            var query = dbContext.Perfis.AsQueryable();

            query = query.Include(p => p.PerfilPermissoes)
                         .Include(p => p.Usuarios);

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(p => p.Nome.ToLower().Contains(nome.ToLower()));
            }

            return await query
                .OrderBy(p => p.Nome)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}