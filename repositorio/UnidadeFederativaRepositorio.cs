using Dapper;
using dominio;
using dominio.Enums;
using repositorio.Contexto;
using repositorio.Interfaces;
using System.Collections.Generic;
using System.Linq;
using static repositorio.Contexto.ResolverContexto;

namespace repositorio
{
    public class UnidadeFederativaRepositorio : IUnidadeFederativaRepositorio
    {
        private readonly IContexto contexto;
        public UnidadeFederativaRepositorio(ResolverContextoDelegate resolverContexto)
        {
            contexto = resolverContexto(ContextoBancoDeDados.Postgresql);
        }
        public IEnumerable<UnidadeFederativa> ObterDominio()
        {
            var sql = @"SELECT id, sigla, descricao FROM public.unidade_federativa";

            var unidadesFederativas = contexto?.Conexao.Query<UnidadeFederativa>(sql);

            return unidadesFederativas ?? Enumerable.Empty<UnidadeFederativa>();
        }
    }
}
