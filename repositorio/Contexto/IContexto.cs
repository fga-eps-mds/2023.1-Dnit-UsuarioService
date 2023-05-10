using System.Data;

namespace repositorio.Contexto
{
    public interface IContexto
    {
        IDbConnection Conexao { get; }
    }
}
