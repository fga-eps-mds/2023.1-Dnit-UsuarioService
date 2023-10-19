using api.Usuarios;
using api.Senhas;
using app.Repositorios.Interfaces;
using app.Services.Interfaces;
using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;
using app.Entidades;
using api;
using auth;

namespace app.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuarioRepositorio usuarioRepositorio;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;
        private readonly AppDbContext dbContext;
        private readonly AuthService autenticacaoService;

        public UsuarioService
        (
            IUsuarioRepositorio usuarioRepositorio, 
            IMapper mapper, 
            IEmailService emailService, 
            IConfiguration configuration,
            AppDbContext dbContext,
            AuthService autenticacaoService
        )
        {
            this.usuarioRepositorio = usuarioRepositorio;
            this.mapper = mapper;
            this.emailService = emailService;
            this.configuration = configuration;
            this.dbContext = dbContext;
            this.autenticacaoService = autenticacaoService;
        }

        public async Task CadastrarUsuarioDnit(UsuarioDTO usuarioDTO)
        {
            var usuario = mapper.Map<UsuarioDnit>(usuarioDTO);

            usuario.Senha = EncriptarSenha(usuario.Senha);

            usuarioRepositorio.CadastrarUsuarioDnit(usuario);

            await dbContext.SaveChangesAsync();
        }

        private string EncriptarSenha(string senha)
        {
            string salt = BCryptNet.GenerateSalt();

            return BCryptNet.HashPassword(senha, salt);
        }

        public void CadastrarUsuarioTerceiro(UsuarioDTO usuarioDTO)
        {
            var usuario = mapper.Map<UsuarioTerceiro>(usuarioDTO);

            usuario.Senha = EncriptarSenha(usuario.Senha);

            usuarioRepositorio.CadastrarUsuarioTerceiro(usuario);
        }

        public async Task<LoginModel> AutenticarUsuarioAsync(string email, string senha)
        {
            var usuario = await usuarioRepositorio.ObterUsuarioAsync(email, includePerfil: true)
                ?? throw new KeyNotFoundException();

            ValidaSenha(senha, usuario.Senha);

            var (token, expiraEm) = autenticacaoService.GenerateToken(new AuthUserModel<Permissao>
            {
                Id = usuario.Id,
                Name = usuario.Nome,
                Permissions = usuario.Perfil?.Permissoes?.ToList(),
            });

            return new LoginModel()
            {
                Token = token,
                ExpiraEm = expiraEm,
                TokenAtualizacao = "",
                Permissoes = usuario.Perfil?.Permissoes?.ToList(),
            };
        }

        private UsuarioModel? Obter(string email)
        {
            UsuarioModel? usuario = usuarioRepositorio.ObterUsuario(email);

            if (usuario == null)
                throw new KeyNotFoundException();

            return usuario;
        }

        public bool ValidaLogin(UsuarioDTO usuarioDTO)
        {
            UsuarioModel? usuarioBanco = Obter(usuarioDTO.Email);

            return ValidaSenha(usuarioDTO.Senha, usuarioBanco.Senha);
        }

        private bool ValidaSenha(string senhaUsuarioEntrada, string senhaUsuarioBanco)
        {
            if (BCryptNet.Verify(senhaUsuarioEntrada, senhaUsuarioBanco))
                return true;

            throw new UnauthorizedAccessException();
        }

        public async Task TrocaSenha(RedefinicaoSenhaDTO redefinicaoSenhaDTO)
        {
            RedefinicaoSenhaModel dadosRedefinicaoSenha = mapper.Map<RedefinicaoSenhaModel>(redefinicaoSenhaDTO);

            string emailUsuario = usuarioRepositorio.ObterEmailRedefinicaoSenha(dadosRedefinicaoSenha.UuidAutenticacao) ?? throw new KeyNotFoundException();
            string senha = EncriptarSenha(dadosRedefinicaoSenha.Senha);

            usuarioRepositorio.TrocarSenha(emailUsuario, senha);

            emailService.EnviarEmail(emailUsuario, "Senha Atualizada", "A sua senha foi atualizada com sucesso.");

            usuarioRepositorio.RemoverUuidRedefinicaoSenha(dadosRedefinicaoSenha.UuidAutenticacao);

            await dbContext.SaveChangesAsync();
        }

        public async Task RecuperarSenha(UsuarioDTO usuarioDTO)
        {
            var usuarioEntrada = mapper.Map<UsuarioDnit>(usuarioDTO);

            UsuarioModel usuarioBanco = Obter(usuarioEntrada.Email);

            string UuidAutenticacao = Guid.NewGuid().ToString();

            usuarioRepositorio.InserirDadosRecuperacao(UuidAutenticacao, usuarioBanco.Id);

            string mensagem = "Olá!!\n\n" +
                              "Recebemos uma solicitação para redefinir a sua senha.\n\n" +
                              "Clique no link abaixo para ser direcionado a página de Redefinição de Senha.\n\n" +
                              $"{GerarLinkDeRecuperacao(UuidAutenticacao)}";

            emailService.EnviarEmail(usuarioBanco.Email, "Link de Recuperação", mensagem);

            await dbContext.SaveChangesAsync();
        }
        private string GerarLinkDeRecuperacao(string UuidAutenticacao)
        {
            var baseUrl = configuration["RedefinirSenhaUrl"];

            string link = $"{baseUrl}?token={UuidAutenticacao}";

            return link;
        }
    }
}
