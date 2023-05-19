using dominio;

namespace repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        public UsuarioDnit Obter(string email);
    }
}
