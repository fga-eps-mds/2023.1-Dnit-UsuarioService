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
        var sql = @"INSERT INTO public.usuario(@nome, @email, @senha) VALUES(usuario.nome, usuario.email, usuario.senha)";

        var parametros = new
        {
            Id = usuario.ID,
            Nome = usuario.nome,
            Email = usuario.email
        };

        // comando do Atualizar() do ItemRepositorio
        contexto?.Conexao.Execute(sql, parametros);
    }
    }
}