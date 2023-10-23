using api;

namespace app.Repositorios.Interfaces
{
    public interface IUnidadeFederativaRepositorio
    {
        IEnumerable<UfModel> ObterDominio();
    }
}
