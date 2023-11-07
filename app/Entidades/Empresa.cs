using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api;

namespace app.Entidades
{
    public class Empresa
    {
        [Key, MaxLength(14)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Cnpj { get; set; }

        [Required, MaxLength(200)]
        public string RazaoSocial { get; set; }

        public List<Usuario> Usuarios { get; set; } = new();

        public List<UF> UFs { get; set;}
    }
}