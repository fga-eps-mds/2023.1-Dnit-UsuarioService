using dominio;

namespace test.Stub
{
    public class UsuarioStub
    {
        public UsuarioDTO RetornarUsuarioDnitDTO()
        {
            return new UsuarioDTO
            {
                Email = "usuarioteste@gmail.com",
                Senha = "senha1234",
                Nome = "Usuario Dnit",
                UF = 27
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
                UF = 27
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
