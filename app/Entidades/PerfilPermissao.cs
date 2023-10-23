using api;
using System.ComponentModel.DataAnnotations;

namespace app.Entidades
{
    public class PerfilPermissao
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PerfilId { get; set; }
        public Perfil Perfil { get; set; }

        [Required]
        public Permissao Permissao { get; set; }
    }
}
