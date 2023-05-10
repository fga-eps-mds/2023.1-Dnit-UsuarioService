using dominio.Enums;

namespace repositorio.Contexto
{
    public class ResolverContexto
    {
        public delegate IContexto? ResolverContextoDelegate(ContextoBancoDeDados contexto);
    }
}
