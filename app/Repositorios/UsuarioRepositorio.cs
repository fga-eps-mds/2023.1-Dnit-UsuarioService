using Microsoft.EntityFrameworkCore;
using AutoMapper;
using app.Entidades;
using api.Usuarios;
using app.Repositorios.Interfaces;
using api;

namespace app.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        public UsuarioRepositorio(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public Usuario? ObterUsuario(string? email = null, int? id = null, bool includePerfil = false)
        {
            var query = dbContext.Usuario.AsQueryable();

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(u => u.Email.ToLower() == email.ToLower());
            }
            if (id.HasValue)
            {
                query = query.Where(u => u.Id == id);
            }
            if (includePerfil)
            {
                query = query.Include(u => u.Perfil);
            }
            return query.FirstOrDefault();
        }

        public Usuario? ObterUsuarioPorEmail(string email)
        {
            return ObterUsuario(email: email);
        }

        public async Task<Usuario?> ObterUsuarioAsync(int? id = null, string? email = null, bool includePerfil = false)
        {
            var query = dbContext.Usuario.AsQueryable();

            if (includePerfil)
            {
                query = query.Include(u => u.Perfil).ThenInclude(p => p.PerfilPermissoes);
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(u => u.Email.ToLower() == email!.ToLower());
            }
            if (id.HasValue)
            {
                query = query.Where(u => u.Id == id);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task CadastrarUsuarioDnit(UsuarioDnit usuario)
        {
            var novoUsuario = new Usuario
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                UfLotacao = usuario.UfLotacao,
                Perfil = await RecuperaPerfilBasicoAsync()
            };

            dbContext.Add(novoUsuario);
        }

        public async Task CadastrarUsuarioTerceiro(UsuarioTerceiro usuarioTerceiro)
        {
            var empresa = dbContext.Empresa.Where(e => e.Cnpj == usuarioTerceiro.CNPJ).FirstOrDefault();

            var novoUsuarioTerceiro = new Usuario
            {
                Nome = usuarioTerceiro.Nome,
                Email = usuarioTerceiro.Email,
                Senha = usuarioTerceiro.Senha,
                Perfil = await RecuperaPerfilBasicoAsync(),
                Associacao = Associacao.Empresa
            };

            if (empresa != null){
                novoUsuarioTerceiro.Empresa = empresa;
            }

            dbContext.Usuario.Add(novoUsuarioTerceiro);
        }

        public UsuarioModel? TrocarSenha(string email, string senha)
        {
            var usuario = dbContext.Usuario.Where(u => u.Email == email).FirstOrDefault();

            if (usuario != null)
            {
                usuario.Senha = senha;
            }

            return mapper.Map<UsuarioModel>(usuario);
        }

        public string? ObterEmailRedefinicaoSenha(string uuid)
        {
            var query = from rs in dbContext.RedefinicaoSenha
                        join u in dbContext.Usuario on rs.IdUsuario equals u.Id
                        where rs.Uuid == uuid
                        select u.Email;

            return query.FirstOrDefault();
        }

        public void RemoverUuidRedefinicaoSenha(string uuid)
        {
            var registro = dbContext.RedefinicaoSenha.Where(rs => rs.Uuid == uuid).FirstOrDefault();

            dbContext.RedefinicaoSenha.Remove(registro);
        }

        public void InserirDadosRecuperacao(string uuid, int idUsuario)
        {
            var newRs = new RedefinicaoSenha
            {
                Uuid = uuid,
                IdUsuario = idUsuario,
            };

            dbContext.RedefinicaoSenha.Add(newRs);
        }

        private async Task<Perfil?> RecuperaPerfilBasicoAsync()
        {
            return await dbContext.Perfis.Where(p => p.Tipo == TipoPerfil.Basico)
                .FirstOrDefaultAsync();
        }

        public async Task<ListaPaginada<Usuario>> ObterUsuariosAsync(PesquisaUsuarioFiltro filtro)
        {
            var query = dbContext.Usuario
                .Include(u => u.Municipio)
                .Include(u => u.Perfil)
                .Include(u => u.Empresa)
                .AsQueryable();

            if (filtro.Nome != null)
                query = query.Where(u => u.Nome.ToLower().Contains(filtro.Nome.ToLower()));

            if (filtro.PerfilId != null)
                query = query.Where(u => u.PerfilId == filtro.PerfilId);

            if (filtro.UfLotacao != null)
                query = query.Where(u => u.UfLotacao == filtro.UfLotacao);

            if (filtro.MunicipioId != null)
                query = query.Where(u => u.MunicipioId == filtro.MunicipioId);

            if (filtro.Empresa != null)
                query = query.Where(u => u.Empresa.RazaoSocial.Contains(filtro.Empresa));

            var total = await query.CountAsync();
            var items = await query
                .Skip(filtro.ItemsPorPagina * (filtro.Pagina - 1))
                .Take(filtro.ItemsPorPagina)
                .ToListAsync();

            return new ListaPaginada<Usuario>(items, filtro.Pagina, filtro.ItemsPorPagina, total);
        }
    }
}
