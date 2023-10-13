using api;
using System.Collections.Generic;

namespace app.Repositorios.Interfaces
{
    public interface IUnidadeFederativaRepositorio
    {
        IEnumerable<UfModel> ObterDominio();
    }
}
