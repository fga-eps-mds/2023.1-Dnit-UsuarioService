using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class UsuarioDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public int? UF { get; set; }
        public string? CNPJ { get; set; }
    }
}
