using api;
using api.Perfis;
using app.Entidades;

namespace app.Repositorios.Interfaces
{
    public interface IPerfilRepositorio
    {
        public Perfil RegistraPerfil(Perfil perfil);
        public PerfilPermissao AdicionaPermissaoAoPerfil(Guid perfilId, Permissao permissao);
        public void RemovePerfil(Perfil perfil);
        public void RemovePermissaoDoPerfil(PerfilPermissao perfilPermissao);
        public Task<Perfil?> ObterPerfilPorIdAsync(Guid id);
        public Task<List<Perfil>> ListarPerfisAsync(int pageIndex, int pageSize, string? nome = null);
    }
}