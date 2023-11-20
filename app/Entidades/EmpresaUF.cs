using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api;

namespace app.Entidades
{
    public class EmpresaUF
    {
        public string EmpresaId { get; set; }
        [Key, Column(Order = 1)]
        public Empresa Empresa { get; set; }
        [Key, Column(Order = 2)]
        public UF Uf { get; set; }
    }
}