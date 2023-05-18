using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace service.Interfaces
{
    public interface IUsuarioService
    {

        public UsuarioDNIT Obter(int id);
    }
}
