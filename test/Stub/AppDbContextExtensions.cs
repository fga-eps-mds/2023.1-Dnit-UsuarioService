using api;
using app.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace test.Stub
{
    public static class AppDbContextExtensions
    {
        public static List<TesteUsuarioStub> PopulaUsuarios(this AppDbContext context, int quantidade, bool includePerfil = false)
        {
            var usuariosTeste = UsuarioStub.Listar().Take(quantidade).ToList();
            foreach (var usuarioDto in usuariosTeste)
            {
                var salt = BCrypt.Net.BCrypt.GenerateSalt();

                usuarioDto.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha, salt);

                var usuario = new Usuario()
                {
                    Id = Random.Shared.Next(),
                    Email = usuarioDto.Email,
                    Nome = usuarioDto.Nome,
                    Senha = usuarioDto.SenhaHash,
                    UfLotacao = UF.DF,
                };

                usuarioDto.Id = usuario.Id;

                if (includePerfil)
                {
                    var perfil = new Perfil
                    {
                        Id = Guid.NewGuid(),
                        Nome = "Teste",
                        PerfilPermissoes = new()
                        {
                            new PerfilPermissao
                            {
                                Id = Guid.NewGuid(),
                                Permissao = Permissao.EscolaCadastrar,
                            }
                        }
                    };
                    usuario.Perfil = perfil;
                    context.Add(perfil);
                }

                context.Add(usuario);
            }
            context.SaveChanges();
            return usuariosTeste;
        }
        public static List<Empresa> PopulaEmpresa(this AppDbContext context, int quantidade)
        {
            var empresaTest = EmpresaStub.Listar().Take(quantidade).ToList();
            context.AddRange(empresaTest);
            context.SaveChanges();
            return empresaTest;



        }
        
    }
}
