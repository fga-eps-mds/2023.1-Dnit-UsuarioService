using app.Services.Interfaces;
using app.Entidades;
using AutoMapper;
using app.Repositorios.Interfaces;
using api;
using api.Empresa;
using api.Usuarios;

namespace app.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepositorio empresaRepositorio;
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        public EmpresaService(IEmpresaRepositorio empresaRepositorio, AppDbContext dbContext, IMapper mapper)
        {
            this.empresaRepositorio = empresaRepositorio;
            this.dbContext = dbContext;
            this.mapper = mapper;
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

        public async Task<ListaPaginada<EmpresaModel>> ListarEmpresas(int pageIndex, int pageSize, string? nome = null, string? cnpj = null)
        {
            var pagina = await empresaRepositorio.ListarEmpresas(pageIndex, pageSize, nome, cnpj);
            var modelo = mapper.Map<List<EmpresaModel>>(pagina.Items);
            return new ListaPaginada<EmpresaModel>(modelo, pagina.Pagina, pagina.ItemsPorPagina, pagina.Total);
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

        public async Task<ListaPaginada<UsuarioModel>> ListarUsuarios(string cnpj, int pageIndex, int pageSize, string? nome)
        {
            var pagina = await empresaRepositorio.ListarUsuarios(cnpj, pageIndex, pageSize, nome);
            var modelo = mapper.Map<List<UsuarioModel>>(pagina.Items);
            return new ListaPaginada<UsuarioModel>(modelo, pagina.Pagina, pagina.ItemsPorPagina, pagina.Total);
        }
    }
}