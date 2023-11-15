using app.Services.Interfaces;
using app.Entidades;
using AutoMapper;
using app.Repositorios.Interfaces;
using api;

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
            var empresas = await empresaRepositorio.ListarEmpresas(pageIndex, pageSize, nome);

            return empresas;
        }

        public async Task<Empresa?> EditarEmpresa(string empresaid, Empresa empresa)
        {
            var empresaAtualizar = await empresaRepositorio.ObterEmpresaPorIdAsync(empresaid) ?? throw new KeyNotFoundException("Empresa não encontrada");

            if (empresaAtualizar != null)
            {
                empresaAtualizar.Cnpj = empresa.Cnpj;
                empresaAtualizar.RazaoSocial = empresa.RazaoSocial;
                empresaAtualizar.EmpresaUFs = empresa.EmpresaUFs;
                empresaAtualizar.Usuarios = empresa.Usuarios;
            }

            await dbContext.SaveChangesAsync();

            return empresaAtualizar;
        }

        public async Task AdicionarUsuario(int usuarioid, string empresaid)
        {
            await empresaRepositorio.AdicionarUsuario(usuarioid, empresaid);
            await dbContext.SaveChangesAsync();
        }

         public async Task RemoverUsuario(int usuarioid, string empresaid)
        {
            await empresaRepositorio.RemoverUsuario(usuarioid, empresaid);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Usuario>> ListarUsuarios(string cnpj, int pageIndex, int pageSize, string? nome)
        {
            var usuarios = await empresaRepositorio.ListarUsuarios(cnpj, pageIndex, pageSize, nome);

            return usuarios;
        }
    }
}