using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IEmpresaRepositorio
    {
        Task CadastrarEmpresa(Empresa empresa);
        Empresa? VisualizarEmpresa(string empresaid);
        Task DeletarEmpresa(Empresa empresa);
        public Task<Empresa?> ObterEmpresaPorIdAsync(string empresaid);
        Task<List<Empresa>> ListarEmpresas(int pageIndex, int pageSize, string? nome = null);
        // Task<List<Usuario>> ListarUsuarios(int pageIndex, int pageSize);
        Task AdicionarUsuario(int usuarioid, string empresaid);
        // Task RemoverUsuario(int usuarioid, string empresaid);
    }
        
}