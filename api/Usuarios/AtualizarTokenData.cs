using System.ComponentModel.DataAnnotations;

namespace api.Usuarios
{
    public class AtualizarTokenDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string TokenAtualizacao { get; set; }
    }
}
