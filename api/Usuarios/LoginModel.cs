using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Usuarios
{
    public class LoginModel
    {
        public string Token { get; set; }
        public string TokenAtualizacao { get; set; }
        public DateTime ExpiraEm { get; set; }
        public List<Permissao> Permissoes { get; set; }
    }
}
