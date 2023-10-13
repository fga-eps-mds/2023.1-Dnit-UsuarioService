using Dapper;
using app.Entidades;
using api;
using app.Repositorios.Interfaces;

namespace app.Repositorios
{
    public class UnidadeFederativaRepositorio : IUnidadeFederativaRepositorio
    {
        private readonly AppDbContext dbContext;
        public UnidadeFederativaRepositorio(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<UfModel> ObterDominio()
        {
            //Não ajustada ainda, retorna lista vazia
            
            var mock = new List<UfModel>();

            return mock;
        }
    }
}
