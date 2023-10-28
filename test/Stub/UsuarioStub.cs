using api.Usuarios;
using app.Entidades;
using api;
using System.Collections.Generic;
using System.Linq;

namespace test.Stub
{
    public class TesteUsuarioStub : UsuarioDTONovo
    {
        public int Id { get; set; }
        public string SenhaHash { get; set; }
    }

    public class UsuarioStub
    {
        public static IEnumerable<TesteUsuarioStub> Listar()
        {
            while (true)
            {
                yield return new TesteUsuarioStub()
                {
                    Nome = "teste " + Random.Shared.Next().ToString(),
                    Email = $"teste{Random.Shared.Next()}@email.com",
                    Senha = $"teste_senha_{Random.Shared.Next()}",
                };
            }
        }

        public UsuarioDTONovo RetornarUsuarioDnitDTO()
        {
            return new UsuarioDTONovo
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
            };
        }

        public UsuarioDTONovo RetornarUsuarioTerceiroDTO()
        {
            return new UsuarioDTONovo
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
            };
        }

        public UsuarioDnitNovo RetornarUsuarioDnit()
        {
            return new UsuarioDnitNovo
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
                UfLotacao = UF.DF
            };
        }

        public Usuario RetornarUsuarioDnitBanco()
        {
            return new Usuario
            {
                Email = "usuarioteste@gmail.com",
                Senha = "$2a$11$p0Q3r8Q7pBBcfoW.EIdvvuosHDfgr6TBBOxQvpnG18fLLlHjC/J6O",
                Nome = "Usuario Dnit",
                UfLotacao = UF.DF
            };
        }

        public UsuarioTerceiroNovo RetornarUsuarioTerceiro()
        {
            return new UsuarioTerceiroNovo
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
                CNPJ = "12345678901234"
            };
        }

        public Usuario RetornarUsuarioValidoLogin()
        {
            return new Usuario
            {
                Email = "usuarioteste@gmail.com",
                Senha = "$2a$11$p0Q3r8Q7pBBcfoW.EIdvvuosHDfgr6TBBOxQvpnG18fLLlHjC/J6O",
                Nome = "Usuario Dnit"
            };
        }

        public Usuario RetornarUsuarioInvalidoLogin()
        {
            return new Usuario
            {
                Email = "usuarioteste@gmail.com",
                Senha = "$2a$11$p0Q3r8Q7pBBcfoW.EIdvvuosHDfgr6TBBOxQvpnG18fLLlHjC/J68",
                Nome = "Usuario Dnit"
            };
        }
    }
}
