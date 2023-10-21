using api.Perfis;
using app.Entidades;
using api;

namespace app.Services.Interfaces
{
    public interface IPerfilService
    {
        public Perfil CriarPerfil(Perfil perfil, List<Permissao> permissoes);
        public Task<Perfil> EditarPerfil(Perfil perfil, List<Permissao> permissoes);
        public Task ExcluirPerfil(Guid id);
        public Task<List<Perfil>> ListarPerfisAsync(int pageIndex, int pageSize);
    }
}