using api;
using api.Usuarios;
using app.Entidades;
using app.Repositorios.Interfaces;
using app.Services;
using Microsoft.EntityFrameworkCore;

namespace app.Repositorios
{
    public class EmpresaRepositorio : IEmpresaRepositorio
    {

        private readonly AppDbContext dbContext;

        public EmpresaRepositorio(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CadastrarEmpresa(Empresa empresa)
        {
            dbContext.Add(empresa);
        }

        public Empresa? VisualizarEmpresa(string empresaid)
        {
            var empresa = dbContext.Empresa.Where(e => e.Cnpj == empresaid).FirstOrDefault();
            return empresa;
        }

        public async Task DeletarEmpresa(Empresa empresa)
        {
            var empresaParaExcluir = dbContext.Empresa.Where(e => e.Cnpj == empresa.Cnpj).FirstOrDefault() 
                ?? throw new ApiException(ErrorCodes.EmpresaNaoEncontrada);

            var query = dbContext.Entry(empresaParaExcluir).Collection(e => e.Usuarios).Query();
            var usuarios = await query.ToListAsync();

            foreach (var usuario in usuarios) 
            {
                empresaParaExcluir.Usuarios.Remove(usuario);
            }

            dbContext.Empresa.Remove(empresaParaExcluir);
        }

        public async Task<Empresa?> ObterEmpresaPorCnpjAsync(string Cnpj)
        {
            var query = dbContext.Empresa.AsQueryable();

            return await query.FirstOrDefaultAsync(p => p.Cnpj == Cnpj);
        }

        public async Task<ListaPaginada<Empresa>> ListarEmpresas(int pageIndex, int pageSize, List<UF> ufs, string? nome = null, string? cnpj = null)
        {
            var query = dbContext.Empresa.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(p => p.RazaoSocial.ToLower().Contains(nome.ToLower()));
            }
            else if (!string.IsNullOrWhiteSpace(cnpj))
            {
                query = query.Where(p => p.Cnpj.ToLower().Contains(cnpj.ToLower()));
            }
            else if (!string.IsNullOrWhiteSpace(nome) && !string.IsNullOrWhiteSpace(cnpj))
            {
                var query_1 = query.Where(p => p.RazaoSocial.ToLower().Contains(nome.ToLower()));

                query = query_1.Where(p => p.Cnpj.ToLower().Contains(cnpj.ToLower()));
            }

            if (ufs.Count > 0) {
                var queryUFs = await dbContext.EmpresaUFs.Where(e => ufs.Contains(e.Uf)).Select(e => e.EmpresaId).ToListAsync();
                query = query.Where(p => queryUFs.Contains(p.Cnpj));
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.RazaoSocial)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ListaPaginada<Empresa>(items, pageIndex, pageSize, total);
        }

        public void AdicionarUsuario(int usuarioid, string empresaid)
        {
            var usuario = dbContext.Usuario.Where(u => u.Id == usuarioid).FirstOrDefault()
                ?? throw new ApiException(ErrorCodes.UsuarioNaoEncontrado);

            var empresa = dbContext.Empresa.Include(e => e.Usuarios).Where(e => e.Cnpj == empresaid).FirstOrDefault()
                ?? throw new ApiException(ErrorCodes.EmpresaNaoEncontrada);

            empresa.Usuarios.Add(usuario);


        }

        public void RemoverUsuario(int usuarioid, string empresaid)
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

        public async Task<ListaPaginada<Usuario>> ListarUsuarios(string cnpj, PesquisaUsuarioFiltro filtro)
        {
            var empresa = dbContext.Empresa.Include(e => e.Usuarios).Where(e => e.Cnpj == cnpj).FirstOrDefault();

            if (empresa != null)
            {
                var query = dbContext.Entry(empresa).Collection(e => e.Usuarios).Query();

                if (!string.IsNullOrWhiteSpace(filtro.Nome))
                {
                    query = query.Where(u => u.Nome.ToLower().Contains(filtro.Nome.ToLower()));
                }

                if (filtro.PerfilId != null)
                    query = query.Where(u => u.PerfilId == filtro.PerfilId);

                if (filtro.UfLotacao != null)
                    query = query.Where(u => u.UfLotacao == filtro.UfLotacao);

                if (filtro.MunicipioId != null)
                    query = query.Where(u => u.MunicipioId == filtro.MunicipioId);

                var total = await query.CountAsync();
                var items = await query
                .Include(u => u.Perfil)
                .OrderBy(u => u.Nome)
                .Skip((filtro.Pagina - 1) * filtro.ItemsPorPagina)
                .Take(filtro.ItemsPorPagina)
                .ToListAsync();

                return new ListaPaginada<Usuario>(items, filtro.Pagina, filtro.ItemsPorPagina, total);
            }
            else
            {
                throw new KeyNotFoundException("A empresa não existe");
            }
        }
    }
}