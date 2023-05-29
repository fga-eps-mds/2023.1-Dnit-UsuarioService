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
            var sqlBuscarEmail = @"SELECT id, email, senha, nome FROM public.usuario WHERE email = @Email";

            var parametro = new
            {
                Email = email
            };

            var usuarioDnit = contexto?.Conexao.QuerySingleOrDefault<UsuarioDnit>(sqlBuscarEmail, parametro);

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

        public UsuarioDnit TrocarSenha(string email, string senha)
        {
            var sqlTrocarSenha = @"UPDATE public.usuario SET senha = @Senha WHERE email = @Email";

            var parametro = new
            {
                Email = email,
                Senha = senha
            };
            var usuarioDnit = contexto?.Conexao.QuerySingleOrDefault<UsuarioDnit>(sqlTrocarSenha, parametro);

            return usuarioDnit;
        }

        public int? ObterIdRedefinicaoSenha(string uuid)
        {
            var sqlBuscarDados = @"SELECT id FROM public.recuperacao_senha WHERE uuid = @Uuid";

            var parametro = new
            {
                Uuid = uuid,
            };

            int? IdUsuario = contexto?.Conexao.QuerySingleOrDefault<int>(sqlBuscarDados, parametro);

            return IdUsuario;
        }

        public RedefinicaoSenha InserirDadosRecuperacao(string uuid, int idUsuario)
        {
            var sqlInserirDadosRecuperacao = @"INSERT INTO public.recuperacao_senha(uuid, id_usuario) VALUES(@Uuid, @IdUsuario) RETURNING id";

            var parametro = new
            {
                Uuid = uuid,
                IdUsuario = idUsuario
            };

            var dadosRedefinicao = contexto?.Conexao.QuerySingleOrDefault<RedefinicaoSenha>(sqlInserirDadosRecuperacao, parametro);

            return dadosRedefinicao;
        }

    }
}
