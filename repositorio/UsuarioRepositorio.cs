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
        }

        public void CadastrarTerceiro(UsuarioTerceiro usuarioTerceiro)
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
                    CnpjEmpresa = usuarioTerceiro.CNPJempresa
                };

                contexto?.Conexao.Execute(sqlInserirEmpresa, parametrosEmpresa);
            }
        }

    }
}
