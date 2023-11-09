using app.Entidades;


namespace app.Services.Interfaces
{
    public interface IEmpresaService
    {
        Task CadastrarEmpresa(Empresa empresa);
        Empresa? VisualizarEmpresa(string empresaid);
        Task DeletarEmpresa(string empresaid);
        
        Task<Empresa?> EditarEmpresa(string empresaid, Empresa empresa);
        Task<List<Empresa>> ListarEmpresas(int pageIndex, int pageSize, string? nome = null);
        // Task<List<Usuario>> ListarUsuarios(int pageIndex, int pageSize);
        Task AdicionarUsuario(int usuarioid, string empresaid);
        Task RemoverUsuario(int usuarioid, string empresaid);
    }
}