using app.Services.Interfaces;
using app.Entidades;
using AutoMapper;
using app.Repositorios.Interfaces;

namespace app.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepositorio empresaRepositorio;
        private readonly AppDbContext dbContext;
        public EmpresaService(IEmpresaRepositorio empresaRepositorio, AppDbContext dbContext)
        {
            this.empresaRepositorio = empresaRepositorio;
            this.dbContext = dbContext;
        }
        
        public async Task CadastrarEmpresa(Empresa empresa)
        {
            await empresaRepositorio.CadastrarEmpresa(empresa);

            await dbContext.SaveChangesAsync();
        }
    }
}