using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IEmpresaRepositorio
    {
        Task CadastrarEmpresa(Empresa empresa);
        Empresa? VisualizarEmpresa(string empresaid);
        // Task DeletarEmpresa(string empresaid);
        // Task<Empresa> EditarEmpresa(string empresaid, Empresa empresa);
        // Task<List<Empresa>> ListarEmpresas(int pageIndex, int pageSize);
        // Task<List<Usuario>> ListarUsuarios(int pageIndex, int pageSize);
        // Task AdicionarUsuario(int usuarioid, string empresaid);
        // Task RemoverUsuario(int usuarioid, string empresaid);
    }
        
}