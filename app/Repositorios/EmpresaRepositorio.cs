using app.Entidades;
using app.Repositorios.Interfaces;

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
    }
}