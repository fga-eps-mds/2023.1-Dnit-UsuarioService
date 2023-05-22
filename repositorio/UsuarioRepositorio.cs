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

        public void Cadastrar(UsuarioDNIT usuario)
        {
            var sql = @"INSERT INTO public.usuario(nome, email, senha) VALUES(@Nome, @Email, @Senha)";

            var parametros = new
            {
                Senha = usuario.senha,
                Nome = usuario.nome,
                Email = usuario.email
            };

            contexto?.Conexao.Execute(sql, parametros);
        }
    }
}