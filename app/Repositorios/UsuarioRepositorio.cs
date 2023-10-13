using app.Entidades;
using Dapper;
using api.Usuarios;
using api.Senhas;
using app.Repositorios.Interfaces;

namespace app.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly AppDbContext dbContext;

        public UsuarioRepositorio(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
       
        public UsuarioModel? ObterUsuario(string email)
        {
            // !! Não ajustado ainda, retorna objeto vazio;

            /* var sqlBuscarEmail = @"SELECT id, email, senha, nome FROM public.usuario WHERE email = @Email";

            var parametro = new
            {
                Email = email
            };

            var usuario = contexto?.Conexao.QuerySingleOrDefault<Usuario>(sqlBuscarEmail, parametro); */

            var mock = new UsuarioModel();

            return mock;
        }

        public void CadastrarUsuarioDnit(UsuarioDnit usuario)
        {

            // !! Método não ajustado

            /* var sqlInserirUsuario = @"INSERT INTO public.usuario(nome, email, senha) VALUES(@Nome, @Email, @Senha) RETURNING id";

            var parametrosUsuario = new
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
            };

            int? usuarioId = contexto?.Conexao.ExecuteScalar<int?>(sqlInserirUsuario, parametrosUsuario);

            var sqlInserirUnidadeFederativaUsuario = @"INSERT INTO 
                                                        public.usuario_unidade_federativa_lotacao(id_usuario, id_unidade_federativa) 
                                                        VALUES (@IdUsuario, @IdUnidadeFederativa)";
            var parametrosUnidadeFederativaUsuario = new
            {
                IdUsuario = usuarioId,
                IdUnidadeFederativa = usuario.UF
            };

            contexto?.Conexao.Execute(sqlInserirUnidadeFederativaUsuario, parametrosUnidadeFederativaUsuario); */
        }

        public UsuarioDnit TrocarSenha(string email, string senha)
        {
            // !! Não ajustado ainda, retorna objeto vazio;

            /* var sqlTrocarSenha = @"UPDATE public.usuario SET senha = @Senha WHERE email = @Email";

            var parametro = new
            {
                Email = email,
                Senha = senha
            };
            var usuarioDnit = contexto?.Conexao.QuerySingleOrDefault<UsuarioDnit>(sqlTrocarSenha, parametro);*/

            var mock = new UsuarioDnit();

            return mock;
        }

        public string? ObterEmailRedefinicaoSenha(string uuid)
        {
            // !! Não ajustado ainda, retorna string generica;

            /* var sqlBuscarDados = @"SELECT u.email FROM public.RedefinicaoSenha rs INNER JOIN public.usuario u ON rs.id_usuario = u.id WHERE uuid = @Uuid";

            var parametro = new
            {
                Uuid = uuid,
            };

            string? email = contexto?.Conexao.QuerySingleOrDefault<string>(sqlBuscarDados, parametro); */

            var mock = "foo";

            return mock;
        }

        public void RemoverUuidRedefinicaoSenha(string uuid)
        {
            // !! Método não ajustado

            /* var sqlBuscarDados = @"DELETE FROM public.RedefinicaoSenha WHERE uuid = @Uuid";

            var parametro = new
            {
                Uuid = uuid,
            };

            contexto?.Conexao.Execute(sqlBuscarDados, parametro); */
        }

        public RedefinicaoSenhaModel InserirDadosRecuperacao(string uuid, int idUsuario)
        {
            // !! Não ajustado ainda, retorna objeto vazio;

            /* var sqlInserirDadosRecuperacao = @"INSERT INTO public.RedefinicaoSenha(uuid, id_usuario) VALUES(@Uuid, @IdUsuario) RETURNING id";

            var parametro = new
            {
                Uuid = uuid,
                IdUsuario = idUsuario
            };

            var dadosRedefinicao = contexto?.Conexao.QuerySingleOrDefault<RedefinicaoSenha>(sqlInserirDadosRecuperacao, parametro); */

            var mock = new RedefinicaoSenhaModel();

            return mock;
        }

        public void CadastrarUsuarioTerceiro(UsuarioTerceiro usuarioTerceiro)
        {
            // !! Método não ajustado

            /* var sqlInserirUsuarioTerceiro = @"INSERT INTO public.usuario(nome, email, senha) VALUES(@Nome, @Email, @Senha) RETURNING id";

            var parametrosUsuarioTerceiro = new
            {
                Nome = usuarioTerceiro.Nome,
                Email = usuarioTerceiro.Email,
                Senha = usuarioTerceiro.Senha
            };

            int? usuarioTerceiroId = contexto?.Conexao.ExecuteScalar<int?>(sqlInserirUsuarioTerceiro, parametrosUsuarioTerceiro);

            var sqlInserirEmpresa = @"INSERT INTO public.usuario_empresa(id_usuario, cnpj_empresa) VALUES(@IdUsuario, @CnpjEmpresa)";

            var parametrosEmpresa = new
            {
                IdUsuario = usuarioTerceiroId,
                CnpjEmpresa = usuarioTerceiro.CNPJ
            };

            contexto?.Conexao.Execute(sqlInserirEmpresa, parametrosEmpresa); */
        }
    }
}
