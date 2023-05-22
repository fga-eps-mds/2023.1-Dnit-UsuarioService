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
            var sqlInserirUsuario = @"INSERT INTO public.usuario(nome, email, senha) VALUES(@Nome, @Email, @Senha) RETURNING id";

            var parametrosUsuario = new
            {
                Senha = usuario.Senha,
                Nome = usuario.Nome,
                Email = usuario.Email
            };

            int? usuarioId = contexto?.Conexao.ExecuteScalar<int>(sqlInserirUsuario, parametrosUsuario);

            if (usuarioId.HasValue)
            {
                var sqlInserirUnidadeFederativaUsuario = @"INSERT INTO 
                                                            public.usuario_unidade_federativa_lotacao(id_usuario, id_unidade_federativa) 
                                                            VALUES (@IdUsuario, @IdUnidadeFederativa)";
                var parametrosUnidadeFederativaUsuario = new
                {
                    IdUsuario = usuarioId,
                    IdUnidadeFederativa = usuario.UF
                };

                contexto?.Conexao.Execute(sqlInserirUnidadeFederativaUsuario, parametrosUnidadeFederativaUsuario);
            }
        }

    }
}
