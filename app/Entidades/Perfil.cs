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

        [Required]
        public TipoPerfil Tipo { get; set; } = TipoPerfil.Customizavel;

        public List<PerfilPermissao>? PerfilPermissoes { get; set; }

        public IEnumerable<Permissao>? Permissoes => PerfilPermissoes?.Select(p => p.Permissao);

        public List<Usuario>? Usuarios { get; set; }
    }
}
