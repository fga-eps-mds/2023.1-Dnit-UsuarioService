using dominio;

namespace repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        public UsuarioDnit ObterUsuario(string email);
    }
}
