using api.Permissoes;

namespace app.Services.Interfaces
{
    public interface IPermissaoService
    {
        public List<string> ObterCategorias();
        public List<PermissaoModel> ObterPermissoesPortCategoria(string categoria);
    }
}