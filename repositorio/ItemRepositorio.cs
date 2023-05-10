using Dapper;
using dominio;
using dominio.Enums;
using repositorio.Contexto;
using repositorio.Interfaces;
using System.Collections.Generic;
using static repositorio.Contexto.ResolverContexto;

namespace repositorio
{
    public class ItemRepositorio : IItemRepositorio
    {
        private readonly IContexto contexto;

        public ItemRepositorio(ResolverContextoDelegate resolverContexto)
        {
            contexto = resolverContexto(ContextoBancoDeDados.Postgresql);
        }

        public void Atualizar(Item item)
        {
            var sql = "UPDATE public.item SET nome = @Nome, status = @Status WHERE id = @Id";

            var parametros = new
            {
                Nome = item.Nome,
                Status = item.Status,
                Id = item.Id
            };

            contexto?.Conexao.Execute(sql, parametros);
        }

        public Item Obter(int id)
        {
            var sql = @"SELECT id, nome, status FROM public.item WHERE id = @Id";

            var parametro = new
            {
                Id = id,
            };

            var item = contexto?.Conexao.QuerySingle<Item>(sql, parametro);

            return item;
        }

        public IEnumerable<Item> ObterLista()
        {
            var sql = @"SELECT id, nome, status FROM public.item";

            var listaItens = contexto?.Conexao.Query<Item>(sql);

            return listaItens ?? new List<Item>();
        }

    }
}
