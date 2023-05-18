using Dapper;
using dominio;
using dominio.Enums;
using repositorio.Contexto;
using repositorio.Interfaces;
using System.Collections.Generic;
using static repositorio.Contexto.ResolverContexto;

namespace repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly IContexto contexto;

        public UsuarioRepositorio(ResolverContextoDelegate resolverContexto)
        {
            contexto = resolverContexto(ContextoBancoDeDados.Postgresql);
        }

       

        public UsuarioDnit Obter(string email, string senha)
        {
            var sql = @"SELECT * FROM public.usuario WHERE email = @Email AND senha = @Senha ";


            var parametro = new
            {
                Email = email,
                Senha = senha
            };

            var usuarioDnit = contexto?.Conexao.QuerySingle<UsuarioDnit>(sql, parametro);

            return usuarioDnit;
        }

    }
}
