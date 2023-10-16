using Dapper;
using app.Entidades;
using api;
using app.Repositorios.Interfaces;
using AutoMapper;

namespace app.Repositorios
{
    public class UnidadeFederativaRepositorio : IUnidadeFederativaRepositorio
    {
        private readonly IMapper mapper;
        public UnidadeFederativaRepositorio(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public IEnumerable<UfModel> ObterDominio()
        {
            return Enum.GetValues<UF>().Select(uf => mapper.Map<UfModel>(uf)).OrderBy(uf => uf.Sigla);;
        }
    }
}
