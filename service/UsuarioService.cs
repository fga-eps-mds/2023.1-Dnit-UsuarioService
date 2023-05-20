using dominio;
using repositorio.Interfaces;
using repositorio.Contexto;
using service.Interfaces;


public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepositorio usuarioRepositorio;

    public UsuarioService(IUsuarioRepositorio usuarioRepositorio)
    {
        this.usuarioRepositorio = usuarioRepositorio;
    }

    public UsuarioDnit Obter(string email)
    {
        UsuarioDnit usuarioDnit = usuarioRepositorio.Obter(email);
        
        return usuarioDnit;
    }

    public bool validaLogin(UsuarioDnit primeiroUsuario) // parâmetros
    {
        UsuarioDnit segundoUsuario = Obter(primeiroUsuario.email);
        
        if (segundoUsuario.email == primeiroUsuario.email) return true;
        else{
            return false;
        }

    }
}