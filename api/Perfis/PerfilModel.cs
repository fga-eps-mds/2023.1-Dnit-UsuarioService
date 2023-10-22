using api.Permissoes;

namespace api.Perfis
{
    public class PerfilModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public TipoPerfil Tipo { get; set; }
        public List<PermissaoModel> Permissoes { get; set; }
    }
}
