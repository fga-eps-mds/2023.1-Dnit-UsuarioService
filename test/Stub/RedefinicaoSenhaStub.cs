using api.Senhas;

namespace test.Stub
{
    public class RedefinicaoSenhaStub
    {
        public RedefinicaoSenhaDTO ObterRedefinicaoSenhaDTO()
        {
            return new RedefinicaoSenhaDTO
            {
                Senha = "senha1234",
                UuidAutenticacao = "123e4567-e89b-12d3-a456-426655440000"
            };
        }

        public RedefinicaoSenhaModel ObterRedefinicaoSenha()
        {
            return new RedefinicaoSenhaModel
            {
                Senha = "senha1234",
                UuidAutenticacao = "123e4567-e89b-12d3-a456-426655440000"
            };
        }
    }
}
