using api;

namespace app.Services.Interfaces
{
    public interface IPermissaoService
    {
        List<CategoriaPermissaoModel> CategorizarPermissoes(List<Permissao> permissaos);
        List<string> ObterCategorias();
    }
}