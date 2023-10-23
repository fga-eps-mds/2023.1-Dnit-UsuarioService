using api;
using System.ComponentModel.DataAnnotations;

namespace app.Entidades
{
    public class Perfil
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string Nome { get; set; }

        public List<PerfilPermissao>? PerfilPermissoes { get; set; }

        public IEnumerable<Permissao>? Permissoes => PerfilPermissoes?.Select(p => p.Permissao);

        public List<Usuario>? Usuarios { get; set; }
    }
}
