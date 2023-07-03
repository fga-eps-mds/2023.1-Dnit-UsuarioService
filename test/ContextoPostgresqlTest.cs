using Npgsql;
using repositorio.Contexto;
using System.Data;
using Xunit;

namespace test
{
    public class ContextoPostgresqlTest
    {
        [Fact]
        public void ContextoPostgresql_QuandoForInstanciado_DeveSerConfiguradoCorretamente()
        {
            var connectionString = "Host=localhost;Port=1234;Database=postgres;Username=usuario;Password=teste";

            using (var contexto = new ContextoPostgresql(connectionString))
            {
                Assert.NotNull(contexto.Conexao);
                Assert.IsType<NpgsqlConnection>(contexto.Conexao);
                Assert.Equal(ConnectionState.Closed, contexto.Conexao.State);
            }
        }
    }
}
