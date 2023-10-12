using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Entidades
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string nome { get; set; }

        [Required, MaxLength(50)]
        public string email { get; set; }
        
        [Required, MaxLength(200)]
        public string senha { get; set; }
    }
}