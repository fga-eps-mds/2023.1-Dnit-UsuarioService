using api;
using System.ComponentModel.DataAnnotations;

namespace app.Entidades
{
    public class Municipio
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Nome { get; set; }
        [Required]
        public UF Uf { get; set; }
    }
}