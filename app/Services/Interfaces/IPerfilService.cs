using app.Entidades;
using api;

namespace app.Services.Interfaces
{
    public interface IPerfilService
    {
        Perfil CriarPerfil(Perfil perfil, List<Permissao> permissoes);
        Task<Perfil> EditarPerfil(Perfil perfil, List<Permissao> permissoes);
        Task ExcluirPerfil(Guid id);
        Task<List<Perfil>> ListarPerfisAsync(int pageIndex, int pageSize, string? nome);
        Task<Perfil?> ObterPorIdAsync(Guid id);
    }
}