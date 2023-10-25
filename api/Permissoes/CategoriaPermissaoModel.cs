using api.Permissoes;

namespace api
{
    public class CategoriaPermissaoModel
    {
        public string Categoria { get; set; }
        public List<PermissaoModel> Permissoes { get; set; }
    }
}