using System.ComponentModel.DataAnnotations;

namespace api.Empresa
{
    public class EmpresaDTO
    {
        [MaxLength(14)]
        public string Cnpj { get; set; }

        [Required, MaxLength(200)]
        public string RazaoSocial { get; set; }

        public List<UF>? UFs { get; set;}
    }
}