using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace app.Entidades
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public UF UfLotacao { get; set; }

        [Required, MaxLength(150)]
        public string Nome { get; set; }

        [Required, MaxLength(50)]
        public string Email { get; set; }
        
        [Required, MaxLength(200)]
        public string Senha { get; set; }

        public List<RedefinicaoSenha> RedefinicaoSenha { get; set; }
        
        public List<Empresa> Empresas { get; set; }
    }
}