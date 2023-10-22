using api;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public List<Permissao>? PermissoesSessao { get; set; }

        [NotMapped]
        public IEnumerable<Permissao>? Permissoes => PermissoesSessao ?? PerfilPermissoes?.Select(p => p.Permissao);

        public List<Usuario>? Usuarios { get; set; }
    }
}
