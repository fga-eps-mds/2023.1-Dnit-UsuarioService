using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IUnidadeFederativaRepositorio
    {
        IEnumerable<UnidadeFederativa> ObterDominio();
    }
}
