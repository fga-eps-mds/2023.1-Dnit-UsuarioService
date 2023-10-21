using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Entidades
{
    public class RedefinicaoSenha
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Uuid { get; set; }

        [Required]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}