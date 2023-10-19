using app.Entidades;
using Dapper;
using api.Usuarios;
using api.Senhas;
using app.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Npgsql;

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
       
        public UsuarioModel? ObterUsuario(string email)
        {            
            var query = dbContext.Usuario.Where(u => u.Email == email).FirstOrDefault();
            
            return mapper.Map<UsuarioModel>(query);
        }

        public async Task<Usuario?> ObterUsuarioAsync(string email, bool includePerfil = false)
        {
            var query = dbContext.Usuario.AsQueryable();

            if (includePerfil)
            {
                query = query.Include(u => u.Perfil).ThenInclude(p => p.PerfilPermissoes);
            }

            return await query.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public void CadastrarUsuarioDnit(UsuarioDnit usuario)
        {
            var novoUsuario = new Usuario
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                UfLotacao = usuario.UfLotacao
            };

            dbContext.Add(novoUsuario);            
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

        public void CadastrarUsuarioTerceiro(UsuarioTerceiro usuarioTerceiro)
        {
            var empresa = dbContext.Empresa.Where(e => e.Cnpj == usuarioTerceiro.CNPJ).FirstOrDefault();

            List<Empresa> empresas = new List<Empresa>{ empresa };

            var novoUsuarioTerceiro = new Usuario
            {
                Nome = usuarioTerceiro.Nome,
                Email = usuarioTerceiro.Email,
                Senha = usuarioTerceiro.Senha,
                Empresas = empresas
            };

            dbContext.Usuario.Add(novoUsuarioTerceiro);
        }
    }
}
