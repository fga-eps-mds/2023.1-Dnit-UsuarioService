using api.Usuarios;
using app.Entidades;
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
        static readonly UF[] ListaUfs = Enum.GetValues<UF>();

        static private UF UfAleatoria()
        {
            return ListaUfs[Random.Shared.Next() % ListaUfs.Length];
        }

        public static IEnumerable<TesteUsuarioStub> Listar()
        {
            while (true)
            {
                yield return new TesteUsuarioStub()
                {
                    Nome = "teste " + Random.Shared.Next().ToString(),
                    Email = $"teste{Random.Shared.Next()}@email.com",
                    Senha = $"teste_senha_{Random.Shared.Next()}",
                    UfLotacao = UfAleatoria(),
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
                UfLotacao = UfAleatoria(),
            };
        }

        public UsuarioDTO RetornarUsuarioTerceiroDTO()
        {
            return new UsuarioDTO
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
                UfLotacao = UfAleatoria()
            };
        }

        public UsuarioDnit RetornarUsuarioDnit()
        {
            return new UsuarioDnit
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
                UfLotacao = UfAleatoria()
            };
        }

        public Usuario RetornarUsuarioDnitBanco()
        {
            return new Usuario
            {
                Email = "usuarioteste@gmail.com",
                Senha = "$2a$11$p0Q3r8Q7pBBcfoW.EIdvvuosHDfgr6TBBOxQvpnG18fLLlHjC/J6O",
                Nome = "Usuario Dnit",
                UfLotacao = UfAleatoria()
            };
        }

        public UsuarioTerceiro RetornarUsuarioTerceiro()
        {
            return new UsuarioTerceiro
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
                CNPJ = "12345678901234",
                UfLotacao = UfAleatoria()
            };
        }

        public Usuario RetornarUsuarioValidoLogin()
        {
            return new Usuario
            {
                Email = "usuarioteste@gmail.com",
                Senha = "$2a$11$p0Q3r8Q7pBBcfoW.EIdvvuosHDfgr6TBBOxQvpnG18fLLlHjC/J6O",
                Nome = "Usuario Dnit",
                UfLotacao = UfAleatoria()
            };
        }

        public Usuario RetornarUsuarioInvalidoLogin()
        {
            return new Usuario
            {
                Email = "usuarioteste@gmail.com",
                Senha = "$2a$11$p0Q3r8Q7pBBcfoW.EIdvvuosHDfgr6TBBOxQvpnG18fLLlHjC/J68",
                Nome = "Usuario Dnit",
                UfLotacao = UfAleatoria()
            };
        }
    }
}
