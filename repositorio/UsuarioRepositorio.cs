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

       
        public UsuarioDnit ObterUsuario(string email)
        {
            var sql = @"SELECT * FROM public.usuario WHERE email = @Email";


            var parametro = new
            {
                Email = email
            };

            var usuarioDnit = contexto?.Conexao.QuerySingleOrDefault<UsuarioDnit>(sql, parametro);

            if (usuarioDnit == null)
                return null;

            return usuarioDnit;
        }

        public void Cadastrar(UsuarioDnit usuario)
        {
            var sql = @"INSERT INTO public.usuario(nome, email, senha) VALUES(@Nome, @Email, @Senha)";

            var parametros = new
            {
                Senha = usuario.Senha,
                Nome = usuario.Nome,
                Email = usuario.Email
            };

            contexto?.Conexao.Execute(sql, parametros);
        }
    }
}
