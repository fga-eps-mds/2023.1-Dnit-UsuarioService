using Xunit;
using repositorio;
using repositorio.Interfaces;
using dominio;
using Microsoft.Data.Sqlite;
using Dapper;
using test.Stub;
using System;

namespace test
{
    public class UsuarioRepositorioTest : IDisposable
    {
        IUsuarioRepositorio repositorio;
        SqliteConnection conexao;

        public UsuarioRepositorioTest()
        {
            conexao = new SqliteConnection("Data Source=:memory:");
            conexao.Open();

            repositorio = new UsuarioRepositorio(contexto => new Contexto(conexao));

            string sql = @"
            CREATE TABLE public.usuario (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                email TEXT UNIQUE,
                senha TEXT,
                nome TEXT
            );

            CREATE TABLE public.usuario_unidade_federativa_lotacao (
                        id_usuario INTEGER REFERENCES usuario (id),
                        id_unidade_federativa INTEGER);

            CREATE TABLE public.usuario_empresa (
                        id_usuario INTEGER REFERENCES usuario (id),
                        cnpj_empresa INTEGER);

            CREATE TABLE public.recuperacao_senha (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        uuid TEXT,
                        id_usuario INTEGER REFERENCES usuario (id));
            ";

            conexao.Execute(sql);
        }

        [Fact]
        public void ObterUsuario_QuandoEmailForPassado_DeveRetornarUsuarioCorrespondente()
        {
            UsuarioStub usuarioStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            repositorio.CadastrarUsuarioDnit(usuarioDNIT);

            var usuarioObtido = repositorio.ObterUsuario("usuarioteste@gmail.com");

            Assert.Equal(usuarioDNIT.Email, usuarioObtido?.Email);
            Assert.Equal(usuarioDNIT.Senha, usuarioObtido?.Senha);
            Assert.Equal(usuarioDNIT.Nome, usuarioObtido?.Nome);
        }

        [Fact]
        public void CadastrarUsuarioDnit_QuandoUsuarioForPassado_DeveCadastrarUsuarioComDadosPassados()
        {
            UsuarioStub usuarioStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            repositorio.CadastrarUsuarioDnit(usuarioDNIT);

            var sql = $@"SELECT u.id, u.email, u.senha, u.nome, uufl.id_unidade_federativa uf
                         FROM public.usuario u
                         JOIN public.usuario_unidade_federativa_lotacao uufl
                            ON u.id = uufl.id_usuario
                         WHERE email = '{usuarioDNIT.Email}';";

            UsuarioDnit? usuarioObtido = conexao.QueryFirst<UsuarioDnit>(sql);

            Assert.Equal(usuarioDNIT.Email, usuarioObtido.Email);
            Assert.Equal(usuarioDNIT.Senha, usuarioObtido.Senha);
            Assert.Equal(usuarioDNIT.Nome, usuarioObtido.Nome);
            Assert.Equal(usuarioDNIT.UF, usuarioObtido.UF);
        }

        [Fact]
        public void TrocarSenha_QuandoNovaSenhaForPassada_DeveAtualizarSenhaDoUsuario()
        {
            UsuarioStub usuarioStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            repositorio.CadastrarUsuarioDnit(usuarioDNIT);

            string novaSenha = "NovaSenha";

            repositorio.TrocarSenha(usuarioDNIT.Email, novaSenha);

            var usuarioObtido = repositorio.ObterUsuario(usuarioDNIT.Email);

            Assert.Equal(novaSenha, usuarioObtido?.Senha);
        }

        [Fact]
        public void ObterEmailRedefinicaoSenha_QuandoUuidForPassado_DeveRetornarEmailCorrespondente()
        {
            UsuarioStub usuarioStub = new();
            RedefinicaoSenhaStub redefinicaoSenhaStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();
            var redefinicaoSenha = redefinicaoSenhaStub.ObterRedefinicaoSenha();

            repositorio.CadastrarUsuarioDnit(usuarioDNIT);
            var usuarioObtido = repositorio.ObterUsuario(usuarioDNIT.Email);

            repositorio.InserirDadosRecuperacao(redefinicaoSenha.UuidAutenticacao, usuarioObtido!.Id);

            var email = repositorio.ObterEmailRedefinicaoSenha(redefinicaoSenha.UuidAutenticacao);

            Assert.Equal(usuarioDNIT.Email, email);
        }

        [Fact]
        public void RemoverUuidRedefinicaoSenha_QuandoUuidForPassado_DeveRemoverUuidDoBanco()
        {
            UsuarioStub usuarioStub = new();
            RedefinicaoSenhaStub redefinicaoSenhaStub = new();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();
            var redefinicaoSenha = redefinicaoSenhaStub.ObterRedefinicaoSenha();

            repositorio.CadastrarUsuarioDnit(usuarioDNIT);
            var usuarioObtido = repositorio.ObterUsuario(usuarioDNIT.Email);

            repositorio.InserirDadosRecuperacao(redefinicaoSenha.UuidAutenticacao, usuarioObtido!.Id);
            repositorio.RemoverUuidRedefinicaoSenha(redefinicaoSenha.UuidAutenticacao);

            var email = repositorio.ObterEmailRedefinicaoSenha(redefinicaoSenha.UuidAutenticacao);

            Assert.Null(email);
        }


        [Fact]
        public void CadastrarUsuarioTerceiro_QuandoUsuarioForPassado_DeveCadastrarUsuarioComDadosPassados()
        {
            UsuarioStub usuarioStub = new();
            var usuarioTerceiro = usuarioStub.RetornarUsuarioTerceiro();

            repositorio.CadastrarUsuarioTerceiro(usuarioTerceiro);

            var sql = $@"SELECT u.id, u.email, u.senha, u.nome, ue.cnpj_empresa cnpj
                         FROM public.usuario u
                         JOIN public.usuario_empresa ue
                            ON u.id = ue.id_usuario
                         WHERE email = '{usuarioTerceiro.Email}';";

            UsuarioTerceiro? usuarioObtido = conexao.QueryFirst<UsuarioTerceiro>(sql);

            Assert.Equal(usuarioTerceiro.Email, usuarioObtido.Email);
            Assert.Equal(usuarioTerceiro.Senha, usuarioObtido.Senha);
            Assert.Equal(usuarioTerceiro.Nome, usuarioObtido.Nome);
            Assert.Equal(usuarioTerceiro.CNPJ, usuarioObtido.CNPJ);
        }
        public void Dispose()
        {
            conexao.Close();
            conexao.Dispose();
        }
    }
}
