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

        public async Task<List<Empresa>> ListarEmpresas(int pageIndex, int pageSize, string? nome = null)
        {
            var query = dbContext.Empresa.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(p => p.RazaoSocial.ToLower().Contains(nome.ToLower()));
            }

            return await query
                .OrderBy(p => p.RazaoSocial)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}