using app.Entidades;
using app.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace app.Repositorios
{
    public class EmpresaRepositorio : IEmpresaRepositorio
    {
        
        private AppDbContext dbContext;

        public EmpresaRepositorio(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CadastrarEmpresa(Empresa empresa)
        {
            dbContext.Add(empresa);
        }

        public Empresa? VisualizarEmpresa(string empresaid)
        {
            var empresa = dbContext.Empresa.Where(e => e.Cnpj == empresaid).FirstOrDefault();
            return empresa;
        }

        public async Task DeletarEmpresa(Empresa empresa){
            var empresaParaExcluir = dbContext.Empresa.Where(e => e.Cnpj == empresa.Cnpj).FirstOrDefault();

            dbContext.Empresa.Remove(empresaParaExcluir);
        }

        public async Task<Empresa?> ObterEmpresaPorIdAsync(string Cnpj)
        {
            var query = dbContext.Empresa.AsQueryable();

            return await query.FirstOrDefaultAsync(p => p.Cnpj == Cnpj);
        }

        public async Task<List<Empresa>> ListarEmpresas(int pageIndex, int pageSize, string? nome = null, string? cnpj = null)
        {
            var query = dbContext.Empresa.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(p => p.RazaoSocial.ToLower().Contains(nome.ToLower()));
            }else if(!string.IsNullOrWhiteSpace(cnpj))
            {
                query = query.Where(p => p.Cnpj.ToLower().Contains(cnpj.ToLower()));
            }else if(!string.IsNullOrWhiteSpace(nome) && !string.IsNullOrWhiteSpace(cnpj))
            {
                var query_1 = query.Where(p => p.RazaoSocial.ToLower().Contains(nome.ToLower()));

                query = query_1.Where(p => p.Cnpj.ToLower().Contains(cnpj.ToLower()));
            }

            return await query
                .OrderBy(p => p.RazaoSocial)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        
        public async Task AdicionarUsuario(int usuarioid, string empresaid)
        {
            var usuario = dbContext.Usuario.Where(u => u.Id == usuarioid).FirstOrDefault();
            var empresa = dbContext.Empresa.Include(e => e.Usuarios).Where(e => e.Cnpj == empresaid).FirstOrDefault();


            if (empresa != null && usuario != null)
            {
                empresa.Usuarios.Add(usuario);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
        public async Task RemoverUsuario(int usuarioid, string empresaid)
        {
            var usuario = dbContext.Usuario.Where(u => u.Id == usuarioid).FirstOrDefault();
            var empresa = dbContext.Empresa.Include(e => e.Usuarios).Where(e => e.Cnpj == empresaid).FirstOrDefault();

            if (empresa != null && usuario != null)
            {
                empresa.Usuarios.Remove(usuario);
            }
            else
            {
                throw new KeyNotFoundException();
            }

        }        
        
        public async Task<List<Usuario>> ListarUsuarios(string cnpj, int pageIndex, int pageSize, string? nome)
        {
            var empresa = dbContext.Empresa.Include(e => e.Usuarios).Where(e => e.Cnpj == cnpj).FirstOrDefault();

            if (empresa != null)
            {
                var query = dbContext.Entry(empresa).Collection(e => e.Usuarios).Query();

                if (!string.IsNullOrWhiteSpace(nome))
                {
                    query = query.Where(u => u.Nome.ToLower().Contains(nome.ToLower()));
                }

                return await query
                .OrderBy(u => u.Nome)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
            {
                throw new KeyNotFoundException("A empresa n�o existe");
            }
        }
    }
}