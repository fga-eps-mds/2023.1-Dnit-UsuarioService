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

        public Empresa? VisualizarEmpresa(string empresaid)
        {
            return empresaRepositorio.VisualizarEmpresa(empresaid);
        }

        public async Task DeletarEmpresa(string empresaid){
            var empresaParaExcluir = await empresaRepositorio.ObterEmpresaPorIdAsync(empresaid) ?? throw new KeyNotFoundException("Empresa não encontrada");

            await empresaRepositorio.DeletarEmpresa(empresaParaExcluir);
            dbContext.SaveChanges();
        }

        public async Task<List<Empresa>> ListarEmpresas(int pageIndex, int pageSize, string? nome = null)
        {
            var empresas = await empresaRepositorio.ListarEmpresas(pageIndex, pageSize);
          
            return empresas;
        }
    }
}