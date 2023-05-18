using dominio;
using System.Collections.Generic;

namespace service.Interfaces
{
    public interface IUsuarioService
    {
        public void Login(string email, string senha);
        public bool validaLogin();
    }
}
