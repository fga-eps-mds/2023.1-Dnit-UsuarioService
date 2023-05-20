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

    public UsuarioDnit Obter(UsuarioDnit usuarioDnit)
    {
        UsuarioDnit usuario = usuarioRepositorio.ObterUsuario(usuarioDnit.email);
        
        return usuario;
    }

    public bool validaLogin(UsuarioDnit primeiroUsuario) //primeiroUsuario: retorno do front; segundoUsuario: retorno do banco
    {
        UsuarioDnit segundoUsuario = Obter(primeiroUsuario);
       
        if (validaEmail(primeiroUsuario, segundoUsuario) && validaSenha(primeiroUsuario, segundoUsuario)) return true;
        else return false;

    }

    public bool validaEmail(UsuarioDnit primeiroUsuario, UsuarioDnit segundoUsuario)
    {
        if (segundoUsuario.email == primeiroUsuario.email) return true;
        else return false;
    }

    public bool validaSenha(UsuarioDnit primeiroUsuario, UsuarioDnit segundoUsuario)
    {
        if (segundoUsuario.senha == primeiroUsuario.senha) return true;
        else return false;
    }
}