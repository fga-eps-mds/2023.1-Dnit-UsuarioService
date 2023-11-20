using api;
using api.Usuarios;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IEmpresaRepositorio
    {
        void CadastrarEmpresa(Empresa empresa);
        Empresa? VisualizarEmpresa(string empresaid);
        Task DeletarEmpresa(Empresa empresa);
        public Task<Empresa?> ObterEmpresaPorCnpjAsync(string empresaid);
        Task<ListaPaginada<Empresa>> ListarEmpresas(int pageIndex, int pageSize, List<UF> ufs, string? nome = null, string? cnpj = null);
        Task<ListaPaginada<Usuario>> ListarUsuarios(string cnpj, PesquisaUsuarioFiltro filtro);
        void AdicionarUsuario(int usuarioid, string empresaid);
        void RemoverUsuario(int usuarioid, string empresaid);
    }
        
}