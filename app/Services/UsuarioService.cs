using api.Usuarios;
using api.Senhas;
using app.Repositorios.Interfaces;
using app.Services.Interfaces;
using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;
using app.Entidades;
using api;
using auth;
using Microsoft.Extensions.Options;
using app.Configuracoes;

namespace app.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuarioRepositorio usuarioRepositorio;
        private readonly IPerfilRepositorio perfilRepositorio;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly SenhaConfig senhaConfig;
        private readonly AppDbContext dbContext;
        private readonly AuthService autenticacaoService;
        private readonly AuthConfig authConfig;

        public UsuarioService
        (
            IUsuarioRepositorio usuarioRepositorio,
            IPerfilRepositorio perfilRepositorio,
            IMapper mapper,
            IEmailService emailService,
            IOptions<SenhaConfig> senhaConfig,
            AppDbContext dbContext,
            AuthService autenticacaoService,
            IOptions<AuthConfig> authConfig
        )
        {
            this.perfilRepositorio = perfilRepositorio;
            this.usuarioRepositorio = usuarioRepositorio;
            this.mapper = mapper;
            this.emailService = emailService;
            this.senhaConfig = senhaConfig.Value;
            this.dbContext = dbContext;
            this.autenticacaoService = autenticacaoService;
            this.authConfig = authConfig.Value;
        }

        public async Task CadastrarUsuarioDnit(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO.UfLotacao == 0) 
                throw new ApiException(ErrorCodes.CodigoUfInvalido);

            var usuario = mapper.Map<UsuarioDnit>(usuarioDTO);

            usuario.Senha = EncriptarSenha(usuario.Senha);

            await usuarioRepositorio.CadastrarUsuarioDnit(usuario);

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
            var usuario = await usuarioRepositorio.ObterUsuarioAsync(email: email, includePerfil: true)
                ?? throw new KeyNotFoundException();

            ValidaSenha(senha, usuario.Senha);

            return await CriarTokenAsync(usuario);
        }

        private Usuario? Obter(string email)
        {
            Usuario? usuario = usuarioRepositorio.ObterUsuarioPorEmail(email);

            if (usuario == null)
                throw new KeyNotFoundException();

            return usuario;
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

            Usuario usuarioBanco = Obter(usuarioEntrada.Email);

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
            var baseUrl = senhaConfig.RedefinirSenhaUrl;

            string link = $"{baseUrl}?token={UuidAutenticacao}";

            return link;
        }

        public async Task<LoginModel> AtualizarTokenAsync(AtualizarTokenDto atualizarTokenDto)
        {
            var usuarioId = autenticacaoService.GetUserId(atualizarTokenDto.Token);
            var usuario = await usuarioRepositorio.ObterUsuarioAsync(usuarioId, includePerfil: true);

            if (usuario?.TokenAtualizacao != atualizarTokenDto.TokenAtualizacao || !usuario.TokenAtualizacaoExpiracao.HasValue || usuario.TokenAtualizacaoExpiracao.Value <= DateTimeOffset.Now)
            {
                throw new AuthForbiddenException("Token expirado. Realize o login novamente.");
            }
            return await CriarTokenAsync(usuario);
        }

        private async Task<LoginModel> CriarTokenAsync(Usuario usuario)
        {
            var permissoes = usuario.Perfil?.Permissoes?.ToList(comInternas: true) ?? new();

            if (!authConfig.Enabled || usuario.Perfil?.Tipo == TipoPerfil.Administrador)
                permissoes = Enum.GetValues<Permissao>().ToList(comInternas: true);
                
            var (token, expiraEm) = autenticacaoService.GenerateToken(new AuthUserModel<Permissao>
            {
                Id = usuario.Id,
                Name = usuario.Nome,
                Permissions = permissoes,
                Administrador = usuario.Perfil?.Tipo == TipoPerfil.Administrador,
            });

            var (tokenAtualizacao, tokenAtualizacaoExpiracao) = autenticacaoService.GenerateRefreshToken();

            usuario.TokenAtualizacao = tokenAtualizacao;
            usuario.TokenAtualizacaoExpiracao = tokenAtualizacaoExpiracao;
            await dbContext.SaveChangesAsync();

            return new LoginModel()
            {
                Token = "Bearer " + token,
                ExpiraEm = expiraEm,
                TokenAtualizacao = tokenAtualizacao,
                Permissoes = permissoes,
            };
        }

        public async Task<List<Permissao>> ListarPermissoesAsync(int userId)
        {
            var usuario = await usuarioRepositorio.ObterUsuarioAsync(userId, includePerfil: true);
            if (usuario!.Perfil?.Tipo == TipoPerfil.Administrador || !authConfig.Enabled)
            {
                usuario.Perfil.PermissoesSessao = Enum.GetValues<Permissao>().ToList();
            }
            return usuario!.Perfil?.Permissoes?.ToList() ?? new();
        }

        public async Task<ListaPaginada<UsuarioModel>> ObterUsuariosAsync(PesquisaUsuarioFiltro filtro)
        {
            var usuarios = await usuarioRepositorio.ObterUsuariosAsync(filtro);
            var modelos = mapper.Map<List<UsuarioModel>>(usuarios.Items);
            return new ListaPaginada<UsuarioModel>(modelos, filtro.Pagina, filtro.ItemsPorPagina, usuarios.Total);
        }

        public async Task EditarUsuarioPerfil(int usuarioId, string novoPerfilId, UF novaUF, int novoMunicipio)
        {
            var usuario = await usuarioRepositorio.ObterUsuarioAsync(usuarioId)
                ?? throw new ApiException(ErrorCodes.UsuarioNaoEncontrado);

            var permissao = await perfilRepositorio.ObterPerfilPorIdAsync(Guid.Parse(novoPerfilId))
                ?? throw new ApiException(ErrorCodes.PermissaoNaoEncontrada);


            usuario.PerfilId = permissao.Id;
            usuario.UfLotacao = novaUF;
            usuario.MunicipioId = novoMunicipio;
            dbContext.SaveChanges();
        }

        public string ObterApiKey(int usuarioid)
        {
            var usuario = usuarioRepositorio.ObterUsuario(id: usuarioid)!;
            var (token, _) = autenticacaoService.GenerateToken(new AuthUserModel<Permissao>
            {
                Id = usuario.Id,
                Name = usuario.Nome,
                Permissions = usuario.Perfil?.Permissoes?.ToList() ?? new(),
                Administrador = usuario.Perfil?.Tipo == TipoPerfil.Administrador,
            },
            apiKey: usuario.Perfil?.Tipo == TipoPerfil.Administrador);
            return "Bearer " + token;
        }
    }
}
