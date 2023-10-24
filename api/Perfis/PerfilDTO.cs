using System.ComponentModel.DataAnnotations;

namespace api.Perfis
{
    public class PerfilDTO
    {
        [MinLength(1)]
        public string Nome { get; set; }
        public List<Permissao> Permissoes { get; set; }
    }
}