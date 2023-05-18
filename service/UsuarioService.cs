using dominio;
using repositorio.Interfaces;
using repositorio.Contexto;
using service.Interfaces;


public class usuarioService : IUsuarioService
{
    private readonly IUsuarioRepositorio usuarioRepositorio;

    public usuarioService(IUsuarioRepositorio usuarioRepositorio)
    {
        this.usuarioRepositorio = usuarioRepositorio;
    }

    public UsuarioDnit Obter(string email, string senha)
    {
        UsuarioDnit usuarioDnit = usuarioRepositorio.Obter(email, senha);
        
        return usuarioDnit;
    }

    public void Login(string email, string senha) // tipo
    {
        email = emailtxt.Text; // emailtxt seria o campo de email do front-end
        senha = senhatxt.Text;

        UsuarioDnit usuarioDnit = usuarioRepositorio.Obter(email, senha);
    }

    public bool validaLogin() // parâmetros
    {
        //valida se a senha digitada confere com a do banco

    }
}