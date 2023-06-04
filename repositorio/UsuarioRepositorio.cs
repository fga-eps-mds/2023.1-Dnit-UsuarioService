using Dapper;
using dominio;
using dominio.Enums;
using repositorio.Contexto;
using repositorio.Interfaces;
using System;
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

       
        public Usuario? ObterUsuario(string email)
        {
            var sqlBuscarEmail = @"SELECT id, email, senha, nome FROM public.usuario WHERE email = @Email";

            var parametro = new
            {
                Email = email
            };

            var usuario = contexto?.Conexao.QuerySingleOrDefault<Usuario>(sqlBuscarEmail, parametro);

            return usuario;
        }

        public void CadastrarUsuarioDnit(UsuarioDnit usuario)
        {
            var sqlInserirUsuario = @"INSERT INTO public.usuario(nome, email, senha) VALUES(@Nome, @Email, @Senha) RETURNING id";

            var parametrosUsuario = new
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
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
            else
            {
                throw new InvalidOperationException("Email já cadastrado.");
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

        public string? ObterEmailRedefinicaoSenha(string uuid)
        {
            var sqlBuscarDados = @"SELECT u.email FROM public.recuperacao_senha rs INNER JOIN public.usuario u ON rs.id_usuario = u.id WHERE uuid = @Uuid";

            var parametro = new
            {
                Uuid = uuid,
            };

            string? email = contexto?.Conexao.QuerySingleOrDefault<string>(sqlBuscarDados, parametro);

            return email;
        }

        public void RemoverUuidRedefinicaoSenha(string uuid)
        {
            var sqlBuscarDados = @"DELETE FROM public.recuperacao_senha WHERE uuid = @Uuid";

            var parametro = new
            {
                Uuid = uuid,
            };

            contexto?.Conexao.Execute(sqlBuscarDados, parametro);
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

        public void CadastrarUsuarioTerceiro(UsuarioTerceiro usuarioTerceiro)
        {
            var sqlInserirUsuarioTerceiro = @"INSERT INTO public.usuario(nome, email, senha) VALUES(@Nome, @Email, @Senha) RETURNING id";

            var parametrosUsuarioTerceiro = new
            {
                Nome = usuarioTerceiro.Nome,
                Email = usuarioTerceiro.Email,
                Senha = usuarioTerceiro.Senha
            };

            int? usuarioTerceiroId = contexto?.Conexao.ExecuteScalar<int>(sqlInserirUsuarioTerceiro, parametrosUsuarioTerceiro);

            if (usuarioTerceiroId.HasValue)
            {
                var sqlInserirEmpresa = @"INSERT INTO public.usuario_empresa(id_usuario, cnpj_empresa) VALUES(@IdUsuario, @CnpjEmpresa)";

                var parametrosEmpresa = new
                {
                    IdUsuario = usuarioTerceiroId,
                    CnpjEmpresa = usuarioTerceiro.CNPJ
                };

                contexto?.Conexao.Execute(sqlInserirEmpresa, parametrosEmpresa);
            }
            else
            {
                throw new InvalidOperationException("Email já cadastrado.");
            }
        }

    }
}
