using api.Usuarios;
using api;
using System.Collections.Generic;
using System.Linq;

namespace test.Stub
{
    public class TesteUsuarioStub : UsuarioDTO
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
                    CNPJ = string.Join("", Enumerable.Range(0, 11).Select(_ => Random.Shared.Next() % 10)),
                    Email = $"teste{Random.Shared.Next()}@email.com",
                    Senha = $"teste_senha_{Random.Shared.Next()}",
                };
            }
        }

        public UsuarioDTO RetornarUsuarioDnitDTO()
        {
            return new UsuarioDTO
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
                UfLotacao = UF.DF
            };
        }

        public UsuarioDTO RetornarUsuarioTerceiroDTO()
        {
            return new UsuarioDTO
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
                CNPJ = "12345678901234"
            };
        }

        public UsuarioDnit RetornarUsuarioDnit()
        {
            return new UsuarioDnit
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
                UfLotacao = UF.DF
            };
        }

        public UsuarioDTO RetornarUsuarioSenhaErrada()
        {
            return new UsuarioDTO
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
                UfLotacao = UF.DF
            };
        }

        public UsuarioTerceiro RetornarUsuarioTerceiro()
        {
            return new UsuarioTerceiro
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
                CNPJ = "12345678901234"
            };
        }

        public UsuarioModel RetornarUsuarioValidoLogin()
        {
            return new UsuarioModel
            {
                Email = "usuarioteste@gmail.com",
                Senha = "$2a$11$p0Q3r8Q7pBBcfoW.EIdvvuosHDfgr6TBBOxQvpnG18fLLlHjC/J6O",
                Nome = "Usuario Dnit"
            };
        }

        public UsuarioModel RetornarUsuarioInvalidoLogin()
        {
            return new UsuarioModel
            {
                Email = "usuarioteste@gmail.com",
                Senha = "$2a$11$p0Q3r8Q7pBBcfoW.EIdvvuosHDfgr6TBBOxQvpnG18fLLlHjC/J68",
                Nome = "Usuario Dnit"
            };
        }
    }
}
