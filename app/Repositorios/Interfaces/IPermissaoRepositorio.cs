using api;

namespace app.Repositorios.Interfaces
{
    public interface IPermissaoRepositorio
    {
        public List<string> ObterCategorias();
        public List<string[]> ObterPermissoesPortCategoria(string categoria);
    }
}