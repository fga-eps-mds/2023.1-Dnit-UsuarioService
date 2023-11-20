using api;
using api.Usuarios;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IEmpresaRepositorio
    {
        Task CadastrarEmpresa(Empresa empresa);
        Empresa? VisualizarEmpresa(string empresaid);
        Task DeletarEmpresa(Empresa empresa);
        public Task<Empresa?> ObterEmpresaPorCnpjAsync(string empresaid);
        Task<ListaPaginada<Empresa>> ListarEmpresas(int pageIndex, int pageSize, List<UF> ufs, string? nome = null, string? cnpj = null);
        Task<ListaPaginada<Usuario>> ListarUsuarios(string cnpj, PesquisaUsuarioFiltro filtro);
        Task AdicionarUsuario(int usuarioid, string empresaid);
        Task RemoverUsuario(int usuarioid, string empresaid);
    }
        
}