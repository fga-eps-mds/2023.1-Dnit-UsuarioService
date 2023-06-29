using System.Data;
using repositorio.Contexto;
using Dapper;

namespace test
{
    public class Contexto : IContexto
    {
        public IDbConnection Conexao { get; }
        public Contexto(IDbConnection conexao)
        {
            Conexao = conexao;

            string sql = @"
                ATTACH DATABASE ':memory:' AS public;
            ";

            Conexao.Execute(sql);
        }
    }
}
